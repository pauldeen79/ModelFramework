namespace ClassFramework.Pipelines;

public class BuilderPipelineBuilder : PipelineBuilder<ClassBuilder, BuilderPipelineBuilderSettings>
{
    public BuilderPipelineBuilder()
    {
        var formattableStringParser = new FormattableStringParser
        (
            new IFormattableStringStateProcessor[]
            {
                new OpenSignProcessor(),
                new CloseSignProcessor(new[] { new ContextProcessor() }),
                new PlaceholderProcessor(),
                new ResultProcessor()
            }
        );

        AddFeatures
        (
            new MakePropertiesWritableFeatureBuilder(),
            new ChangeNameFeatureBuilder(formattableStringParser),
            new AbstractBuilderFeatureBuilder(formattableStringParser)
        );
    }

    public BuilderPipelineBuilder(Pipeline<ClassBuilder, BuilderPipelineBuilderSettings> source) : base(source)
    {
    }
}
