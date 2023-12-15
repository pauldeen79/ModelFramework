﻿namespace ClassFramework.Pipelines.Builder.Features;

public class GenericsFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, BuilderContext> Build()
        => new GenericsFeature();
}

public class GenericsFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.GenericTypeArguments.AddRange(context.Context.SourceModel.GenericTypeArguments);
        context.Model.GenericTypeArgumentConstraints.AddRange(context.Context.SourceModel.GenericTypeArgumentConstraints);

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderContext>> ToBuilder()
        => new GenericsFeatureBuilder();
}
