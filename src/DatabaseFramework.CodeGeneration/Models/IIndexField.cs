namespace DatabaseFramework.CodeGeneration.Models;

internal interface IIndexField : INameContainer, IMetadataContainer
{
    bool IsDescending { get; }
}
