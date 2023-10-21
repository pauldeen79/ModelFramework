namespace ClassFramework.Pipelines.Shared.Features;

public class PartialFeatureBuilder : ISharedFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new PartialFeature();
}

public class PartialFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    public Result<BuilderContext> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.Partial = true;

        return Result.Continue<BuilderContext>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new PartialFeatureBuilder();
}
