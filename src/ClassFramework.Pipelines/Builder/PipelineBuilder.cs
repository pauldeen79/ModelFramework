namespace ClassFramework.Pipelines.Builder;

public class PipelineBuilder : PipelineBuilder<ClassBuilder, BuilderContext>
{
    public PipelineBuilder(
        IEnumerable<ISharedFeatureBuilder<ClassBuilder>> sharedFeatureBuilders,
        IEnumerable<IBuilderFeatureBuilder> builderFeatureBuilders)
    {
        AddFeatures(builderFeatureBuilders);
        AddFeatures(sharedFeatureBuilders.Select(x => x.BuildFor<BuilderContext>()));
    }

    public PipelineBuilder(Pipeline<ClassBuilder, BuilderContext> source) : base(source)
    {
    }
}
