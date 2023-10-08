namespace ClassFramework.Pipelines.Builder;

public class PipelineBuilder : PipelineBuilder<ClassBuilder, BuilderContext>
{
    public PipelineBuilder(
        IEnumerable<ISharedFeatureBuilder> sharedFeatureBuilders,
        IEnumerable<IBuilderFeatureBuilder> builderFeatureBuilders)
    {
        AddFeatures(sharedFeatureBuilders);
        AddFeatures(builderFeatureBuilders);
    }

    public PipelineBuilder(Pipeline<ClassBuilder, BuilderContext> source) : base(source)
    {
    }
}
