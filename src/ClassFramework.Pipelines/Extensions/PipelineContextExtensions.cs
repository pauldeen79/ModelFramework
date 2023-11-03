namespace ClassFramework.Pipelines.Extensions;

public static class PipelineContextExtensions
{
    public static Result<string> CreateEntityInstanciation(this PipelineContext<ClassBuilder, BuilderContext> context, IFormattableStringParser formattableStringParser, string classNameSuffix)
    {
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));

        var customEntityInstanciation = context.Context.SourceModel.Metadata.GetStringValue(MetadataNames.CustomBuilderEntityInstanciation);
        if (!string.IsNullOrEmpty(customEntityInstanciation))
        {
            return formattableStringParser.Parse(customEntityInstanciation, context.Context.FormatProvider, context);
        }

        var constructorsContainer = context.Context.SourceModel as IConstructorsContainer;

        if (constructorsContainer is null)
        {
            return Result.Invalid<string>("Cannot create an instance of a type that does not have constructors");
        }

        if (context.Context.SourceModel is Class cls && cls.Abstract)
        {
            return Result.Invalid<string>("Cannot create an instance of an abstract class");
        }

        var hasPublicParameterlessConstructor = constructorsContainer.HasPublicParameterlessConstructor();
        var openSign = GetImmutableBuilderPocoOpenSign(hasPublicParameterlessConstructor && context.Context.SourceModel.Properties.Any());
        var closeSign = GetImmutableBuilderPocoCloseSign(hasPublicParameterlessConstructor && context.Context.SourceModel.Properties.Any());

        var parametersResult = GetConstructionMethodParameters(context, formattableStringParser, hasPublicParameterlessConstructor);

        if (!parametersResult.IsSuccessful())
        {
            return parametersResult;
        }

        return Result.Success($"new {context.Context.SourceModel.GetFullName()}{classNameSuffix}{context.Context.SourceModel.GetGenericTypeArgumentsString()}{openSign}{parametersResult.Value}{closeSign}");
    }

    private static Result<string> GetConstructionMethodParameters(PipelineContext<ClassBuilder, BuilderContext> context, IFormattableStringParser formattableStringParser, bool hasPublicParameterlessConstructor)
    {
        var properties = context.Context.SourceModel.GetImmutableBuilderConstructorProperties(context.Context);

        var defaultValueDelegate = hasPublicParameterlessConstructor
            ? new Func<ClassProperty, string>(p => "{Name} = {Name}")
            : new Func<ClassProperty, string>(p => "{Name}");

        var results = properties.Select
        (
            p => formattableStringParser.Parse
            (
                p.Metadata.GetStringValue(MetadataNames.CustomBuilderMethodParameterExpression, defaultValueDelegate(p)),
                context.Context.FormatProvider,
                new ParentChildContext<BuilderContext, ClassProperty>(context, p)
            )
        ).TakeWhileWithFirstNonMatching(x => x.IsSuccessful()).ToArray();

        var error = Array.Find(results, x => !x.IsSuccessful());
        if (error is not null)
        {
            return error;
        }

        return Result.Success(string.Join(", ", results.Select(x => x.Value)));
    }

    private static string GetImmutableBuilderPocoCloseSign(bool poco)
        => poco
            ? " }"
            : ")";

    private static string GetImmutableBuilderPocoOpenSign(bool poco)
        => poco
            ? " { "
            : "(";
}
