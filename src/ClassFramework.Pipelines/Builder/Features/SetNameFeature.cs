namespace ClassFramework.Pipelines.Builder.Features;

public class SetNameFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public SetNameFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new SetNameFeature(_formattableStringParser);
}

public class SetNameFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public SetNameFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        var results = new[]
{
            new { Name = "Name", Result = _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, context) },
            new { Name = "Namespace", Result = _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNamespaceFormatString, context.Context.FormatProvider, context) },
        }.TakeWhileWithFirstNonMatching(x => x.Result.IsSuccessful()).ToArray();

        var error = Array.Find(results, x => !x.Result.IsSuccessful());
        if (error is not null)
        {
            // Error in formattable string parsing
            return Result.FromExistingResult<ClassBuilder>(error.Result);
        }

        context.Model.Name = results.First(x => x.Name == "Name").Result.Value!;
        context.Model.Namespace = results.First(x => x.Name == "Namespace").Result.Value!;

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new SetNameFeatureBuilder(_formattableStringParser);
}
