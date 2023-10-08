namespace ClassFramework.Pipelines.Builder.Features;

public class AddPropertiesFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddPropertiesFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, PipelineBuilderContext> Build()
        => new AddPropertiesFeature(_formattableStringParser);
}

public class AddPropertiesFeature : IPipelineFeature<ClassBuilder, PipelineBuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddPropertiesFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public void Process(PipelineContext<ClassBuilder, PipelineBuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.AddProperties(
            context.Context.SourceModel.Properties
            .Where
            (
                x => !context.Context.Settings.IsAbstractBuilder && context.Context.SourceModel.IsMemberValidForImmutableBuilderClass(x, context.Context.Settings.InheritanceSettings, isForWithStatement: false)
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
            )
        );
    }

    public IBuilder<IPipelineFeature<ClassBuilder, PipelineBuilderContext>> ToBuilder()
        => new AddPropertiesFeatureBuilder(_formattableStringParser);
}
