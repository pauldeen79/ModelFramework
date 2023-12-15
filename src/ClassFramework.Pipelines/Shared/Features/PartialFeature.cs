namespace ClassFramework.Pipelines.Shared.Features;

public class PartialFeatureBuilder<TModel> : ISharedFeatureBuilder<TModel>
    where TModel : ITypeBuilder
{
    public IPipelineFeature<TModel> Build()
        => new PartialFeature<TModel>();

    public IBuilder<IPipelineFeature<TModel, TContext>> BuildFor<TContext>()
        => new PartialFeatureBuilder<TModel, TContext>();
}

public class PartialFeature<TModel> : IPipelineFeature<TModel>
    where TModel : ITypeBuilder
{
    public Result<TModel> Process(PipelineContext<TModel> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.Partial = true;

        return Result.Continue<TModel>();
    }

    public IBuilder<IPipelineFeature<TModel>> ToBuilder()
        => new PartialFeatureBuilder<TModel>();
}

public class PartialFeatureBuilder<TModel, TContext> : IBuilder<IPipelineFeature<TModel, TContext>>
    where TModel : ITypeBuilder
{
    public IPipelineFeature<TModel, TContext> Build()
        => new PipelineFeatureWrapper<TModel, TContext>(() => new PartialFeature<TModel>());
}
