namespace ClassFramework.Pipelines.Builder.Features;

public class AddMetadataFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AddMetadataFeature();
}

public class AddMetadataFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.AddMetadata(context.Context.SourceModel.Metadata.Select(x => new MetadataBuilder(x)));

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddMetadataFeatureBuilder();
}
