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

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (context.Context.IsAbstractBuilder)
        {
            return Result.Continue<ClassBuilder>();
        }

        foreach (var property in context.Context.SourceModel.Properties.Where(x => context.Context.SourceModel.IsMemberValidForBuilderClass(x, context.Context.Settings)))
        {
            var typeNameResult = _formattableStringParser.Parse(property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName), context.Context.FormatProvider, new ParentChildContext<BuilderContext, ClassProperty>(context, property, context.Context.Settings.GenerationSettings));

            if (!typeNameResult.IsSuccessful())
            {
                return Result.FromExistingResult<ClassBuilder>(typeNameResult);
            }

            context.Model.AddProperties(new ClassPropertyBuilder()
                .WithName(property.Name)
                .WithTypeName(typeNameResult.Value!.FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName))
                .WithIsNullable(property.IsNullable)
                .WithIsValueType(property.IsValueType)
                .AddAttributes(property.Attributes.Where(_ => context.Context.Settings.GenerationSettings.CopyAttributes).Select(x => new AttributeBuilder(x)))
                .AddMetadata(property.Metadata.Select(x => new MetadataBuilder(x)))
                .AddGetterCodeStatements(CreateBuilderPropertyGetterStatements(property, context.Context))
                .AddSetterCodeStatements(CreateBuilderPropertySetterStatements(property, context.Context))
            );
        }

        // Note that we are not checking the result, because the same formattable string (CustomBuilderArgumentType) has already been checked earlier in this class
        // We can simple use GetValueOrThrow to keep the compiler happy (the value should be a string, and not be null)
        context.Model.AddFields(context.Context.SourceModel
            .GetBuilderClassFields(context, _formattableStringParser)
            .Select(x => x.GetValueOrThrow()));

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new AddPropertiesFeatureBuilder(_formattableStringParser);

    private static IEnumerable<CodeStatementBaseBuilder> CreateBuilderPropertyGetterStatements(
        ClassProperty property,
        BuilderContext context)
    {
        if (context.Settings.GenerationSettings.AddNullChecks && context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments != ArgumentValidationType.Shared)
        {
            yield return new StringCodeStatementBuilder().WithStatement($"return _{property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo())};");
        }
    }

    private static IEnumerable<CodeStatementBaseBuilder> CreateBuilderPropertySetterStatements(
        ClassProperty property,
        BuilderContext context)
    {
        if (context.Settings.GenerationSettings.AddNullChecks && context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments != ArgumentValidationType.Shared)
        {
            yield return new StringCodeStatementBuilder().WithStatement($"_{property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo())} = value{property.GetNullCheckSuffix("value", context.Settings.GenerationSettings.AddNullChecks)};");
        }
    }
}
