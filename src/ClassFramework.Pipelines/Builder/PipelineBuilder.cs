namespace ClassFramework.Pipelines.Builder;

public interface IPipelineBuilder : IValidatableObject
{
    public Pipeline<ClassBuilder, PipelineBuilderContext> Build();
}

public class PipelineBuilder : PipelineBuilder<ClassBuilder, PipelineBuilderContext>, IPipelineBuilder
{
    public PipelineBuilder(
        IPartialFeatureBuilder partialFeatureBuilder,
        ISetNameFeatureBuilder setNameFeatureBuilder,
        IAbstractBuilderFeatureBuilder abstractBuilderFeatureBuilder,
        IBaseClassFeatureBuilder baseClassFeatureBuilder,
        IAddPropertiesFeatureBuilder addPropertiesFeatureBuilder)
    {
        AddFeatures
        (
            partialFeatureBuilder.IsNotNull(nameof(partialFeatureBuilder)).Convert<PipelineBuilderContext>(),
            setNameFeatureBuilder,
            abstractBuilderFeatureBuilder,
            baseClassFeatureBuilder,
            addPropertiesFeatureBuilder
        );
    }

    public PipelineBuilder(Pipeline<ClassBuilder, PipelineBuilderContext> source) : base(source)
    {
    }
}
