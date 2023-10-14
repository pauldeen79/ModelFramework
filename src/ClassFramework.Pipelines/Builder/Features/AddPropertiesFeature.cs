namespace ClassFramework.Pipelines.Builder.Features;

public class AddPropertiesFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddPropertiesFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new AddPropertiesFeature(_formattableStringParser);
}

public class AddPropertiesFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddPropertiesFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public void Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.IsAbstractBuilder)
        {
            return;
        }

        context.Model.AddProperties(
            context.Context.SourceModel.Properties
            .Where
            (
                x => context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings)
            )
            .Select(property => new ClassPropertyBuilder()
                .WithName(property.Name)
                .WithTypeName
                (
                    _formattableStringParser
                        .Parse(property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName), context.Context.FormatProvider, property)
                        .GetValueOrThrow()
                        .FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName)
                )
                .WithIsNullable(property.IsNullable)
                .WithIsValueType(property.IsValueType)
                .AddAttributes(property.Attributes.Where(_ => context.Context.Settings.GenerationSettings.CopyAttributes).Select(x => new AttributeBuilder(x)))
                .AddMetadata(property.Metadata.Select(x => new MetadataBuilder(x)))
                .AddGetterCodeStatements(CreateImmutableBuilderPropertyGetterStatements(property, context.Context))
                .AddSetterCodeStatements(CreateImmutableBuilderPropertySetterStatements(property, context.Context))
            )
        ).AddFields(context.Context.SourceModel.GetImmutableBuilderClassFields(context.Context, _formattableStringParser));
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddPropertiesFeatureBuilder(_formattableStringParser);

    private static IEnumerable<CodeStatementBaseBuilder> CreateImmutableBuilderPropertyGetterStatements(
        ClassProperty property,
        BuilderContext context)
    {
        if (context.Settings.GenerationSettings.AddNullChecks && context.Settings.ClassSettings.ConstructorSettings.OriginalValidateArguments != ArgumentValidationType.Shared)
        {
            yield return new StringCodeStatementBuilder().WithStatement($"return _{property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo())};");
        }
    }

    private static IEnumerable<CodeStatementBaseBuilder> CreateImmutableBuilderPropertySetterStatements(
        ClassProperty property,
        BuilderContext context)
    {
        if (context.Settings.GenerationSettings.AddNullChecks && context.Settings.ClassSettings.ConstructorSettings.OriginalValidateArguments != ArgumentValidationType.Shared)
        {
            yield return new StringCodeStatementBuilder().WithStatement($"_{property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo())} = value{property.GetNullCheckSuffix("value", context.Settings.GenerationSettings.AddNullChecks)};");
        }
    }
}
