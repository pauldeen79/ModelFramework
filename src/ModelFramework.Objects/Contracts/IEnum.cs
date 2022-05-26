namespace ModelFramework.Objects.Contracts;

public interface IEnum : IAttributesContainer, IMetadataContainer, INameContainer, IVisibilityContainer
{
    IReadOnlyCollection<IEnumMember> Members { get; }
}
