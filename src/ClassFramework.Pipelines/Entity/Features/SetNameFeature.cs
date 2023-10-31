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

        var results = new[]
        {
            new { Name = "Name", LazyResult = new Lazy<Result<string>>(() => _formattableStringParser.Parse(context.Context.Settings.NameSettings.EntityNameFormatString, context.Context.FormatProvider, context)) },
            new { Name = "Namespace", LazyResult = new Lazy<Result<string>>(() => _formattableStringParser.Parse(context.Context.Settings.NameSettings.EntityNamespaceFormatString, context.Context.FormatProvider, context)) },
        }.TakeWhileWithFirstNonMatching(x => x.LazyResult.Value.IsSuccessful()).ToArray();

        var error = Array.Find(results, x => !x.LazyResult.Value.IsSuccessful());
        if (error is not null)
        {
            // Error in formattable string parsing
            return Result.FromExistingResult<ClassBuilder>(error.LazyResult.Value);
        }

        context.Model.Name = results.First(x => x.Name == "Name").LazyResult.Value.Value!;
        context.Model.Namespace = results.First(x => x.Name == "Namespace").LazyResult.Value.Value!;

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new SetNameFeatureBuilder(_formattableStringParser);
}
