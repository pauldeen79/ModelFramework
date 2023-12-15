namespace ClassFramework.Pipelines.Builder;

public class PipelineBuilder : PipelineBuilder<IConcreteTypeBuilder, BuilderContext>
{
    public PipelineBuilder(
        IEnumerable<ISharedFeatureBuilder<IConcreteTypeBuilder>> sharedFeatureBuilders,
        IEnumerable<IBuilderFeatureBuilder> builderFeatureBuilders)
    {
        AddFeatures(builderFeatureBuilders);
        AddFeatures(sharedFeatureBuilders.Select(x => x.BuildFor<BuilderContext>()));
    }

    public PipelineBuilder(Pipeline<IConcreteTypeBuilder, BuilderContext> source) : base(source)
    {
    }
}
