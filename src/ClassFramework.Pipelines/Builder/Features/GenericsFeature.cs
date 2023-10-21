﻿namespace ClassFramework.Pipelines.Builder.Features;

public class GenericsFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new GenericsFeature();
}

public class GenericsFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    public Result<BuilderContext> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model
            .AddGenericTypeArguments(context.Context.SourceModel.GenericTypeArguments)
            .AddGenericTypeArgumentConstraints(context.Context.SourceModel.GenericTypeArgumentConstraints);

        return Result.Continue<BuilderContext>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new GenericsFeatureBuilder();
}
