namespace ClassFramework.Pipelines.Entity;

public class PipelineBuilder : PipelineBuilder<TypeBaseBuilder, EntityContext>
{
    public PipelineBuilder(
        IEnumerable<ISharedFeatureBuilder<TypeBaseBuilder>> sharedFeatureBuilders,
        IEnumerable<IEntityFeatureBuilder> entityFeatureBuilders)
    {
        AddFeatures(entityFeatureBuilders);
        AddFeatures(sharedFeatureBuilders.Select(x => x.BuildFor<EntityContext>()));
    }

    public PipelineBuilder(Pipeline<TypeBaseBuilder, EntityContext> source) : base(source)
    {
    }
}
