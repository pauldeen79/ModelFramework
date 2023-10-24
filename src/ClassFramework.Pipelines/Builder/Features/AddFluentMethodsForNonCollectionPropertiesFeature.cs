namespace ClassFramework.Pipelines.Builder.Features;

public class AddFluentMethodsForNonCollectionPropertiesFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddFluentMethodsForNonCollectionPropertiesFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AddFluentMethodsForNonCollectionPropertiesFeature(_formattableStringParser);
}

public class AddFluentMethodsForNonCollectionPropertiesFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddFluentMethodsForNonCollectionPropertiesFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (string.IsNullOrEmpty(context.Context.Settings.NameSettings.SetMethodNameFormatString))
        {
            return Result.Continue<ClassBuilder>();
        }

        foreach (var property in context.Context.SourceModel.GetPropertiesFromClassAndBaseClass(context.Context.Settings).Where(x => !x.TypeName.FixTypeName().IsCollectionTypeName()))
        {
            var childContext = new ParentChildContext<BuilderContext, ClassProperty>(context, property);
            var typeName = _formattableStringParser
                .Parse(property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName), context.Context.FormatProvider, childContext)
                .GetValueOrThrow();

            context.Model.AddMethods(new ClassMethodBuilder()
                .WithName(_formattableStringParser.Parse(context.Context.Settings.NameSettings.SetMethodNameFormatString, context.Context.FormatProvider, childContext).GetValueOrThrow())
                .WithTypeName(context.Context.IsBuilderForAbstractEntity
                      ? "TBuilder" + context.Context.SourceModel.GetGenericTypeArgumentsString()
                      : _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, childContext).GetValueOrThrow() + context.Context.SourceModel.GetGenericTypeArgumentsString())
                .AddParameters
                (
                    new ParameterBuilder()
                        .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                        .WithTypeName(typeName)
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                        .WithDefaultValue(property.Metadata.GetValue<object?>(MetadataNames.CustomBuilderWithDefaultPropertyValue, () => null))
                )
                .AddStringCodeStatements
                (
                    new[]
                    {
                        _formattableStringParser.Parse
                        (
                            property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentNullCheckExpression, "{NullCheck.Argument}"),
                            context.Context.FormatProvider,
                            childContext
                        ).GetValueOrThrow()
                    }.Where(_ => context.Context.Settings.GenerationSettings.AddNullChecks)
                )
                .AddStringCodeStatements
                (
                    _formattableStringParser.Parse
                    (
                        property.Metadata.GetStringValue(MetadataNames.CustomBuilderWithExpression, "{Name} = {NamePascal};"),
                        context.Context.FormatProvider,
                        childContext
                    ).GetValueOrThrow(),
                    $"return {GetReturnValue(context.Context)};"
                ));
        }

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
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
