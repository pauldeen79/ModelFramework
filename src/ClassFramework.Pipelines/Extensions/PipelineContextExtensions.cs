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

        var defaultValueDelegate = hasPublicParameterlessConstructor
            ? new Func<ClassProperty, string>(p => "{Name} = {Name}")
            : new Func<ClassProperty, string>(p => "{Name}");

        var results = properties.Select
        (
            property => formattableStringParser.Parse
            (
                property.Metadata.GetStringValue(MetadataNames.CustomBuilderMethodParameterExpression, defaultValueDelegate(property)),
                context.Context.FormatProvider,
                new ParentChildContext<BuilderContext, ClassProperty>(context, property, context.Context.Settings.GenerationSettings)
            )
        ).TakeWhileWithFirstNonMatching(x => x.IsSuccessful()).ToArray();

        var error = Array.Find(results, x => !x.IsSuccessful());
        if (error is not null)
        {
            return error;
        }

        return Result.Success(string.Join(", ", results.Select(x => x.Value)));
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
