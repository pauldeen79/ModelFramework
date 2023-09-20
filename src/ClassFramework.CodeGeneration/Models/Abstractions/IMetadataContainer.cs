namespace ClassFramework.CodeGeneration.Models.Abstractions;

public interface IMetadataContainer
{
    IReadOnlyCollection<IMetadata> Metadata { get; }
}
