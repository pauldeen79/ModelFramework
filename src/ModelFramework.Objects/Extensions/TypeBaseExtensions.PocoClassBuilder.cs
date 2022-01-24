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
                                .ForPocoClassBuilder(p, newCollectionTypeName)
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
