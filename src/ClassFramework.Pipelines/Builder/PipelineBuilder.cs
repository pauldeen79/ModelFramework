namespace ClassFramework.Pipelines.Builder;

public class PipelineBuilder : PipelineBuilder<ClassBuilder, BuilderContext>
{
    public PipelineBuilder(
        IEnumerable<ISharedFeatureBuilder> baseContextFeatureBuilders,
        IEnumerable<IBuilderFeatureBuilder> builderContextFeatureBuilders)
    {
        baseContextFeatureBuilders.IsNotNull(nameof(baseContextFeatureBuilders));
        builderContextFeatureBuilders.IsNotNull(nameof(builderContextFeatureBuilders));

        AddFeatures(baseContextFeatureBuilders);
        AddFeatures(builderContextFeatureBuilders);
    }

    public PipelineBuilder(Pipeline<ClassBuilder, BuilderContext> source) : base(source)
    {
    }
}
