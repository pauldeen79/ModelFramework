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

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.IsBuilderForAbstractEntity)
        {
            context.Model
                .AddGenericTypeArguments("TBuilder", "TEntity")
                .AddGenericTypeArgumentConstraints($"where TEntity : {context.Context.SourceModel.GetFullName()}")
                .AddGenericTypeArgumentConstraints($"where TBuilder : {_formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, context).GetValueOrThrow()}<TBuilder, TEntity>")
                .WithAbstract();
        }

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AbstractBuilderFeatureBuilder(_formattableStringParser);
}
