namespace ClassFramework.Pipelines.Reflection;

public class PipelineBuilder : PipelineBuilder<ClassBuilder, ReflectionContext>
{
    public PipelineBuilder(
        IEnumerable<ISharedFeatureBuilder> sharedFeatureBuilders,
        IEnumerable<IReflectionFeatureBuilder> reflectionFeatureBuilders)
    {
        AddFeatures(reflectionFeatureBuilders);
        AddFeatures(sharedFeatureBuilders.Select(x => x.BuildFor<ReflectionContext>()));
    }

    public PipelineBuilder(Pipeline<ClassBuilder, ReflectionContext> source) : base(source)
    {
    }
}
