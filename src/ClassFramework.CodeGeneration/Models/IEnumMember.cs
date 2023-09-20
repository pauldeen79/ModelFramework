namespace ClassFramework.CodeGeneration.Models;

public interface IEnumMember : IAttributesContainer, INameContainer, IMetadataContainer
{
    object? Value { get; }
}
