namespace ClassFramework.CodeGeneration.Models;

internal interface IEnum : IAttributesContainer, IMetadataContainer, INameContainer, IVisibilityContainer
{
    [Required] IReadOnlyCollection<IEnumMember> Members { get; }
}
