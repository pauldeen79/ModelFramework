using CrossCutting.Common;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Objects.Contracts
{
    public interface ITypeBase : IMetadataContainer, IVisibilityContainer, INameContainer, IAttributesContainer
    {
        string Namespace { get; }
        bool Partial { get; }
        ValueCollection<string> Interfaces { get; }
        ValueCollection<IClassProperty> Properties { get; }
        ValueCollection<IClassMethod> Methods { get; }
        string[] GenericTypeArguments { get; }
    }
}
