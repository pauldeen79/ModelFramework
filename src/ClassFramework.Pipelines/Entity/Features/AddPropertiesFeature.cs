namespace ClassFramework.Pipelines.Entity.Features;

public class AddPropertiesFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, EntityContext> Build()
        => new AddPropertiesFeature();
}

public class AddPropertiesFeature : IPipelineFeature<ClassBuilder, EntityContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.AddProperties(
            context.Context.SourceModel
                .Properties
                .Where(property => context.Context.SourceModel.IsMemberValidForBuilderClass(property, context.Context.Settings))
                .Select
                (
                    property => new ClassPropertyBuilder()
                        .WithName(property.Name)
                        .WithTypeName(context.Context.MapTypeName(property.TypeName
                            .FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName)
                            .FixNullableTypeName(property)))
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                        .AddAttributes(property.Attributes
                            .Where(x => context.Context.Settings.CopySettings.CopyAttributePredicate?.Invoke(x) ?? true)
                            .Select(x => new AttributeBuilder(context.Context.MapAttribute(x))))
                        .WithStatic(property.Static)
                        .WithVirtual(property.Virtual)
                        .WithAbstract(property.Abstract)
                        .WithProtected(property.Protected)
                        .WithOverride(property.Override)
                        .WithHasGetter(property.HasGetter)
                        .WithHasInitializer(property.HasInitializer)
                        .WithHasSetter(context.Context.Settings.GenerationSettings.AddSetters)
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                        .WithVisibility(property.Visibility)
                        .WithGetterVisibility(property.GetterVisibility)
                        .WithSetterVisibility(context.Context.Settings.GenerationSettings.SetterVisibility)
                        .WithInitializerVisibility(property.InitializerVisibility)
                        .WithExplicitInterfaceName(property.ExplicitInterfaceName)
                        .AddMetadata(property.Metadata.Select(x => new MetadataBuilder(x)))
                        .AddGetterCodeStatements(property.GetterCodeStatements.Select(x => x.ToBuilder()))
                        .AddSetterCodeStatements(property.SetterCodeStatements.Select(x => x.ToBuilder()))
                        .AddInitializerCodeStatements(property.InitializerCodeStatements.Select(x => x.ToBuilder()))
                ));

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, EntityContext>> ToBuilder()
        => new AddPropertiesFeatureBuilder();
}
