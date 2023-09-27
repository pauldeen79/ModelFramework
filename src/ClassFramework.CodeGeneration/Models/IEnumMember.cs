namespace ClassFramework.CodeGeneration.Models;

internal interface IEnumMember : IAttributesContainer, INameContainer, IMetadataContainer
{
    object? Value { get; }
}
