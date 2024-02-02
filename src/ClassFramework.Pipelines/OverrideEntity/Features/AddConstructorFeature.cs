namespace ClassFramework.Pipelines.OverrideEntity.Features;

public class AddConstructorFeatureBuilder : IOverrideEntityFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddConstructorFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, OverrideEntityContext> Build()
        => new AddConstructorFeature(_formattableStringParser);
}

public class AddConstructorFeature : IPipelineFeature<IConcreteTypeBuilder, OverrideEntityContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddConstructorFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, OverrideEntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        var ctorResult = CreateOverrideEntityConstructor(context);
        if (!ctorResult.IsSuccessful())
        {
            return Result.FromExistingResult<IConcreteTypeBuilder>(ctorResult);
        }

        context.Model.AddConstructors(ctorResult.Value!);

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, OverrideEntityContext>> ToBuilder()
        => new AddConstructorFeatureBuilder(_formattableStringParser);

    private Result<ConstructorBuilder> CreateOverrideEntityConstructor(PipelineContext<IConcreteTypeBuilder, OverrideEntityContext> context)
    {
        var parameters = string.Join(", ", context.Context.SourceModel.Properties.Select(x => x.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()).GetCsharpFriendlyName()));

        return Result.Success(new ConstructorBuilder()
            .WithProtected(context.Context.Settings.InheritanceSettings.EnableInheritance && context.Context.Settings.InheritanceSettings.IsAbstract)
            .WithChainCall($"base({parameters})")
            //.AddParameters(context.CreateImmutableClassCtorParameters())
            .AddParameters(context.Context.SourceModel.Properties
                .Select
                (
                    property => new ParameterBuilder()
                        .WithName(property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo()))
                        .WithTypeName
                        (
                            context.Context.MapTypeName
                            (
                                property.Metadata
                                    .WithMappingMetadata(property.TypeName.GetCollectionItemType().WhenNullOrEmpty(property.TypeName), context.Context.Settings.TypeSettings)
                                    .GetStringValue(MetadataNames.CustomImmutableArgumentType, () => property.TypeName)
                            ).FixCollectionTypeName(/*context.Context.Settings.ConstructorSettings.CollectionTypeName.WhenNullOrEmpty(*/typeof(IEnumerable<>).WithoutGenerics()/*)*/)
                        )
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                ))
            );
    }
}
