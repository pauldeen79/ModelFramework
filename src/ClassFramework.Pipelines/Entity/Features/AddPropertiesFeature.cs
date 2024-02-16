﻿namespace ClassFramework.Pipelines.Entity.Features;

public class AddPropertiesFeatureBuilder : IEntityFeatureBuilder
{
    public IPipelineFeature<IConcreteTypeBuilder, EntityContext> Build()
        => new AddPropertiesFeature();
}

public class AddPropertiesFeature : IPipelineFeature<IConcreteTypeBuilder, EntityContext>
{
    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, EntityContext> context)
    {
        context = context.IsNotNull(nameof(context));

        var properties = context.Context.SourceModel
            .Properties
            .Where(property => context.Context.SourceModel.IsMemberValidForBuilderClass(property, context.Context.Settings))
            .ToArray();

        context.Model.AddProperties(
            properties.Select
            (
                property => new PropertyBuilder()
                    .WithName(property.Name)
                    .WithTypeName(context.Context.MapTypeName(property.TypeName
                        .FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName)
                        .FixNullableTypeName(property)))
                    .WithIsNullable(property.IsNullable)
                    .WithIsValueType(property.IsValueType)
                    .AddAttributes(property.Attributes
                        .Where(x => context.Context.Settings.CopySettings.CopyAttributes && (context.Context.Settings.CopySettings.CopyAttributePredicate?.Invoke(x) ?? true))
                        .Select(x => context.Context.MapAttribute(x).ToBuilder()))
                    .WithStatic(property.Static)
                    .WithVirtual(property.Virtual)
                    .WithAbstract(property.Abstract)
                    .WithProtected(property.Protected)
                    .WithOverride(property.Override)
                    .WithHasGetter(property.HasGetter)
                    .WithHasInitializer(property.HasInitializer && !(context.Context.Settings.GenerationSettings.AddSetters || context.Context.Settings.GenerationSettings.AddBackingFields))
                    .WithHasSetter(context.Context.Settings.GenerationSettings.AddSetters || context.Context.Settings.GenerationSettings.AddBackingFields)
                    .WithIsNullable(property.IsNullable)
                    .WithIsValueType(property.IsValueType)
                    .WithVisibility(property.Visibility)
                    .WithGetterVisibility(property.GetterVisibility)
                    .WithSetterVisibility(context.Context.Settings.GenerationSettings.SetterVisibility)
                    .WithInitializerVisibility(property.InitializerVisibility)
                    .WithExplicitInterfaceName(property.ExplicitInterfaceName)
                    .WithParentTypeFullName(property.ParentTypeFullName)
                    .AddMetadata(property.Metadata.Select(x => x.ToBuilder()))
                    .AddGetterCodeStatements(CreateBuilderPropertyGetterStatements(property, context.Context))
                    .AddSetterCodeStatements(CreateBuilderPropertySetterStatements(property, context.Context))
            )
        );

        if (context.Context.Settings.GenerationSettings.AddBackingFields)
        {
            AddBackingFields(context, properties);
        }

        return Result.Continue<IConcreteTypeBuilder>();
    }

    private static void AddBackingFields(PipelineContext<IConcreteTypeBuilder, EntityContext> context, Property[] properties)
        => context.Model.AddFields
        (
            properties
                .Select
                (
                    property =>new FieldBuilder()
                        .WithName($"_{property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo())}")
                        .WithTypeName(context.Context.MapTypeName(property.TypeName)
                            .FixCollectionTypeName(context.Context.Settings.ConstructorSettings.CollectionTypeName
                                .WhenNullOrEmpty(context.Context.Settings.TypeSettings.NewCollectionTypeName)
                                .WhenNullOrEmpty(typeof(List<>).WithoutGenerics()))
                        .FixNullableTypeName(property))
                        .WithIsNullable(property.IsNullable)
                        .WithIsValueType(property.IsValueType)
                )
        );

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, EntityContext>> ToBuilder()
        => new AddPropertiesFeatureBuilder();

    private static IEnumerable<CodeStatementBaseBuilder> CreateBuilderPropertyGetterStatements(
        Property property,
        EntityContext context)
    {
        if (context.Settings.GenerationSettings.AddBackingFields)
        {
            yield return new StringCodeStatementBuilder().WithStatement($"return _{property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo())};");
        }
    }

    private static IEnumerable<CodeStatementBaseBuilder> CreateBuilderPropertySetterStatements(
        Property property,
        EntityContext context)
    {
        if (context.Settings.GenerationSettings.AddBackingFields)
        {
            yield return new StringCodeStatementBuilder().WithStatement($"_{property.Name.ToPascalCase(context.FormatProvider.ToCultureInfo())} = value{property.GetNullCheckSuffix("value", context.Settings.NullCheckSettings.AddNullChecks)};");
            if (context.Settings.GenerationSettings.CreateAsObservable)
            {
                yield return new StringCodeStatementBuilder().WithStatement($"HandlePropertyChanged(nameof({property.Name}));");
            }
        }
    }
}
