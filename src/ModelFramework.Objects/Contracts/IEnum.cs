using System.Collections.Generic;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Objects.Contracts
{
    public interface IEnum : IAttributesContainer, IMetadataContainer, INameContainer, IVisibilityContainer
    {
        IReadOnlyCollection<IEnumMember> Members { get;  }
    }
}