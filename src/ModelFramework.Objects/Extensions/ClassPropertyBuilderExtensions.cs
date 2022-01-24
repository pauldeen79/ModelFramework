using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    internal static class ClassPropertyBuilderExtensions
    {
        internal static ClassPropertyBuilder ForPocoClassBuilder(this ClassPropertyBuilder builder, IClassProperty p, string newCollectionTypeName)
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
                .WithVisibility(p.Visibility)
                .WithGetterVisibility(p.GetterVisibility)
                .WithSetterVisibility(p.SetterVisibility)
                .WithInitializerVisibility(p.InitializerVisibility);
    }
}
