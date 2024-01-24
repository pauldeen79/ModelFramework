namespace ClassFramework.Pipelines.Builder.Features;

public class AddFluentMethodsForNonCollectionPropertiesFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddFluentMethodsForNonCollectionPropertiesFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, BuilderContext> Build()
        => new AddFluentMethodsForNonCollectionPropertiesFeature(_formattableStringParser);
}

public class AddFluentMethodsForNonCollectionPropertiesFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddFluentMethodsForNonCollectionPropertiesFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (string.IsNullOrEmpty(context.Context.Settings.NameSettings.SetMethodNameFormatString))
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        foreach (var property in context.Context.SourceModel.GetPropertiesFromClassAndBaseClass(context.Context.Settings).Where(x => !x.TypeName.FixTypeName().IsCollectionTypeName()))
        {
            var childContext = new ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderContext>, Property>(context, property, context.Context.Settings);

            var results = new[]
            {
                new { Name = "TypeName", LazyResult = new Lazy<Result<string>>(() => property.GetBuilderArgumentType(context, _formattableStringParser)) },
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
                .WithReturnTypeName(context.Context.IsBuilderForAbstractEntity
                      ? $"TBuilder{context.Context.SourceModel.GetGenericTypeArgumentsString()}"
                      : $"{results.First(x => x.Name == "BuilderName").LazyResult.Value.Value}{context.Context.SourceModel.GetGenericTypeArgumentsString()}")
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                        .WithTypeName(results.First(x => x.Name == "TypeName").LazyResult.Value.Value!)
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                        .WithDefaultValue(property.Metadata.GetValue<object?>(MetadataNames.CustomBuilderWithDefaultPropertyValue, () => null))
                );

            if (context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks)
            {
                builder.AddStringCodeStatements(results.First(x => x.Name == "ArgumentNullCheck").LazyResult.Value.Value!);
            }

            builder.AddStringCodeStatements
            (
                results.First(x => x.Name == "BuilderWithExpression").LazyResult.Value.Value!,
                $"return {GetReturnValue(context.Context)};"
            );

            context.Model.AddMethods(builder);
        }

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderContext>> ToBuilder()
        => new AddFluentMethodsForNonCollectionPropertiesFeatureBuilder(_formattableStringParser);

    private static string GetReturnValue(BuilderContext context)
    {
        if (context.IsBuilderForAbstractEntity)
        {
            return "(TBuilder)this";
        }

        return "this";
    }
}
