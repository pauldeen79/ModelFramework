namespace ClassFramework.Pipelines.Reflection;

public class PipelineBuilder : PipelineBuilder<TypeBaseBuilder, ReflectionContext>
{
    public PipelineBuilder(
        IEnumerable<ISharedFeatureBuilder<TypeBaseBuilder>> sharedFeatureBuilders,
        IEnumerable<IReflectionFeatureBuilder> reflectionFeatureBuilders)
    {
        AddFeatures(reflectionFeatureBuilders);
        AddFeatures(sharedFeatureBuilders.Select(x => x.BuildFor<ReflectionContext>()));
    }

    public PipelineBuilder(Pipeline<TypeBaseBuilder, ReflectionContext> source) : base(source)
    {
    }
}
