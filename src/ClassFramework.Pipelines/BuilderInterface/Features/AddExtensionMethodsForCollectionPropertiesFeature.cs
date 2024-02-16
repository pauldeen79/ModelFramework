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
                context.Context.Settings.NameSettings.BuilderNamespaceFormatString,
                context.Context.FormatProvider,
                childContext
            );

            if (!namespaceResult.IsSuccessful())
            {
                return Result.FromExistingResult<IConcreteTypeBuilder>(namespaceResult);
            }

            var nameResult = _formattableStringParser.Parse
            (
                context.Context.Settings.NameSettings.BuilderNameFormatString,
                context.Context.FormatProvider,
                childContext
            );

            if (!nameResult.IsSuccessful())
            {
                return Result.FromExistingResult<IConcreteTypeBuilder>(nameResult);
            }

            var returnType = string.IsNullOrEmpty(namespaceResult.Value)
                ? $"{nameResult.Value}{context.Context.SourceModel.GetGenericTypeArgumentsString()}"
                : $"{namespaceResult.Value}.{nameResult.Value}{context.Context.SourceModel.GetGenericTypeArgumentsString()}";

            var enumerableOverloadResults = GetCodeStatementsForEnumerableOverload(context, property)
                .TakeWhileWithFirstNonMatching(x => x.IsSuccessful())
                .ToArray();

            var errorResult = Array.Find(enumerableOverloadResults, x => !x.IsSuccessful());
            if (errorResult is not null)
            {
                return Result.FromExistingResult<IConcreteTypeBuilder>(errorResult);
            }

            var arrayOverloadResults = GetCodeStatementsForArrayOverload(context, property)
                .TakeWhileWithFirstNonMatching(x => x.IsSuccessful())
                .ToArray();

            errorResult = Array.Find(arrayOverloadResults, x => !x.IsSuccessful());
            if (errorResult is not null)
            {
                return Result.FromExistingResult<IConcreteTypeBuilder>(errorResult);
            }

            var addMethodNameFormatStringResult = _formattableStringParser.Parse(context.Context.Settings.NameSettings.AddMethodNameFormatString, context.Context.FormatProvider, childContext);
            if (!addMethodNameFormatStringResult.IsSuccessful())
            {
                return Result.FromExistingResult<IConcreteTypeBuilder>(addMethodNameFormatStringResult);
            }

            context.Model.AddMethods(new MethodBuilder()
                .WithName(addMethodNameFormatStringResult.Value!)
                .WithReturnTypeName(returnType)
                .WithStatic()
                .WithExtensionMethod()
                .AddGenericTypeArguments("T")
                .AddGenericTypeArgumentConstraints($"where T : {returnType}")
                .AddParameter("instance", returnType)
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                        .WithTypeName(typeNameResult.Value!.FixCollectionTypeName(typeof(IEnumerable<>).WithoutGenerics()))
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                )
                .AddStringCodeStatements(enumerableOverloadResults.Select(x => x.Value!))
            );

            context.Model.AddMethods(new MethodBuilder()
                .WithName(addMethodNameFormatStringResult.Value!)
                .WithReturnTypeName(returnType)
                .WithStatic()
                .WithExtensionMethod()
                .AddGenericTypeArguments("T")
                .AddGenericTypeArgumentConstraints($"where T : {returnType}")
                .AddParameter("instance", returnType)
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                        .WithTypeName(typeNameResult.Value!.FixTypeName().ConvertTypeNameToArray())
                        .WithIsParamArray()
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                )
                .AddStringCodeStatements(arrayOverloadResults.Select(x => x.Value!))
            );
        }

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderInterfaceContext>> ToBuilder()
        => new AddExtensionMethodsForCollectionPropertiesFeatureBuilder(_formattableStringParser);

    private IEnumerable<Result<string>> GetCodeStatementsForEnumerableOverload(PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> context, Property property)
    {
        yield return Result.Success(context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks
            ? $"return instance.Add{property.Name}<T>({property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()}?.ToArray() ?? throw new {typeof(ArgumentNullException).FullName}(nameof({property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()})));"
            : $"return instance.Add{property.Name}<T>({property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()}.ToArray());");
    }

    private IEnumerable<Result<string>> GetCodeStatementsForArrayOverload(PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> context, Property property)
    {
        if (context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks)
        {
            var argumentNullCheckResult = _formattableStringParser.Parse
            (
                property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentNullCheckExpression, "{NullCheck.Argument}"),
                context.Context.FormatProvider,
                new ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext>, Property>(context, property, context.Context.Settings)
            );
            yield return argumentNullCheckResult;

            if (context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared)
            {
                var constructorInitializerResult = property.GetBuilderConstructorInitializer(context.Context.Settings.TypeSettings, context.Context.FormatProvider, CreateParentChildContext(context, property), context.Context.MapTypeName(property.TypeName), context.Context.Settings.TypeSettings.NewCollectionTypeName, _formattableStringParser); // note that we're not checking the status of this result, because it is using the same expression that we heve already checked before (typeNameResult, see above in this class)
                yield return Result.Success($"if (instance.{property.GetBuilderMemberName(context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks, context.Context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Context.Settings.EntitySettings.GenerationSettings.AddBackingFields, context.Context.FormatProvider.ToCultureInfo())} is null) {property.GetBuilderMemberName(context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks, context.Context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Context.Settings.EntitySettings.GenerationSettings.AddBackingFields, context.Context.FormatProvider.ToCultureInfo())} = {constructorInitializerResult.GetValueOrThrow()};"); // note that we use GetValueOrThrow here, because we have already checked this expression in the typeNameResult (see above in this class)
            }
        }

        var builderAddExpressionResult = _formattableStringParser.Parse
        (
            property.Metadata
                .WithMappingMetadata(property.TypeName.GetCollectionItemType().WhenNullOrEmpty(property.TypeName), context.Context.Settings.TypeSettings)
                .GetStringValue(MetadataNames.CustomBuilderAddExpression, context.Context.Settings.TypeSettings.CollectionCopyStatementFormatString),
            context.Context.FormatProvider,
            new ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext>, Property>(context, property, context.Context.Settings)
        );

        yield return builderAddExpressionResult;

        yield return Result.Success("return instance;");
    }

    private static ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext>, Property> CreateParentChildContext(PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> context, Property property)
        => new(context, property, context.Context.Settings);
}
