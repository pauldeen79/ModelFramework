namespace ClassFramework.Pipelines.Entity.Features;

public class SetRecordFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, EntityContext> Build()
        => new SetRecordFeature();
}

public class SetRecordFeature : IPipelineFeature<TypeBaseBuilder, EntityContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Model is IRecordContainerBuilder recordContainerBuilder)
        {
            recordContainerBuilder.Record = context.Context.Settings.GenerationSettings.CreateRecord;
        }

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, EntityContext>> ToBuilder()
        => new SetRecordFeatureBuilder();
}
