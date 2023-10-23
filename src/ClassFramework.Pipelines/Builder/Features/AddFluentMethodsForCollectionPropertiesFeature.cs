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

    public Result<BuilderContext> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (string.IsNullOrEmpty(context.Context.Settings.NameSettings.AddMethodNameFormatString))
        {
            return Result.Continue<BuilderContext>();
        }

        foreach (var property in context.Context.SourceModel.GetPropertiesFromClassAndBaseClass(context.Context.Settings).Where(x => x.TypeName.FixTypeName().IsCollectionTypeName()))
        {
            var childContext = new ParentChildContext<ClassProperty>(context, property);
            var typeName = _formattableStringParser
                .Parse(property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName), context.Context.FormatProvider, childContext)
                .GetValueOrThrow();

            context.Model.AddMethods(new ClassMethodBuilder()
                .WithName(_formattableStringParser.Parse(context.Context.Settings.NameSettings.AddMethodNameFormatString, context.Context.FormatProvider, childContext).GetValueOrThrow())
                .WithTypeName(context.Context.IsBuilderForAbstractEntity
                      ? "TBuilder" + context.Context.SourceModel.GetGenericTypeArgumentsString()
                      : _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, childContext).GetValueOrThrow() + context.Context.SourceModel.GetGenericTypeArgumentsString())
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                        .WithTypeName(typeName.FixCollectionTypeName(typeof(IEnumerable<>).WithoutGenerics()))
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                )
                .AddStringCodeStatements(context.Context.Settings.GenerationSettings.AddNullChecks
                    ? $"return Add{property.Name}({property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo())}?.ToArray() ?? throw new {typeof(ArgumentNullException).FullName}(nameof({property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo())})));"
                    : $"return Add{property.Name}({property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo())}.ToArray());")
            );

            context.Model.AddMethods(new ClassMethodBuilder()
                .WithName(_formattableStringParser.Parse(context.Context.Settings.NameSettings.AddMethodNameFormatString, context.Context.FormatProvider, childContext).GetValueOrThrow())
                .WithTypeName(context.Context.IsBuilderForAbstractEntity
                      ? "TBuilder" + context.Context.SourceModel.GetGenericTypeArgumentsString()
                      : _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, childContext).GetValueOrThrow() + context.Context.SourceModel.GetGenericTypeArgumentsString())
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                        .WithTypeName(typeName.FixTypeName().ConvertTypeNameToArray())
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                )
                .AddStringCodeStatements(GetCodeStatementsForArrayOverload(context, property))
            );
        }

        return Result.Continue<BuilderContext>();
    }

    private IEnumerable<string> GetCodeStatementsForArrayOverload(PipelineContext<ClassBuilder, BuilderContext> context, ClassProperty property)
    {
        if (context.Context.Settings.GenerationSettings.AddNullChecks)
        {
            yield return _formattableStringParser.Parse
            (
                property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentNullCheckExpression, "{NullCheck.Argument}"),
                context.Context.FormatProvider,
                new ParentChildContext<ClassProperty>(context, property)
            ).GetValueOrThrow();

            if (context.Context.Settings.ClassSettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared)
            {
                yield return $"if ({property.Name} is null) {GetInitializationName(property.Name, context.Context)} = {GetImmutableBuilderClassConstructorInitializer(context, property)};";
            }
        }

        yield return _formattableStringParser.Parse
        (
            property.Metadata.GetStringValue(MetadataNames.CustomBuilderAddExpression, () => CreateImmutableBuilderCollectionPropertyAddExpression(property, context.Context)),
            context.Context.FormatProvider,
            new ParentChildContext<ClassProperty>(context, property)
        ).GetValueOrThrow();

        yield return $"return {GetReturnValue(context.Context)};";
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddFluentMethodsForCollectionPropertiesFeatureBuilder(_formattableStringParser);

    private string GetImmutableBuilderClassConstructorInitializer(PipelineContext<ClassBuilder, BuilderContext> context, ClassProperty property)
        => _formattableStringParser.Parse
        (
            property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
            context.Context.FormatProvider,
            new ParentChildContext<ClassProperty>(context, property)
        )
        .GetValueOrThrow()
        .FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName)
        .GetCollectionInitializeStatement()
        .GetCsharpFriendlyTypeName();

    private static string CreateImmutableBuilderCollectionPropertyAddExpression(ClassProperty property, BuilderContext context)
    {
        if (context.Settings.TypeSettings.NewCollectionTypeName == typeof(IEnumerable<>).WithoutGenerics())
        {
            return $"{property.Name} = {property.Name}.Concat({property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo())});";
        }

        return $"{property.Name}.AddRange({property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo())});";
    }

    private static string GetReturnValue(BuilderContext context)
    {
        if (context.IsBuilderForAbstractEntity)
        {
            return "(TBuilder)this";
        }

        return "this";
    }

    private static string GetInitializationName(string name, BuilderContext context)
    {
        if (context.Settings.GenerationSettings.AddNullChecks && context.Settings.ClassSettings.ConstructorSettings.OriginalValidateArguments != ArgumentValidationType.Shared)
        {
            return $"_{name.ToPascalCase(context.FormatProvider.ToCultureInfo())}";
        }

        return name;
    }
}
