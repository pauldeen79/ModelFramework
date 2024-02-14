namespace ClassFramework.Pipelines.Builder.Features;

public class AddFluentMethodsForCollectionPropertiesFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddFluentMethodsForCollectionPropertiesFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, BuilderContext> Build()
        => new AddFluentMethodsForCollectionPropertiesFeature(_formattableStringParser);
}

public class AddFluentMethodsForCollectionPropertiesFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddFluentMethodsForCollectionPropertiesFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (string.IsNullOrEmpty(context.Context.Settings.NameSettings.AddMethodNameFormatString))
        {
            return Result.Continue<IConcreteTypeBuilder>();
        }

        foreach (var property in context.Context.GetSourceProperties().Where(x => context.Context.IsValidForFluentMethod(x) && x.TypeName.FixTypeName().IsCollectionTypeName()))
        {
            var childContext = CreateParentChildContext(context, property);

            var typeNameResult = property.GetBuilderArgumentTypeName(context, _formattableStringParser);

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

            var returnType = context.Context.IsBuilderForAbstractEntity
                ? $"TBuilder{context.Context.SourceModel.GetGenericTypeArgumentsString()}"
                : $"{namespaceResult.Value}{context.Context.SourceModel.GetGenericTypeArgumentsString()}";

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

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderContext>> ToBuilder()
        => new AddFluentMethodsForCollectionPropertiesFeatureBuilder(_formattableStringParser);

    private IEnumerable<Result<string>> GetCodeStatementsForEnumerableOverload(PipelineContext<IConcreteTypeBuilder, BuilderContext> context, Property property)
    {
        if (context.Context.Settings.TypeSettings.NewCollectionTypeName == typeof(IEnumerable<>).WithoutGenerics())
        {
            // When using IEnumerable<>, do not call ToArray because we want lazy evaluation
            foreach (var statement in GetCodeStatementsForArrayOverload(context, property))
            {
                yield return statement;
            }

            yield break;
        }

        // When not using IEnumerable<>, we can simply force ToArray because it's stored in a generic list or collection of some sort anyway.
        // (in other words, materialization is always performed)
        yield return Result.Success(context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks
            ? $"return Add{property.Name}({property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()}?.ToArray() ?? throw new {typeof(ArgumentNullException).FullName}(nameof({property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()})));"
            : $"return Add{property.Name}({property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()}.ToArray());");
    }

    private IEnumerable<Result<string>> GetCodeStatementsForArrayOverload(PipelineContext<IConcreteTypeBuilder, BuilderContext> context, Property property)
    {
        if (context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks)
        {
            var argumentNullCheckResult = _formattableStringParser.Parse
            (
                property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentNullCheckExpression, "{NullCheck.Argument}"),
                context.Context.FormatProvider,
                new ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderContext>, Property>(context, property, context.Context.Settings)
            );
            yield return argumentNullCheckResult;
            
            if (context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared)
            {
                var constructorInitializerResult = property.GetBuilderConstructorInitializer(context, _formattableStringParser); // note that we're not checking the status of this result, because it is using the same expression that we heve already checked before (typeNameResult, see above in this class)
                yield return Result.Success($"if ({property.GetBuilderMemberName(context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks, context.Context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Context.Settings.EntitySettings.GenerationSettings.AddBackingFields, context.Context.FormatProvider.ToCultureInfo())} is null) {property.GetBuilderMemberName(context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks, context.Context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Context.Settings.EntitySettings.GenerationSettings.AddBackingFields, context.Context.FormatProvider.ToCultureInfo())} = {constructorInitializerResult.GetValueOrThrow()};"); // note that we use GetValueOrThrow here, because we have already checked this expression in the typeNameResult (see above in this class)
            }
        }

        var builderAddExpressionResult = _formattableStringParser.Parse
        (
            property.Metadata
                .WithMappingMetadata(property.TypeName.GetCollectionItemType().WhenNullOrEmpty(property.TypeName), context.Context.Settings.TypeSettings)
                .GetStringValue(MetadataNames.CustomBuilderAddExpression, context.Context.Settings.TypeSettings.CollectionCopyStatementFormatString),
            context.Context.FormatProvider,
            new ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderContext>, Property>(context, property, context.Context.Settings)
        );

        yield return builderAddExpressionResult;

        yield return Result.Success($"return {GetReturnValue(context.Context)};");
    }

    private static string GetReturnValue(BuilderContext context)
    {
        if (context.IsBuilderForAbstractEntity)
        {
            return "(TBuilder)this";
        }

        return "this";
    }

    private static ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderContext>, Property> CreateParentChildContext(PipelineContext<IConcreteTypeBuilder, BuilderContext> context, Property property)
        => new(context, property, context.Context.Settings);
}
