namespace ClassFramework.IntegrationTests.Models;

internal interface IMetadata : INameContainer
{
    object? Value { get; }
}
