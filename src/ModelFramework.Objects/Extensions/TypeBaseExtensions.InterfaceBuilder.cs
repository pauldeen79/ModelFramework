using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    public static partial class TypeBaseExtensions
    {
        public static IInterface ToInterface(this ITypeBase instance)
            => instance.ToInterfaceBuilder().Build();

        public static InterfaceBuilder ToInterfaceBuilder(this ITypeBase instance)
            => new InterfaceBuilder()
                .WithName(instance.Name)
                .WithNamespace(instance.Namespace)
                .WithPartial(instance.Partial)
                .WithVisibility(instance.Visibility)
                .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)))
                .AddMetadata(instance.Metadata.Select(x => new MetadataBuilder(x)))
                .AddMethods(instance.Methods.Select(x => new ClassMethodBuilder(x)))
                .AddProperties(instance.Properties.Select(x => new ClassPropertyBuilder(x)));
    }
}
