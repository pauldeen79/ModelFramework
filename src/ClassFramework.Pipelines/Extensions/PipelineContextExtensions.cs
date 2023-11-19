namespace ClassFramework.Pipelines.Extensions;

public static class PipelineContextExtensions
{
    public static Result<string> CreateEntityInstanciation(this PipelineContext<ClassBuilder, BuilderContext> context, IFormattableStringParser formattableStringParser, string classNameSuffix)
    {
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));

        var customEntityInstanciation = context.Context.Model.Metadata.GetStringValue(MetadataNames.CustomBuilderEntityInstanciation);
        if (!string.IsNullOrEmpty(customEntityInstanciation))
        {
            return formattableStringParser.Parse(customEntityInstanciation, context.Context.FormatProvider, context);
        }

        var constructorsContainer = context.Context.Model as IConstructorsContainer;

        if (constructorsContainer is null)
        {
            return Result.Invalid<string>("Cannot create an instance of a type that does not have constructors");
        }

        if (context.Context.Model is Class cls && cls.Abstract)
        {
            return Result.Invalid<string>("Cannot create an instance of an abstract class");
        }

        var hasPublicParameterlessConstructor = constructorsContainer.HasPublicParameterlessConstructor();
        var openSign = GetBuilderPocoOpenSign(hasPublicParameterlessConstructor && context.Context.Model.Properties.Count != 0);
        var closeSign = GetBuilderPocoCloseSign(hasPublicParameterlessConstructor && context.Context.Model.Properties.Count != 0);

        var parametersResult = GetConstructionMethodParameters(context, formattableStringParser, hasPublicParameterlessConstructor);

        if (!parametersResult.IsSuccessful())
        {
            return parametersResult;
        }

        var entityNamespace = context.Context.Model.Metadata.WithMappingMetadata(context.Context.Model.GetFullName().GetCollectionItemType().WhenNullOrEmpty(context.Context.Model.GetFullName()), context.Context.Settings.TypeSettings).GetStringValue(MetadataNames.CustomEntityNamespace, () => context.Context.Model.Namespace);
        var ns = string.IsNullOrEmpty(entityNamespace)
            ? string.Empty
            : $"{context.Context.MapNamespace(entityNamespace)}.";

        return Result.Success($"new {ns}{context.Context.Model.Name}{classNameSuffix}{context.Context.Model.GetGenericTypeArgumentsString()}{openSign}{parametersResult.Value}{closeSign}");
    }

    public static string CreateEntityChainCall(
        this PipelineContext<ClassBuilder, EntityContext> context,
        IFormattableStringParser formattableStringParser,
        bool baseClass)
    {
        context = context.IsNotNull(nameof(context));

        if (baseClass && context.Context.Settings.AddValidationCode == ArgumentValidationType.Shared)
        {
            return $"base({CreateImmutableClassCtorParameterNames(context, formattableStringParser)})";
        }

        return context.Context.Settings.InheritanceSettings.EnableInheritance && context.Context.Settings.InheritanceSettings.BaseClass is not null
            ? $"base({GetPropertyNamesConcatenated(context.Context.Settings.InheritanceSettings.BaseClass.Properties, context.Context.FormatProvider.ToCultureInfo())})"
            : context.Context.Model.GetCustomValueForInheritedClass(context.Context.Settings,
            cls => Result.Success($"base({GetPropertyNamesConcatenated(context.Context.Model.Properties.Where(x => x.ParentTypeFullName == cls.BaseClass), context.Context.FormatProvider.ToCultureInfo())})")).GetValueOrThrow(); // we can simply shortcut the result evaluation, because we are injecting the Success in the delegate
    }

    public static IEnumerable<ParameterBuilder> CreateImmutableClassCtorParameters(
        this PipelineContext<ClassBuilder, EntityContext> context,
        IFormattableStringParser formattableStringParser)
        => context.Context.Model.Properties
            .Select
            (
                property => new ParameterBuilder()
                    .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                    .WithTypeName
                    (
                        context.Context.MapTypeName(formattableStringParser.Parse
                        (
                            property.Metadata
                                .WithMappingMetadata(property.TypeName.GetCollectionItemType().WhenNullOrEmpty(property.TypeName), context.Context.Settings.TypeSettings)
                                .GetStringValue(MetadataNames.CustomImmutableArgumentType, () => property.TypeName),
                            context.Context.FormatProvider,
                            new ParentChildContext<EntityContext, ClassProperty>(context, property, context.Context.Settings)
                        ).GetValueOrThrow())
                        .FixCollectionTypeName(context.Context.Settings.ConstructorSettings.CollectionTypeName.WhenNullOrEmpty(typeof(IEnumerable<>).WithoutGenerics()))
                    )
                    .WithIsNullable(property.IsNullable)
                    .WithIsValueType(property.IsValueType)
            );
    
    private static string GetPropertyNamesConcatenated(IEnumerable<ClassProperty> properties, CultureInfo cultureInfo)
        => string.Join(", ", properties.Select(x => x.Name.ToPascalCase(cultureInfo).GetCsharpFriendlyName()));

    private static string CreateImmutableClassCtorParameterNames(
        PipelineContext<ClassBuilder, EntityContext> context,
        IFormattableStringParser formattableStringParser)
        => string.Join(", ", context.CreateImmutableClassCtorParameters(formattableStringParser).Select(x => x.Name.ToString().GetCsharpFriendlyName()));

    private static Result<string> GetConstructionMethodParameters(PipelineContext<ClassBuilder, BuilderContext> context, IFormattableStringParser formattableStringParser, bool hasPublicParameterlessConstructor)
    {
        var properties = context.Context.Model.GetBuilderConstructorProperties(context.Context);

        var results = properties.Select
        (
            property => new
            {
                property.Name,
                Source = property,
                Result = formattableStringParser.Parse
                (
                    property.Metadata
                        .WithMappingMetadata(property.TypeName.GetCollectionItemType().WhenNullOrEmpty(property.TypeName), context.Context.Settings.TypeSettings)
                        .GetStringValue(MetadataNames.CustomBuilderMethodParameterExpression, "[Name]"),
                    context.Context.FormatProvider,
                    new ParentChildContext<BuilderContext, ClassProperty>(context, property, context.Context.Settings)
                ),
                Suffix = property.GetSuffix(context.Context.Settings.GenerationSettings.EnableNullableReferenceTypes)
            }
        ).TakeWhileWithFirstNonMatching(x => x.Result.IsSuccessful()).ToArray();

        var error = Array.Find(results, x => !x.Result.IsSuccessful());
        if (error is not null)
        {
            return error.Result;
        }

        return Result.Success(string.Join(", ", results.Select(x => hasPublicParameterlessConstructor
            ? $"{x.Name} = {GetBuilderPropertyExpression(x.Result.Value, x.Source, x.Suffix)}"
            : GetBuilderPropertyExpression(x.Result.Value, x.Source, x.Suffix))));
    }

    private static string? GetBuilderPropertyExpression(this string? value, ClassProperty sourceProperty, string suffix)
    {
        if (value is null || !value.Contains("[Name]"))
        {
            return value;
        }

        if (value == "[Name]")
        {
            return sourceProperty.Name;
        }

        return sourceProperty.TypeName.IsCollectionTypeName()
            ? $"{sourceProperty.Name}{suffix}.Select(x => {value!.Replace("[Name]", "x").Replace("[NullableSuffix]", string.Empty)})"
            : value!.Replace("[Name]", sourceProperty.Name).Replace("[NullableSuffix]", suffix);
    }

    private static string GetBuilderPocoCloseSign(bool poco)
        => poco
            ? " }"
            : ")";

    private static string GetBuilderPocoOpenSign(bool poco)
        => poco
            ? " { "
            : "(";
}
