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
        var openSign = GetBuilderPocoOpenSign(hasPublicParameterlessConstructor && context.Context.Model.Properties.Any());
        var closeSign = GetBuilderPocoCloseSign(hasPublicParameterlessConstructor && context.Context.Model.Properties.Any());

        var parametersResult = GetConstructionMethodParameters(context, formattableStringParser, hasPublicParameterlessConstructor);

        if (!parametersResult.IsSuccessful())
        {
            return parametersResult;
        }

        return Result.Success($"new {context.Context.Model.GetFullName()}{classNameSuffix}{context.Context.Model.GetGenericTypeArgumentsString()}{openSign}{parametersResult.Value}{closeSign}");
    }

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
