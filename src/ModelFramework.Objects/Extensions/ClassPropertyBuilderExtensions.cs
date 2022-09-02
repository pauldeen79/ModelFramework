namespace ModelFramework.Objects.Extensions;

public static class ClassPropertyBuilderExtensions
{
    internal static ClassPropertyBuilder ForPocoClassBuilder(this ClassPropertyBuilder builder,
                                                             IClassProperty p,
                                                             string newCollectionTypeName)
        => builder.WithName(p.Name)
            .WithTypeName(p.TypeName.FixCollectionTypeName(newCollectionTypeName))
            .WithStatic(p.Static)
            .WithVirtual(p.Virtual)
            .WithAbstract(p.Abstract)
            .WithProtected(p.Protected)
            .WithOverride(p.Override)
            .WithHasGetter(p.HasGetter)
            .WithHasSetter()
            .WithHasInitializer(false)
            .WithIsNullable(p.IsNullable)
            .WithIsValueType(p.IsValueType)
            .WithVisibility(p.Visibility)
            .WithGetterVisibility(p.GetterVisibility)
            .WithSetterVisibility(p.SetterVisibility)
            .WithInitializerVisibility(p.InitializerVisibility);

    public static ClassPropertyBuilder WithConstructorNullCheck(this ClassPropertyBuilder builder, bool nullCheck = true)
        => builder.AddMetadata(TypeBaseExtensions.NullCheckMetadataValue, nullCheck);
}
