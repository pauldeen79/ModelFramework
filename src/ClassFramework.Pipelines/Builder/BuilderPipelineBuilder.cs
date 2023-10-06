namespace ClassFramework.Pipelines.Builder;

public class BuilderPipelineBuilder : PipelineBuilder<ClassBuilder, BuilderPipelineBuilderContext>
{
    public BuilderPipelineBuilder()
    {
        var formattableStringParser = FormattableStringParser.Create(new ContextProcessor());

        AddFeatures
        (
            new AddPropertiesFeatureBuilder(),
            new SetNameFeatureBuilder(formattableStringParser),
            new AbstractBuilderFeatureBuilder(formattableStringParser)
        );
    }

    public BuilderPipelineBuilder(Pipeline<ClassBuilder, BuilderPipelineBuilderContext> source) : base(source)
    {
    }
}
