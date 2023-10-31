namespace ClassFramework.Pipelines.Entity.Features;

public class SetNameFeatureBuilder : IEntityFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public SetNameFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, EntityContext> Build()
        => new SetNameFeature(_formattableStringParser);
}

public class SetNameFeature : IPipelineFeature<ClassBuilder, EntityContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public SetNameFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.Name = _formattableStringParser.Parse(context.Context.Settings.NameSettings.EntityNameFormatString, context.Context.FormatProvider, context).GetValueOrThrow();
        context.Model.Namespace = _formattableStringParser.Parse(context.Context.Settings.NameSettings.EntityNamespaceFormatString, context.Context.FormatProvider, context).GetValueOrThrow();

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new SetNameFeatureBuilder(_formattableStringParser);
}
