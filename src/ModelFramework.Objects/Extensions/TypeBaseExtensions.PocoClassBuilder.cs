using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    public static partial class TypeBaseExtensions
    {
        public static IClass ToPocoClass(this ITypeBase instance,
                                         string newCollectionTypeName = "System.Collections.Generic.ICollection")
            => instance.ToPocoClassBuilder(newCollectionTypeName).Build();

        public static ClassBuilder ToPocoClassBuilder(this ITypeBase instance,
                                                      string newCollectionTypeName = "System.Collections.Generic.ICollection")
            => new ClassBuilder()
                .WithName(instance.Name)
                .WithNamespace(instance.Namespace)
                .AddProperties
                (
                    instance
                        .Properties
                        .Select
                        (
                            p => new ClassPropertyBuilder()
                                .WithName(p.Name)
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
                                .WithInitializerVisibility(p.InitializerVisibility)
                                .WithExplicitInterfaceName(p.ExplicitInterfaceName)
                                .AddMetadata
                                (
                                    p.Metadata
                                        .Concat(p.GetBuilderCollectionMetadata(newCollectionTypeName))
                                        .Select(x => new MetadataBuilder(x))
                                )
                                .AddAttributes(p.Attributes.Select(x => new AttributeBuilder(x)))
                                .AddGetterCodeStatements(p.GetterCodeStatements.Select(x => x.CreateBuilder()))
                                .AddSetterCodeStatements(p.SetterCodeStatements.Select(x => x.CreateBuilder()))
                        )
                )
                .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)));
    }
}
