namespace ClassFramework.CodeGeneration.Models.Abstractions;

public interface IMetadataContainer
{
    [Required] IReadOnlyCollection<IMetadata> Metadata { get; }
}
