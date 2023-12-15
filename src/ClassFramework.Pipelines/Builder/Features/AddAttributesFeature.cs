﻿namespace ClassFramework.Pipelines.Builder.Features;

public class AddAttributesFeatureBuilder : IBuilderFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, BuilderContext> Build()
        => new AddAttributesFeature();
}

public class AddAttributesFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.EntitySettings.CopySettings.CopyAttributes)
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        context.Model.Attributes.AddRange(context.Context.SourceModel.Attributes
            .Where(x => context.Context.Settings.EntitySettings.CopySettings.CopyAttributePredicate?.Invoke(x) ?? true)
            .Select(x => new AttributeBuilder(context.Context.MapAttribute(x))));

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderContext>> ToBuilder()
        => new AddAttributesFeatureBuilder();
}
