namespace ClassFramework.Pipelines.OverrideEntity.Features;

public class SetNameFeatureBuilder : IOverrideEntityFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public SetNameFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, OverrideEntityContext> Build()
        => new SetNameFeature(_formattableStringParser);
}

public class SetNameFeature : IPipelineFeature<IConcreteTypeBuilder, OverrideEntityContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public SetNameFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, OverrideEntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        var resultSetBuilder = new NamedResultSetBuilder<string>();
        resultSetBuilder.Add("Name", () => _formattableStringParser.Parse(context.Context.Settings.EntityNameFormatString, context.Context.FormatProvider, context));
        resultSetBuilder.Add("Namespace", () => context.Context.SourceModel.Metadata.WithMappingMetadata(context.Context.SourceModel.GetFullName().GetCollectionItemType().WhenNullOrEmpty(context.Context.SourceModel.GetFullName), context.Context.Settings).GetStringResult(MetadataNames.CustomEntityNamespace, () => _formattableStringParser.Parse(context.Context.Settings.EntityNamespaceFormatString, context.Context.FormatProvider, context)));
        var results = resultSetBuilder.Build();

        var error = Array.Find(results, x => !x.Result.IsSuccessful());
        if (error is not null)
        {
            // Error in formattable string parsing
            return Result.FromExistingResult<IConcreteTypeBuilder>(error.Result);
        }

        context.Model
            .WithName(results.First(x => x.Name == "Name").Result.Value!)
            .WithNamespace(context.Context.MapNamespace(results.First(x => x.Name == "Namespace").Result.Value!));

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, OverrideEntityContext>> ToBuilder()
        => new SetNameFeatureBuilder(_formattableStringParser);
}
