namespace ClassFramework.Pipelines.Builder;

public interface IPipelineBuilder : IValidatableObject
{
    public Pipeline<ClassBuilder, PipelineBuilderContext> Build();
}

public class PipelineBuilder : PipelineBuilder<ClassBuilder, PipelineBuilderContext>, IPipelineBuilder
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
