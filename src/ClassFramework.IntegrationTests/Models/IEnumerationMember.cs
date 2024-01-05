namespace ClassFramework.IntegrationTests.Models;

internal interface IEnumerationMember : IAttributesContainer, INameContainer, IMetadataContainer
{
    object? Value { get; }
}
