namespace ClassFramework.Pipelines.Shared.Features.Abstractions;

public interface ISharedFeatureBuilder : IBuilder<IPipelineFeature<ClassBuilder>>
{
    IBuilder<IPipelineFeature<ClassBuilder, TContext>> BuildFor<TContext>();
}
