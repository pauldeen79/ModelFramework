namespace ClassFramework.Pipelines.BuilderInterface.Features;

public class AddExtensionMethodsForNonCollectionPropertiesFeatureBuilder : IBuilderInterfaceFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddExtensionMethodsForNonCollectionPropertiesFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext> Build()
        => new AddExtensionMethodsForNonCollectionPropertiesFeature(_formattableStringParser);
}

public class AddExtensionMethodsForNonCollectionPropertiesFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddExtensionMethodsForNonCollectionPropertiesFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (string.IsNullOrEmpty(context.Context.Settings.NameSettings.SetMethodNameFormatString))
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        foreach (var property in context.Context.GetSourceProperties().Where(x => !x.TypeName.FixTypeName().IsCollectionTypeName()))
        {
            var childContext = new ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext>, Property>(context, property, context.Context.Settings);

            var results = new[]
            {
                new { Name = "TypeName", LazyResult = new Lazy<Result<string>>(() => property.GetBuilderArgumentTypeName(context.Context.Settings.TypeSettings, context.Context.FormatProvider, new ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext>, Property>(context, property, context.Context.Settings), context.Context.MapTypeName(property.TypeName), _formattableStringParser)) },
                new { Name = "Name", LazyResult = new Lazy<Result<string>>(() => _formattableStringParser.Parse(context.Context.Settings.NameSettings.SetMethodNameFormatString, context.Context.FormatProvider, childContext)) },
                new { Name = "BuilderName", LazyResult = new Lazy<Result<string>>(() => _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, childContext)) },
                new { Name = "ArgumentNullCheck", LazyResult = new Lazy<Result<string>>(() => _formattableStringParser.Parse(property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentNullCheckExpression, "{NullCheck.Argument}"), context.Context.FormatProvider, childContext)) },
                new { Name = "BuilderWithExpression", LazyResult = new Lazy<Result<string>>(() => _formattableStringParser.Parse(property.Metadata.GetStringValue(MetadataNames.CustomBuilderWithExpression, "{Name} = {NamePascalCsharpFriendlyName};"), context.Context.FormatProvider, childContext)) },
            }.TakeWhileWithFirstNonMatching(x => x.LazyResult.Value.IsSuccessful()).ToArray();

            var error = Array.Find(results, x => !x.LazyResult.Value.IsSuccessful());
            if (error is not null)
            {
                // Error in formattable string parsing
                return Result.FromExistingResult<IConcreteTypeBuilder>(error.LazyResult.Value);
            }

            var builder = new MethodBuilder()
                .WithName(results.First(x => x.Name == "Name").LazyResult.Value.Value!)
                .WithReturnTypeName($"{results.First(x => x.Name == "BuilderName").LazyResult.Value.Value}{context.Context.SourceModel.GetGenericTypeArgumentsString()}")
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                        .WithTypeName(results.First(x => x.Name == "TypeName").LazyResult.Value.Value!)
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                        .WithDefaultValue(GetMetadata(context, property).GetValue<object?>(MetadataNames.CustomBuilderWithDefaultPropertyValue, () => null))
                );

            if (context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks)
            {
                var nullCheckStatement = results.First(x => x.Name == "ArgumentNullCheck").LazyResult.Value.Value!;
                if (!string.IsNullOrEmpty(nullCheckStatement))
                {
                    builder.AddStringCodeStatements(nullCheckStatement);
                }
            }

            builder.AddStringCodeStatements
            (
                results.First(x => x.Name == "BuilderWithExpression").LazyResult.Value.Value!,
                "return instance;"
            );

            context.Model.AddMethods(builder);
        }

        return Result.Continue<IConcreteTypeBuilder>();
    }

    private static IEnumerable<Metadata> GetMetadata(PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> context, Property property)
        => property.Metadata.WithMappingMetadata(property.TypeName.GetCollectionItemType().WhenNullOrEmpty(property.TypeName), context.Context.Settings.TypeSettings);

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext>> ToBuilder()
        => new AddExtensionMethodsForNonCollectionPropertiesFeatureBuilder(_formattableStringParser);
}
