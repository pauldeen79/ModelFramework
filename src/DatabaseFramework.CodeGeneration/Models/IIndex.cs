namespace DatabaseFramework.CodeGeneration.Models;

internal interface IIndex : INameContainer, IMetadataContainer, IFileGroupNameContainer
{
    [Required] IReadOnlyCollection<IIndexField> Fields { get; }
    bool Unique { get; }
}
