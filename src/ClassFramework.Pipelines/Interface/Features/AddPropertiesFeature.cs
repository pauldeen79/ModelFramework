namespace ClassFramework.Pipelines.Interface.Features;

public class AddPropertiesFeatureBuilder : IInterfaceFeatureBuilder
{
    public IPipelineFeature<InterfaceBuilder, InterfaceContext> Build()
        => new AddPropertiesFeature();
}

public class AddPropertiesFeature : IPipelineFeature<InterfaceBuilder, InterfaceContext>
{
    public Result<InterfaceBuilder> Process(PipelineContext<InterfaceBuilder, InterfaceContext> context)
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
                        .FixCollectionTypeName(context.Context.Settings.EntityNewCollectionTypeName)
                        .FixNullableTypeName(property)))
                    .WithIsNullable(property.IsNullable)
                    .WithIsValueType(property.IsValueType)
                    .AddAttributes(property.Attributes
                        .Where(x => context.Context.Settings.CopyAttributes && (context.Context.Settings.CopyAttributePredicate?.Invoke(x) ?? true))
                        .Select(x => context.Context.MapAttribute(x).ToBuilder()))
                    .WithStatic(property.Static)
                    .WithHasGetter(property.HasGetter)
                    .WithHasInitializer(false)
                    .WithHasSetter(property.HasSetter && context.Context.Settings.AddSetters)
                    .WithIsNullable(property.IsNullable)
                    .WithIsValueType(property.IsValueType)
                    .WithVisibility(property.Visibility)
                    .WithParentTypeFullName(property.ParentTypeFullName)
                    .AddMetadata(property.Metadata.Select(x => x.ToBuilder()))
            )
        );

        return Result.Continue<InterfaceBuilder>();
    }

    public IBuilder<IPipelineFeature<InterfaceBuilder, InterfaceContext>> ToBuilder()
        => new AddPropertiesFeatureBuilder();
}
