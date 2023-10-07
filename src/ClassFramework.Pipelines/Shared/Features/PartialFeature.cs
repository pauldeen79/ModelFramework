namespace ClassFramework.Pipelines.Shared.Features;

public class PartialFeatureBuilder : IBuilder<IPipelineFeature<ClassBuilder, PipelineBuilderContext>>
{
    public IPipelineFeature<ClassBuilder, PipelineBuilderContext> Build()
        => new PartialFeature();
}

public class PartialFeature : IPipelineFeature<ClassBuilder, PipelineBuilderContext>
{
    public void Process(PipelineContext<ClassBuilder, PipelineBuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.Partial = true;
    }

    public IBuilder<IPipelineFeature<ClassBuilder, PipelineBuilderContext>> ToBuilder()
        => new PartialFeatureBuilder();
}
