namespace ModelFramework.Objects.Contracts;

public interface IClassField : IMetadataContainer, IExtendedVisibilityContainer, INameContainer, IAttributesContainer, ITypeContainer, IDefaultValueContainer, IParentTypeContainer
{
    bool ReadOnly { get; }
    bool Constant { get; }
    bool Event { get; }
}
