﻿namespace ClassFramework.Pipelines.BuilderExtension.Features;

public class AddMetadataFeatureBuilder : IBuilderInterfaceFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, BuilderExtensionContext> Build()
        => new AddMetadataFeature();
}

public class AddMetadataFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderExtensionContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderExtensionContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.AddMetadata(context.Context.SourceModel.Metadata.Select(x => x.ToBuilder()));

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderExtensionContext>> ToBuilder()
        => new AddMetadataFeatureBuilder();
}
