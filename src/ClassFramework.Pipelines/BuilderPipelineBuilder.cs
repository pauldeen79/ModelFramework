namespace ClassFramework.Pipelines;

public class BuilderPipelineBuilder : PipelineBuilder<TypeBuilder, BuilderPipelineBuilderSettings>
{
    public BuilderPipelineBuilder()
    {
        AddFeatures
        (
            new BogusFeatureBuilder()
        );
    }

    public BuilderPipelineBuilder(Pipeline<TypeBuilder, BuilderPipelineBuilderSettings> source) : base(source)
    {
    }
}
