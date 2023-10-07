namespace ClassFramework.Pipelines.Shared.Features;

public class PartialFeatureBuilder : IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext>>
{
    public IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext> Build()
        => new PartialFeature();
}

public class PartialFeature : IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext>
{
    public void Process(PipelineContext<ClassBuilder, BuilderPipelineBuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.Partial = true;
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderPipelineBuilderContext>> ToBuilder()
        => new PartialFeatureBuilder();
}
