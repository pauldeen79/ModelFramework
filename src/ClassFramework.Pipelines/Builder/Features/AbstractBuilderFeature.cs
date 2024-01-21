﻿namespace ClassFramework.Pipelines.Builder.Features;

public class AbstractBuilderFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AbstractBuilderFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, BuilderContext> Build()
        => new AbstractBuilderFeature(_formattableStringParser);
}

public class AbstractBuilderFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AbstractBuilderFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.IsBuilderForAbstractEntity /*&& context.Context.IsAbstractBuilder*/)
        {
            var nameResult = _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, context);
            if (!nameResult.IsSuccessful())
            {
                return Result.FromExistingResult<IConcreteTypeBuilder>(nameResult);
            }

            var classBuilder = context.Model as ClassBuilder;
            if (classBuilder is null)
            {
                return Result.Invalid<IConcreteTypeBuilder>($"You can only create abstract classes. The type of model ({context.Model.GetType().FullName}) is not a ClassBuilder");
            }

            classBuilder
                .AddGenericTypeArguments("TBuilder", "TEntity")
                .AddGenericTypeArgumentConstraints($"where TEntity : {context.Context.SourceModel.GetFullName()}")
                .AddGenericTypeArgumentConstraints($"where TBuilder : {nameResult.Value}<TBuilder, TEntity>")
                .WithAbstract();
        }

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderContext>> ToBuilder()
        => new AbstractBuilderFeatureBuilder(_formattableStringParser);
}
