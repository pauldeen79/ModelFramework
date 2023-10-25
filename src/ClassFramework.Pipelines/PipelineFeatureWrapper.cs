namespace ClassFramework.Pipelines;

public class PipelineFeatureWrapper<TContext> : IPipelineFeature<ClassBuilder, TContext>
{
    private readonly Func<PartialFeature> _featureCreateDelegate;

    public PipelineFeatureWrapper(Func<PartialFeature> featureCreateDelegate)
        => _featureCreateDelegate = featureCreateDelegate.IsNotNull(nameof(featureCreateDelegate));

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, TContext> context)
    {
        context = context.IsNotNull(nameof(context));

        return _featureCreateDelegate().Process(new PipelineContext<ClassBuilder>(context.Model));
    }

    public IBuilder<IPipelineFeature<ClassBuilder, TContext>> ToBuilder()
        => new PipelineFeatureWrapperBuilder<TContext>(_featureCreateDelegate);
}

public class PipelineFeatureWrapperBuilder<TContext> : IBuilder<IPipelineFeature<ClassBuilder, TContext>>
{
    private readonly Func<PartialFeature> _featureCreateDelegate;

    public PipelineFeatureWrapperBuilder(Func<PartialFeature> featureCreateDelegate)
        => _featureCreateDelegate = featureCreateDelegate.IsNotNull(nameof(featureCreateDelegate));

    public IPipelineFeature<ClassBuilder, TContext> Build()
        => new PipelineFeatureWrapper<TContext>(_featureCreateDelegate);
}
