﻿namespace ClassFramework.Pipelines.BuilderInterface.Features;

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

            var resultSetBuilder = new NamedResultSetBuilder<string>();
            resultSetBuilder.Add("TypeName", () => property.GetBuilderArgumentTypeName(context.Context.Settings.TypeSettings, context.Context.FormatProvider, CreateParentChildContext(context, property), context.Context.MapTypeName(property.TypeName), _formattableStringParser));
            resultSetBuilder.Add("Namespace", () => _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNamespaceFormatString, context.Context.FormatProvider, childContext));
            resultSetBuilder.Add("BuilderName", () => _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, childContext));
            resultSetBuilder.Add("AddMethodName", () => _formattableStringParser.Parse(context.Context.Settings.NameSettings.AddMethodNameFormatString, context.Context.FormatProvider, childContext));
            resultSetBuilder.AddRange("EnumerableOverload", () => GetCodeStatementsForEnumerableOverload(context, property));
            resultSetBuilder.AddRange("ArrayOverload", () => GetCodeStatementsForArrayOverload(context, property));
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

            context.Model.AddMethods(new MethodBuilder()
                .WithName(results.First(x => x.Name == "AddMethodName").Result.Value!)
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
                        .WithTypeName(results.First(x => x.Name == "TypeName").Result.Value!.FixCollectionTypeName(typeof(IEnumerable<>).WithoutGenerics()))
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                )
                .AddStringCodeStatements(results.Where(x => x.Name == "EnumerableOverload").Select(x => x.Result.Value!))
            );

            context.Model.AddMethods(new MethodBuilder()
                .WithName(results.First(x => x.Name == "AddMethodName").Result.Value!)
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
                        .WithTypeName(results.First(x => x.Name == "TypeName").Result.Value!.FixTypeName().ConvertTypeNameToArray())
                        .WithIsParamArray()
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                )
                .AddStringCodeStatements(results.Where(x => x.Name == "ArrayOverload").Select(x => x.Result.Value!))
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
                if (!constructorInitializerResult.IsSuccessful())
                {
                    yield return constructorInitializerResult;
                }
                else
                {
                    yield return Result.Success($"if (instance.{property.GetBuilderMemberName(context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks, context.Context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Context.Settings.EntitySettings.GenerationSettings.AddBackingFields, context.Context.FormatProvider.ToCultureInfo())} is null) {property.GetBuilderMemberName(context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks, context.Context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Context.Settings.EntitySettings.GenerationSettings.AddBackingFields, context.Context.FormatProvider.ToCultureInfo())} = {constructorInitializerResult.Value};");
                }
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
