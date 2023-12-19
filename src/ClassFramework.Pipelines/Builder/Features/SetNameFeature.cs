namespace ClassFramework.Pipelines.Builder.Features;

public class SetNameFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public SetNameFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, BuilderContext> Build()
        => new SetNameFeature(_formattableStringParser);
}

public class SetNameFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public SetNameFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        var results = new[]
        {
            new { Name = "Name", LazyResult = new Lazy<Result<string>>(() => _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, context)) },
            new { Name = "Namespace", LazyResult = new Lazy<Result<string>>(() => context.Context.SourceModel.Metadata.WithMappingMetadata(context.Context.SourceModel.GetFullName().GetCollectionItemType().WhenNullOrEmpty(context.Context.SourceModel.GetFullName()), context.Context.Settings.TypeSettings).GetStringResult(MetadataNames.CustomBuilderNamespace, () => _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNamespaceFormatString, context.Context.FormatProvider, context))) },
        }.TakeWhileWithFirstNonMatching(x => x.LazyResult.Value.IsSuccessful()).ToArray();

        var error = Array.Find(results, x => !x.LazyResult.Value.IsSuccessful());
        if (error is not null)
        {
            // Error in formattable string parsing
            return Result.FromExistingResult<IConcreteTypeBuilder>(error.LazyResult.Value);
        }

        context.Model
            .WithName(results.First(x => x.Name == "Name").LazyResult.Value.Value!)
            .WithNamespace(results.First(x => x.Name == "Namespace").LazyResult.Value.Value!);

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderContext>> ToBuilder()
        => new SetNameFeatureBuilder(_formattableStringParser);
}
