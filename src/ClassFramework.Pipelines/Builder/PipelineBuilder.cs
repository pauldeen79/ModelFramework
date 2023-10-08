namespace ClassFramework.Pipelines.Builder;

public class PipelineBuilder : PipelineBuilder<ClassBuilder, PipelineBuilderContext>
{
    public PipelineBuilder(
        IEnumerable<IContextBaseBuilder> baseContextFeatureBuilders,
        IEnumerable<IBuilderFeatureBuilder> builderContextFeatureBuilders)
    {
        baseContextFeatureBuilders.IsNotNull(nameof(baseContextFeatureBuilders));
        builderContextFeatureBuilders.IsNotNull(nameof(builderContextFeatureBuilders));

        AddFeatures(baseContextFeatureBuilders.Select(x => x.Convert<PipelineBuilderContext>()));
        AddFeatures(builderContextFeatureBuilders);
    }

    public PipelineBuilder(Pipeline<ClassBuilder, PipelineBuilderContext> source) : base(source)
    {
    }
}
