namespace ClassFramework.Pipelines.Entity.Features;

public class AddGenericsFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, EntityContext> Build()
        => new AddGenericsFeature();
}

public class AddGenericsFeature : IPipelineFeature<ClassBuilder, EntityContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model
            .AddGenericTypeArguments(context.Context.SourceModel.GenericTypeArguments)
            .AddGenericTypeArgumentConstraints(context.Context.SourceModel.GenericTypeArgumentConstraints);

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new AddGenericsFeatureBuilder();
}
