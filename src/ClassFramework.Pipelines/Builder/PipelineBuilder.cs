namespace ClassFramework.Pipelines.Builder;

public class PipelineBuilder : PipelineBuilder<ClassBuilder, BuilderContext>
{
    public PipelineBuilder(
        IEnumerable<ISharedFeatureBuilder> sharedFeatureBuilders,
        IEnumerable<IBuilderFeatureBuilder> builderFeatureBuilders)
    {
        AddFeatures(builderFeatureBuilders.Where(x => x is ValidationFeatureBuilder)); // important to add validation first, so the model does not get altered when validation fails...
        AddFeatures(sharedFeatureBuilders);
        AddFeatures(builderFeatureBuilders.Where(x => x is not ValidationFeatureBuilder));
    }

    public PipelineBuilder(Pipeline<ClassBuilder, BuilderContext> source) : base(source)
    {
    }
}
