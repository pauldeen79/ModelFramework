namespace ClassFramework.Pipelines.Entity.Features;

public class AddGenericsFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, EntityContext> Build()
        => new AddGenericsFeature();
}

public class AddGenericsFeature : IPipelineFeature<TypeBaseBuilder, EntityContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.GenericTypeArguments.AddRange(context.Context.SourceModel.GenericTypeArguments);
        context.Model.GenericTypeArgumentConstraints.AddRange(context.Context.SourceModel.GenericTypeArgumentConstraints);

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, EntityContext>> ToBuilder()
        => new AddGenericsFeatureBuilder();
}
