namespace ClassFramework.Pipelines.Builder.Features;

public class SetNameFeatureBuilder : IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext>>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public SetNameFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext> Build()
        => new SetNameFeature(_formattableStringParser);
}

public class SetNameFeature : IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public SetNameFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public void Process(PipelineContext<ClassBuilder, BuilderPipelineBuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.Name = _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, context).GetValueOrThrow();
        context.Model.Namespace = _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNamespaceFormatString, context.Context.FormatProvider, context).GetValueOrThrow();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext>> ToBuilder()
        => new SetNameFeatureBuilder(_formattableStringParser);
}
