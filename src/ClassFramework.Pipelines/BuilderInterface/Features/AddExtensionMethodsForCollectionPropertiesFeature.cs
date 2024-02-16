namespace ClassFramework.Pipelines.BuilderInterface.Features;

public class AddExtensionMethodsForCollectionPropertiesFeatureBuilder : IBuilderInterfaceFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddExtensionMethodsForCollectionPropertiesFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext> Build()
        => new AddExtensionMethodsForCollectionPropertiesFeature(_formattableStringParser);
}

public class AddExtensionMethodsForCollectionPropertiesFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddExtensionMethodsForCollectionPropertiesFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (string.IsNullOrEmpty(context.Context.Settings.NameSettings.AddMethodNameFormatString))
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        foreach (var property in context.Context.GetSourceProperties().Where(x => x.TypeName.FixTypeName().IsCollectionTypeName()))
        {
            var childContext = CreateParentChildContext(context, property);

            var typeNameResult = property.GetBuilderArgumentTypeName(context.Context.Settings.TypeSettings, context.Context.FormatProvider, CreateParentChildContext(context, property), context.Context.MapTypeName(property.TypeName), _formattableStringParser);

            if (!typeNameResult.IsSuccessful())
            {
                return Result.FromExistingResult<IConcreteTypeBuilder>(typeNameResult);
            }

            var namespaceResult = _formattableStringParser.Parse
            (
                context.Context.Settings.NameSettings.BuilderNameFormatString,
                context.Context.FormatProvider,
                childContext
            );

            if (!namespaceResult.IsSuccessful())
            {
                return Result.FromExistingResult<IConcreteTypeBuilder>(namespaceResult);
            }

            var returnType = $"{namespaceResult.Value}{context.Context.SourceModel.GetGenericTypeArgumentsString()}";

            var addMethodNameFormatStringResult = _formattableStringParser.Parse(context.Context.Settings.NameSettings.AddMethodNameFormatString, context.Context.FormatProvider, childContext);
            if (!addMethodNameFormatStringResult.IsSuccessful())
            {
                return Result.FromExistingResult<IConcreteTypeBuilder>(addMethodNameFormatStringResult);
            }

            context.Model.AddMethods(new MethodBuilder()
                .WithName(addMethodNameFormatStringResult.Value!)
                .WithReturnTypeName(returnType)
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                        .WithTypeName(typeNameResult.Value!.FixCollectionTypeName(typeof(IEnumerable<>).WithoutGenerics()))
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                )
                //.AddStringCodeStatements(enumerableOverloadResults.Select(x => x.Value!))
            );

            context.Model.AddMethods(new MethodBuilder()
                .WithName(addMethodNameFormatStringResult.Value!)
                .WithReturnTypeName(returnType)
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                        .WithTypeName(typeNameResult.Value!.FixTypeName().ConvertTypeNameToArray())
                        .WithIsParamArray()
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                )
                //.AddStringCodeStatements(arrayOverloadResults.Select(x => x.Value!))
            );
        }

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext>> ToBuilder()
        => new AddExtensionMethodsForCollectionPropertiesFeatureBuilder(_formattableStringParser);

    private static ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext>, Property> CreateParentChildContext(PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> context, Property property)
        => new(context, property, context.Context.Settings);
}
