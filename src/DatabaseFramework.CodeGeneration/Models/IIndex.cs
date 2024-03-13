namespace DatabaseFramework.CodeGeneration.Models;

public interface IIndex : INameContainer, IMetadataContainer, IFileGroupNameContainer
{
    [Required] IReadOnlyCollection<IIndexField> Fields { get; }
    bool Unique { get; }
}
