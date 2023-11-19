namespace ClassFramework.Pipelines.Entity.Features;

public class SetRecordFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, EntityContext> Build()
        => new SetRecordFeature();
}

public class SetRecordFeature : IPipelineFeature<ClassBuilder, EntityContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.Record = context.Context.Settings.GenerationSettings.CreateRecord;

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new SetRecordFeatureBuilder();
}
