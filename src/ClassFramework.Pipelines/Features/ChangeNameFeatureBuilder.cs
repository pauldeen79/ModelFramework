namespace ClassFramework.Pipelines.Features;

public class ChangeNameFeatureBuilder : IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderSettings>>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public ChangeNameFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderPipelineBuilderSettings> Build()
        => new ChangeNameFeature(_formattableStringParser);
}

public class ChangeNameFeature : IPipelineFeature<ClassBuilder, BuilderPipelineBuilderSettings>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public ChangeNameFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public void Process(PipelineContext<ClassBuilder, BuilderPipelineBuilderSettings> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.Name = _formattableStringParser.Parse(context.Context.NameSettings.BuilderNameFormatString, CultureInfo.CurrentCulture, context).GetValueOrThrow();
        context.Model.Namespace = _formattableStringParser.Parse(context.Context.NameSettings.BuilderNamespaceFormatString, CultureInfo.CurrentCulture, context).GetValueOrThrow();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderSettings>> ToBuilder()
        => new ChangeNameFeatureBuilder(_formattableStringParser);
}
