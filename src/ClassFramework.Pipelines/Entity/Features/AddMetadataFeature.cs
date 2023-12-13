namespace ClassFramework.Pipelines.Entity.Features;

public class AddMetadataFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, EntityContext> Build()
        => new AddMetadataFeature();
}

public class AddMetadataFeature : IPipelineFeature<TypeBaseBuilder, EntityContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.Metadata.AddRange(context.Context.SourceModel.Metadata.Select(x => new MetadataBuilder(x)));

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, EntityContext>> ToBuilder()
        => new AddMetadataFeatureBuilder();
}
