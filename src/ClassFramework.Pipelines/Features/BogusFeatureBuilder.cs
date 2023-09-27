namespace ClassFramework.Pipelines.Features;

public class BogusFeatureBuilder : IBuilder<IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings>>
{
    public IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings> Build()
        => new BogusFeature();
}

public class BogusFeature : IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings>
{
    public void Process(PipelineContext<TypeBuilder, BuilderPipelineBuilderSettings> context)
    {
        throw new NotImplementedException();
    }

    public IBuilder<IPipelineFeature<TypeBuilder, BuilderPipelineBuilderSettings>> ToBuilder()
        => new BogusFeatureBuilder();
}
