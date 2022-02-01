namespace ModelFramework.Objects.Contracts;

public interface IEnum : IAttributesContainer, IMetadataContainer, INameContainer, IVisibilityContainer
{
    ValueCollection<IEnumMember> Members { get; }
}
