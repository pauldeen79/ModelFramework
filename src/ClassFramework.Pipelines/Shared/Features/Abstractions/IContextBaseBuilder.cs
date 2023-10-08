namespace ClassFramework.Pipelines.Shared.Features.Abstractions;

public interface IContextBaseBuilder : IBuilder<IPipelineFeature<ClassBuilder, BuilderContextBase>>
{
    IBuilder<IPipelineFeature<ClassBuilder, T>> Convert<T>() where T : BuilderContextBase;
}
