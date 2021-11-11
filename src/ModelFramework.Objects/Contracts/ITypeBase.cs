using System.Collections.Generic;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Objects.Contracts
{
    public interface ITypeBase : IMetadataContainer, IVisibilityContainer, INameContainer, IAttributesContainer
    {
        string Namespace { get; }
        bool Partial { get; }
        IReadOnlyCollection<string> Interfaces { get; }
        IReadOnlyCollection<IClassProperty> Properties { get; }
        IReadOnlyCollection<IClassMethod> Methods { get; }
        string[] GenericTypeArguments { get; }
    }
}
