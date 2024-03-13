namespace DatabaseFramework.CodeGeneration.Models;

internal interface IIndexField : Abstractions.INameContainer, Abstractions.IMetadataContainer
{
    bool IsDescending { get; }
}
