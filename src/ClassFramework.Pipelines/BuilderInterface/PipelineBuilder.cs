namespace ClassFramework.Pipelines.BuilderInterface;

public class PipelineBuilder : PipelineBuilder<IConcreteTypeBuilder, BuilderInterfaceContext>
{
    public PipelineBuilder(
        IEnumerable<ISharedFeatureBuilder> sharedFeatureBuilders,
        IEnumerable<IBuilderInterfaceFeatureBuilder> builderInterfaceFeatureBuilders)
    {
        AddFeatures(builderInterfaceFeatureBuilders);
        AddFeatures(sharedFeatureBuilders.Select(x => x.BuildFor<BuilderInterfaceContext>()));
    }
}
