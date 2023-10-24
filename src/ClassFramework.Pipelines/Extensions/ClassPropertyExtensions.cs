namespace ClassFramework.Pipelines.Extensions;

public static class ClassPropertyExtensions
{
    public static string GetDefaultValue(this ClassProperty property, bool enableNullableReferenceTypes, CultureInfo cultureInfo)
    {
        var md = property.Metadata.FirstOrDefault(x => x.Name == MetadataNames.CustomBuilderDefaultValue);
        if (md is not null && md.Value is not null)
        {
            if (md.Value is Literal literal && literal.Value is not null)
            {
                return literal.Value;
            }

            return md.Value.CsharpFormat(cultureInfo);
        }

        return property.TypeName.GetDefaultValue(property.IsNullable, property.IsValueType, enableNullableReferenceTypes);
    }

    public static string GetNullCheckSuffix(this ClassProperty property, string name, bool addNullChecks)
    {
        if (!addNullChecks || property.IsNullable || property.IsValueType)
        {
            return string.Empty;
        }

        return $" ?? throw new {typeof(ArgumentNullException).FullName}(nameof({name}))";
    }

    public static string GetInitializationName(this ClassProperty property, BuilderContext context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Settings.GenerationSettings.AddNullChecks
            && context.Settings.ClassSettings.ConstructorSettings.OriginalValidateArguments != ArgumentValidationType.Shared)
        {
            return $"_{property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo())}";
        }

        return property.Name;
    }

    public static string GetImmutableBuilderClassConstructorInitializer(this ClassProperty property, PipelineContext<ClassBuilder, BuilderContext> context, IFormattableStringParser formattableStringParser)
    {
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
        context = context.IsNotNull(nameof(context));

        return formattableStringParser
            .Parse
            (
                property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                context.Context.FormatProvider,
                new ParentChildContext<BuilderContext, ClassProperty>(context, property)
            )
            .GetValueOrThrow()
            .FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName)
            .GetCollectionInitializeStatement()
            .GetCsharpFriendlyTypeName();
    }

    public static ClassProperty EnsureParentTypeFullName(this ClassProperty property, Class parentClass)
        => new ClassPropertyBuilder(property)
            .WithParentTypeFullName(property.ParentTypeFullName.WhenNullOrEmpty(() => parentClass.IsNotNull(nameof(parentClass)).GetFullName().WithoutGenerics()))
            .Build();
}
