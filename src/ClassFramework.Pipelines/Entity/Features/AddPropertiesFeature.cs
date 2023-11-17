namespace ClassFramework.Pipelines.Entity.Features;

public class AddPropertiesFeatureBuilder : IEntityFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddPropertiesFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, EntityContext> Build()
        => new AddPropertiesFeature(_formattableStringParser);
}

public class AddPropertiesFeature : IPipelineFeature<ClassBuilder, EntityContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public AddPropertiesFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.AddProperties(
            context.Context.Model
                .Properties
                .Where(x => context.Context.Model.IsMemberValidForBuilderClass(x, context.Context.Settings))
                .Select
                (
                    p => new ClassPropertyBuilder()
                        .WithName(p.Name)
                        .WithTypeName(p.TypeName
                            .FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName)
                            .FixNullableTypeName(p))
                        .WithIsNullable(p.IsNullable)
                        .WithIsValueType(p.IsValueType)
                        .AddAttributes(p.Attributes
                            .Where(x => context.Context.Settings.GenerationSettings.CopyAttributePredicate?.Invoke(x) ?? true)
                            .Select(x => new AttributeBuilder(context.Context.MapAttribute(x))))
                        .WithStatic(p.Static)
                        .WithVirtual(p.Virtual)
                        .WithAbstract(p.Abstract)
                        .WithProtected(p.Protected)
                        .WithOverride(p.Override)
                        .WithHasGetter(p.HasGetter)
                        .WithHasInitializer(p.HasInitializer)
                        .WithHasSetter(context.Context.Settings.GenerationSettings.AddSetters)
                        .WithIsNullable(p.IsNullable)
                        .WithIsValueType(p.IsValueType)
                        .WithVisibility(p.Visibility)
                        .WithGetterVisibility(p.GetterVisibility)
                        .WithSetterVisibility(context.Context.Settings.GenerationSettings.SetterVisibility)
                        .WithInitializerVisibility(p.InitializerVisibility)
                        .WithExplicitInterfaceName(p.ExplicitInterfaceName)
                        .AddMetadata(p.Metadata.Select(x => new MetadataBuilder(x))) //TODO: Also add metadata on class level to both the entities and builders
                        .AddAttributes(p.Attributes.Select(x => new AttributeBuilder(x)))
                        .AddGetterCodeStatements(p.GetterCodeStatements.Select(x => x.ToBuilder()))
                        .AddSetterCodeStatements(p.SetterCodeStatements.Select(x => x.ToBuilder()))
                        .AddInitializerCodeStatements(p.InitializerCodeStatements.Select(x => x.ToBuilder()))
                ));

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new AddPropertiesFeatureBuilder(_formattableStringParser);
}
