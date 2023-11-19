namespace ClassFramework.Pipelines.Entity.Features;

public class AddMetadataFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, EntityContext> Build()
        => new AddMetadataFeature();
}

public class AddMetadataFeature : IPipelineFeature<ClassBuilder, EntityContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.AddMetadata(context.Context.SourceModel.Metadata.Select(x => new MetadataBuilder(x)));

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new AddMetadataFeatureBuilder();
}
