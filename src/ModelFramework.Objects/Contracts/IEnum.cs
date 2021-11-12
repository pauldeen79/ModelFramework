using CrossCutting.Common;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Objects.Contracts
{
    public interface IEnum : IAttributesContainer, IMetadataContainer, INameContainer, IVisibilityContainer
    {
        ValueCollection<IEnumMember> Members { get;  }
    }
}