namespace ClassFramework.Pipelines.Builder.Features;

public class AddFluentMethodsForCollectionPropertiesFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddFluentMethodsForCollectionPropertiesFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AddFluentMethodsForCollectionPropertiesFeature(_formattableStringParser);
}

public class AddFluentMethodsForCollectionPropertiesFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddFluentMethodsForCollectionPropertiesFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (string.IsNullOrEmpty(context.Context.Settings.NameSettings.AddMethodNameFormatString))
        {
            return Result.Continue<ClassBuilder>();
        }

        foreach (var property in context.Context.Model.GetPropertiesFromClassAndBaseClass(context.Context.Settings).Where(x => x.TypeName.FixTypeName().IsCollectionTypeName()))
        {
            var childContext = new ParentChildContext<BuilderContext, ClassProperty>(context, property, context.Context.Settings);

            var typeNameResult = _formattableStringParser.Parse
            (
                property.Metadata
                    .WithMappingMetadata(property.TypeName.GetCollectionItemType().WhenNullOrEmpty(property.TypeName), context.Context.Settings.TypeSettings)
                    .GetStringValue(MetadataNames.CustomBuilderArgumentType, () => context.Context.MapTypeName(property.TypeName)),
                context.Context.FormatProvider,
                childContext
            );

            if (!typeNameResult.IsSuccessful())
            {
                return Result.FromExistingResult<ClassBuilder>(typeNameResult);
            }

            var namespaceResult = _formattableStringParser.Parse
            (
                context.Context.Settings.NameSettings.BuilderNameFormatString,
                context.Context.FormatProvider,
                childContext
            );

            if (!namespaceResult.IsSuccessful())
            {
                return Result.FromExistingResult<ClassBuilder>(namespaceResult);
            }

            var returnType = context.Context.IsBuilderForAbstractEntity
                ? "TBuilder" + context.Context.Model.GetGenericTypeArgumentsString()
                : namespaceResult.Value! + context.Context.Model.GetGenericTypeArgumentsString();

            var enumerableOverloadResults = GetCodeStatementsForEnumerableOverload(context, property)
                .TakeWhileWithFirstNonMatching(x => x.IsSuccessful())
                .ToArray();

            var errorResult = Array.Find(enumerableOverloadResults, x => !x.IsSuccessful());
            if (errorResult is not null)
            {
                return Result.FromExistingResult<ClassBuilder>(errorResult);
            }

            var arrayOverloadResults = GetCodeStatementsForArrayOverload(context, property)
                .TakeWhileWithFirstNonMatching(x => x.IsSuccessful())
                .ToArray();

            errorResult = Array.Find(arrayOverloadResults, x => !x.IsSuccessful());
            if (errorResult is not null)
            {
                return Result.FromExistingResult<ClassBuilder>(errorResult);
            }

            var addMethodNameFormatStringResult = _formattableStringParser.Parse(context.Context.Settings.NameSettings.AddMethodNameFormatString, context.Context.FormatProvider, childContext);
            if (!addMethodNameFormatStringResult.IsSuccessful())
            {
                return Result.FromExistingResult<ClassBuilder>(addMethodNameFormatStringResult);
            }

            context.Model.AddMethods(new ClassMethodBuilder()
                .WithName(addMethodNameFormatStringResult.Value!)
                .WithTypeName(returnType)
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

            context.Model.AddMethods(new ClassMethodBuilder()
                .WithName(addMethodNameFormatStringResult.Value!)
                .WithTypeName(returnType)
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                        .WithTypeName(typeNameResult.Value!.FixTypeName().ConvertTypeNameToArray())
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                )
                .AddStringCodeStatements(arrayOverloadResults.Select(x => x.Value!))
            );
        }

        return Result.Continue<ClassBuilder>();
    }

    private IEnumerable<Result<string>> GetCodeStatementsForEnumerableOverload(PipelineContext<ClassBuilder, BuilderContext> context, ClassProperty property)
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
        yield return Result.Success(context.Context.Settings.GenerationSettings.AddNullChecks
            ? $"return Add{property.Name}({property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()}?.ToArray() ?? throw new {typeof(ArgumentNullException).FullName}(nameof({property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()})));"
            : $"return Add{property.Name}({property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()}.ToArray());");
    }

    private IEnumerable<Result<string>> GetCodeStatementsForArrayOverload(PipelineContext<ClassBuilder, BuilderContext> context, ClassProperty property)
    {
        if (context.Context.Settings.GenerationSettings.AddNullChecks)
        {
            var argumentNullCheckResult = _formattableStringParser.Parse
            (
                property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentNullCheckExpression, "{NullCheck.Argument}"),
                context.Context.FormatProvider,
                new ParentChildContext<BuilderContext, ClassProperty>(context, property, context.Context.Settings)
            );
            yield return argumentNullCheckResult;
            
            if (context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared)
            {
                var constructorInitializerResult = property.GetBuilderClassConstructorInitializer(context, _formattableStringParser, property.TypeName); // note that we're not checking the status of this result, because it is using the same expression that we heve already checked before (typeNameResult, see above in this class)
                yield return Result.Success($"if ({property.GetInitializationName(context.Context.Settings.GenerationSettings.AddNullChecks, context.Context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Context.FormatProvider.ToCultureInfo())} is null) {property.GetInitializationName(context.Context.Settings.GenerationSettings.AddNullChecks, context.Context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Context.FormatProvider.ToCultureInfo())} = {constructorInitializerResult.GetValueOrThrow()};"); // note that we use GetValueOrThrow here, because we have already checked this expression in the typeNameResult (see above in this class)
            }
        }

        var builderAddExpressionResult = _formattableStringParser.Parse
        (
            property.Metadata
                .WithMappingMetadata(property.TypeName.GetCollectionItemType().WhenNullOrEmpty(property.TypeName), context.Context.Settings.TypeSettings)
                .GetStringValue(MetadataNames.CustomBuilderAddExpression, () => CreateBuilderCollectionPropertyAddExpression(property, context.Context)),
            context.Context.FormatProvider,
            new ParentChildContext<BuilderContext, ClassProperty>(context, property, context.Context.Settings)
        );

        yield return builderAddExpressionResult;

        yield return Result.Success($"return {GetReturnValue(context.Context)};");
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddFluentMethodsForCollectionPropertiesFeatureBuilder(_formattableStringParser);

    private static string CreateBuilderCollectionPropertyAddExpression(ClassProperty property, BuilderContext context)
    {
        if (context.Settings.TypeSettings.NewCollectionTypeName == typeof(IEnumerable<>).WithoutGenerics())
        {
            return $"{property.Name} = {property.Name}.Concat({property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()});";
        }

        return $"{property.Name}.AddRange({property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()});";
    }

    private static string GetReturnValue(BuilderContext context)
    {
        if (context.IsBuilderForAbstractEntity)
        {
            return "(TBuilder)this";
        }

        return "this";
    }
}
