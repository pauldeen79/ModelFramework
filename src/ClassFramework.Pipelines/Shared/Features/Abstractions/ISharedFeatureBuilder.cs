namespace ClassFramework.Pipelines.Shared.Features.Abstractions;

public interface ISharedFeatureBuilder<TModel> : IBuilder<IPipelineFeature<TModel>>
{
    IBuilder<IPipelineFeature<TModel, TContext>> BuildFor<TContext>();
}
