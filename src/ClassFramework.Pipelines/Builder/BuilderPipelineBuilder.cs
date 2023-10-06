namespace ClassFramework.Pipelines.Builder;

public class BuilderPipelineBuilder : PipelineBuilder<ClassBuilder, BuilderPipelineBuilderSettings>
{
    public BuilderPipelineBuilder()
    {
        var formattableStringParser = FormattableStringParser.Create(new ContextProcessor());

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
