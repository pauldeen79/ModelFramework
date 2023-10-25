namespace ClassFramework.Pipelines.Shared.Features;

public class PartialFeatureBuilder : ISharedFeatureBuilder
{
    public IPipelineFeature<ClassBuilder> Build()
        => new PartialFeature();

    public IBuilder<IPipelineFeature<ClassBuilder, TContext>> BuildFor<TContext>()
        => new PartialFeatureBuilder<TContext>();
}

public class PartialFeature : IPipelineFeature<ClassBuilder>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.Partial = true;

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder>> ToBuilder()
        => new PartialFeatureBuilder();
}

public class PartialFeatureBuilder<TContext> : IBuilder<IPipelineFeature<ClassBuilder, TContext>>
{
    public IPipelineFeature<ClassBuilder, TContext> Build()
        => new PipelineFeatureWrapper<TContext>(() => new PartialFeature());
}
