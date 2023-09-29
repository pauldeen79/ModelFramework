namespace ClassFramework.Pipelines;

public class BuilderPipelineBuilder : PipelineBuilder<TypeBuilder, BuilderPipelineBuilderSettings>
{
    public BuilderPipelineBuilder()
    {
        var formattableStringParser = new FormattableStringParser(new IFormattableStringStateProcessor[] { new OpenSignProcessor(), new CloseSignProcessor(new[] { new ContextProcessor() }), new PlaceholderProcessor(), new ResultProcessor()  });

        AddFeatures
        (
            new MakePropertiesWritableFeatureBuilder(),
            new ChangeNameFeatureBuilder(formattableStringParser)
        );
    }

    public BuilderPipelineBuilder(Pipeline<TypeBuilder, BuilderPipelineBuilderSettings> source) : base(source)
    {
    }
}
