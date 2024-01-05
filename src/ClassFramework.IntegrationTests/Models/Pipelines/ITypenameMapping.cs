namespace ClassFramework.IntegrationTests.Models.Pipelines;

internal interface ITypenameMapping : IMetadataContainer
{
    [Required] string SourceTypeName { get; }
    [Required] string TargetTypeName { get; }
}
