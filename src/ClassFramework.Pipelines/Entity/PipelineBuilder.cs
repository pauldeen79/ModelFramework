namespace ClassFramework.Pipelines.Entity;

public class PipelineBuilder : PipelineBuilder<ClassBuilder, EntityContext>
{
    public PipelineBuilder(
        IEnumerable<ISharedFeatureBuilder<ClassBuilder>> sharedFeatureBuilders,
        IEnumerable<IEntityFeatureBuilder> entityFeatureBuilders)
    {
        AddFeatures(entityFeatureBuilders);
        AddFeatures(sharedFeatureBuilders.Select(x => x.BuildFor<EntityContext>()));
    }

    public PipelineBuilder(Pipeline<ClassBuilder, EntityContext> source) : base(source)
    {
    }
}
