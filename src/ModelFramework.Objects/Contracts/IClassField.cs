using ModelFramework.Common.Contracts;

namespace ModelFramework.Objects.Contracts
{
    public interface IClassField : IMetadataContainer, IExtendedVisibilityContainer, INameContainer, IAttributesContainer, ITypeContainer, IDefaultValueContainer
    {
        bool ReadOnly { get; }
        bool Constant { get; }
        bool Event { get; }
    }
}
