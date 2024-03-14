namespace DatabaseFramework.CodeGeneration.Models;

internal interface IIndex : INameContainer, IMetadataContainer, IFileGroupNameContainer
{
    [Required][MinCount(1)] IReadOnlyCollection<IIndexField> Fields { get; }
    bool Unique { get; }
}
