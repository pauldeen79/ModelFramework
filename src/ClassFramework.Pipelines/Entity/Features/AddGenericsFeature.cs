namespace ClassFramework.Pipelines.Entity.Features;

public class AddGenericsFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, EntityContext> Build()
        => new AddGenericsFeature();
}

public class AddGenericsFeature : IPipelineFeature<IConcreteTypeBuilder, EntityContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.GenericTypeArguments.AddRange(context.Context.SourceModel.GenericTypeArguments);
        context.Model.GenericTypeArgumentConstraints.AddRange(context.Context.SourceModel.GenericTypeArgumentConstraints);

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, EntityContext>> ToBuilder()
        => new AddGenericsFeatureBuilder();
}
