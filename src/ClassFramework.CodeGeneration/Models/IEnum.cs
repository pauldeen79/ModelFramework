namespace ClassFramework.CodeGeneration.Models;

public interface IEnum : IAttributesContainer, IMetadataContainer, INameContainer, IVisibilityContainer
{
    IReadOnlyCollection<IEnumMember> Members { get; }
}
