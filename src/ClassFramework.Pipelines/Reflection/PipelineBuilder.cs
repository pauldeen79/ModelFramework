namespace ClassFramework.Pipelines.Reflection;

public class PipelineBuilder : PipelineBuilder<TypeBaseBuilder, ReflectionContext>
{
    public PipelineBuilder(IEnumerable<IReflectionFeatureBuilder> reflectionFeatureBuilders)
    {
        AddFeatures(reflectionFeatureBuilders);
    }

    public PipelineBuilder(Pipeline<TypeBaseBuilder, ReflectionContext> source) : base(source)
    {
    }
}
