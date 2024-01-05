namespace ClassFramework.IntegrationTests.Models;

internal interface IAttribute : IMetadataContainer, INameContainer
{
    [Required] IReadOnlyCollection<IAttributeParameter> Parameters { get; }
}
