namespace ClassFramework.Pipelines.Builder.Features;

public class AddMetadataFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, BuilderContext> Build()
        => new AddMetadataFeature();
}

public class AddMetadataFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.Metadata.AddRange(context.Context.SourceModel.Metadata.Select(x => new MetadataBuilder(x)));

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderContext>> ToBuilder()
        => new AddMetadataFeatureBuilder();
}
