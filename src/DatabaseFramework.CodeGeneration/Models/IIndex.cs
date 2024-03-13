namespace DatabaseFramework.CodeGeneration.Models;

internal interface IIndex : Abstractions.INameContainer, Abstractions.IMetadataContainer, IFileGroupNameContainer
{
    [Required] IReadOnlyCollection<IIndexField> Fields { get; }
    bool Unique { get; }
}
