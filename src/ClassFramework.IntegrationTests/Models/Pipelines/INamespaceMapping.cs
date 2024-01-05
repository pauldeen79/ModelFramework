namespace ClassFramework.IntegrationTests.Models.Pipelines;

internal interface INamespaceMapping : IMetadataContainer
{
    [Required] string SourceNamespace { get; }
    [Required] string TargetNamespace { get; }
}
