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

        if (string.IsNullOrEmpty(context.Context.Settings.SetMethodNameFormatString))
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        foreach (var property in context.Context.GetSourceProperties().Where(x => !x.TypeName.FixTypeName().IsCollectionTypeName()))
        {
            var childContext = new ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext>, Property>(context, property, context.Context.Settings);

            var resultSetBuilder = new NamedResultSetBuilder<string>();
            resultSetBuilder.Add("TypeName", () => property.GetBuilderArgumentTypeName(context.Context.Settings, context.Context.FormatProvider, new ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext>, Property>(context, property, context.Context.Settings), context.Context.MapTypeName(property.TypeName), _formattableStringParser));
            resultSetBuilder.Add("Namespace", () => _formattableStringParser.Parse(context.Context.Settings.BuilderNamespaceFormatString, context.Context.FormatProvider, childContext));
            resultSetBuilder.Add("MethodName", () => _formattableStringParser.Parse(context.Context.Settings.SetMethodNameFormatString, context.Context.FormatProvider, childContext));
            resultSetBuilder.Add("BuilderName", () => _formattableStringParser.Parse(context.Context.Settings.BuilderNameFormatString, context.Context.FormatProvider, childContext));
            resultSetBuilder.Add("ArgumentNullCheck", () => _formattableStringParser.Parse(property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentNullCheckExpression, "{NullCheck.Argument}"), context.Context.FormatProvider, childContext));
            resultSetBuilder.Add("BuilderWithExpression", () => _formattableStringParser.Parse(property.Metadata.GetStringValue(MetadataNames.CustomBuilderWithExpression, "instance.{Name} = {NamePascalCsharpFriendlyName};"), context.Context.FormatProvider, childContext));
            var results = resultSetBuilder.Build();

            var error = Array.Find(results, x => !x.Result.IsSuccessful());
            if (error is not null)
            {
                // Error in formattable string parsing
                return Result.FromExistingResult<IConcreteTypeBuilder>(error.Result);
            }

            var returnType = string.IsNullOrEmpty(results.First(x => x.Name == "Namespace").Result.Value)
                ? $"{results.First(x => x.Name == "BuilderName").Result.Value}{context.Context.SourceModel.GetGenericTypeArgumentsString()}"
                : $"{results.First(x => x.Name == "Namespace").Result.Value}.{results.First(x => x.Name == "BuilderName").Result.Value}{context.Context.SourceModel.GetGenericTypeArgumentsString()}";

            var builder = new MethodBuilder()
                .WithName(results.First(x => x.Name == "MethodName").Result.Value!)
                .WithReturnTypeName($"{returnType}{context.Context.SourceModel.GetGenericTypeArgumentsString()}")
                .WithStatic()
                .WithExtensionMethod()
                .AddGenericTypeArguments("T")
                .AddGenericTypeArgumentConstraints($"where T : {returnType}")
                .AddParameter("instance", returnType)
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                        .WithTypeName(results.First(x => x.Name == "TypeName").Result.Value!)
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                        .WithDefaultValue(GetMetadata(context, property).GetValue<object?>(MetadataNames.CustomBuilderWithDefaultPropertyValue, () => null))
                );

            if (context.Context.Settings.AddNullChecks)
            {
                var nullCheckStatement = results.First(x => x.Name == "ArgumentNullCheck").Result.Value!;
                if (!string.IsNullOrEmpty(nullCheckStatement))
                {
                    builder.AddStringCodeStatements(nullCheckStatement);
                }
            }

            builder.AddStringCodeStatements
            (
                results.First(x => x.Name == "BuilderWithExpression").Result.Value!,
                "return instance;"
            );

            context.Model.AddMethods(builder);
        }

        return Result.Continue<IConcreteTypeBuilder>();
    }

    private static IEnumerable<Metadata> GetMetadata(PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> context, Property property)
        => property.Metadata.WithMappingMetadata(property.TypeName.GetCollectionItemType().WhenNullOrEmpty(property.TypeName), context.Context.Settings);

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext>> ToBuilder()
        => new AddExtensionMethodsForNonCollectionPropertiesFeatureBuilder(_formattableStringParser);
}
