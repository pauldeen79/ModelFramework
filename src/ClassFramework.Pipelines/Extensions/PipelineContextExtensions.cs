namespace ClassFramework.Pipelines.Extensions;

public static class PipelineContextExtensions
{
    public static string CreateEntityInstanciation(this PipelineContext<ClassBuilder, BuilderContext> context, IFormattableStringParser formattableStringParser, string classNameSuffix)
    {
        var constructorsContainer = context.Context.SourceModel as IConstructorsContainer;

        if (constructorsContainer is null)
        {
            throw new InvalidOperationException("Cannot create an instance of a type that does not have constructors");
        }

        if (context.Context.SourceModel is Class cls && cls.Abstract)
        {
            throw new InvalidOperationException("Cannot create an instance of an abstract class");
        }

        var hasPublicParameterlessConstructor = constructorsContainer.HasPublicParameterlessConstructor();
        var openSign = GetImmutableBuilderPocoOpenSign(hasPublicParameterlessConstructor && context.Context.SourceModel.Properties.Any());
        var closeSign = GetImmutableBuilderPocoCloseSign(hasPublicParameterlessConstructor && context.Context.SourceModel.Properties.Any());
        return $"new {context.Context.SourceModel.GetFullName()}{classNameSuffix}{context.Context.SourceModel.GetGenericTypeArgumentsString()}{openSign}{GetConstructionMethodParameters(context, formattableStringParser, hasPublicParameterlessConstructor)}{closeSign}";
    }

    private static string GetConstructionMethodParameters(PipelineContext<ClassBuilder, BuilderContext> context, IFormattableStringParser formattableStringParser, bool hasPublicParameterlessConstructor)
    {
        var properties = context.Context.SourceModel.GetImmutableBuilderConstructorProperties(context.Context);

        var defaultValueDelegate = hasPublicParameterlessConstructor
            ? new Func<ClassProperty, string>(p => "{Name} = {Name}")
            : new Func<ClassProperty, string>(p => "{Name}");

        return string.Join
        (
            ", ",
            properties.Select
            (
                p => formattableStringParser.Parse
                (
                    p.Metadata.GetStringValue(MetadataNames.CustomBuilderMethodParameterExpression, defaultValueDelegate(p)),
                    context.Context.FormatProvider,
                    new ParentChildContext<ClassProperty>(context, p)
                ).GetValueOrThrow()
            )
        );
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
