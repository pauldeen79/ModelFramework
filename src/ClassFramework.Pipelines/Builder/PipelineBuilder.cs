namespace ClassFramework.Pipelines.Builder;

public class PipelineBuilder : PipelineBuilder<ClassBuilder, BuilderContext>
{
    public PipelineBuilder(
        IEnumerable<ISharedFeatureBuilder> sharedFeatureBuilders,
        IEnumerable<IBuilderFeatureBuilder> builderFeatureBuilders)
    {
        AddFeatures(builderFeatureBuilders);
        AddFeatures(sharedFeatureBuilders);
    }

    public PipelineBuilder(Pipeline<ClassBuilder, BuilderContext> source) : base(source)
    {
    }
}
