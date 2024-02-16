namespace ClassFramework.Pipelines.BuilderInterface.Features;

public class AddMetadataFeatureBuilder : IBuilderInterfaceFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext> Build()
        => new AddMetadataFeature();
}

public class AddMetadataFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.AddMetadata(context.Context.SourceModel.Metadata.Select(x => x.ToBuilder()));

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext>> ToBuilder()
        => new AddMetadataFeatureBuilder();
}
