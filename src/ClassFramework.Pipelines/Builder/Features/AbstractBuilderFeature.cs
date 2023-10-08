namespace ClassFramework.Pipelines.Builder.Features;

public class AbstractBuilderFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AbstractBuilderFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AbstractBuilderFeature(_formattableStringParser);
}

public class AbstractBuilderFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AbstractBuilderFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public void Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.IsBuilderForAbstractEntity)
        {
            context.Model
                .AddGenericTypeArguments("TBuilder", "TEntity")
                .AddGenericTypeArgumentConstraints($"where TEntity : {context.Context.SourceModel.FormatInstanceName(false, context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate)}")
                .AddGenericTypeArgumentConstraints($"where TBuilder : {_formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, context).GetValueOrThrow()}<TBuilder, TEntity>")
                .WithAbstract();
        }
        else if (context.Context.Settings.ClassSettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared)
        {
            context.Model.AddInterfaces(typeof(IValidatableObject));
        }
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AbstractBuilderFeatureBuilder(_formattableStringParser);
}
