namespace ClassFramework.Pipelines.Shared.Features;

public class PartialFeatureBuilder<T> : IBuilder<IPipelineFeature<ClassBuilder, T>> where T : BuilderContextBase
{
    public IPipelineFeature<ClassBuilder, T> Build()
        => new PartialFeature<T>();
}

public class PartialFeatureBuilder : IContextBaseBuilder
{
    public virtual IPipelineFeature<ClassBuilder, BuilderContextBase> Build()
        => new PartialFeature<BuilderContextBase>();

    public IBuilder<IPipelineFeature<ClassBuilder, T>> Convert<T>() where T : BuilderContextBase
        => new PartialFeatureBuilder<T>();
}

public class PartialFeature<T> : IPipelineFeature<ClassBuilder, T> where T : BuilderContextBase
{
    public void Process(PipelineContext<ClassBuilder, T> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.Partial = true;
    }

    public IBuilder<IPipelineFeature<ClassBuilder, T>> ToBuilder()
        => new PartialFeatureBuilder().Convert<T>();
}
