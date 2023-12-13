namespace ClassFramework.Pipelines.Entity.Features;

public class AbstractEntityFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, EntityContext> Build()
        => new AbstractEntityFeature();
}

public class AbstractEntityFeature : IPipelineFeature<TypeBaseBuilder, EntityContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        var cls = context.Model as ClassBuilder;
        if (cls is not null)
        {
            cls.WithAbstract(context.Context.IsAbstract);
        }

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, EntityContext>> ToBuilder()
        => new AbstractEntityFeatureBuilder();
}
