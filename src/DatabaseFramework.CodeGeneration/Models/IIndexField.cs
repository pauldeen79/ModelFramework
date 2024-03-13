namespace DatabaseFramework.CodeGeneration.Models;

public interface IIndexField : INameContainer, IMetadataContainer
{
    bool IsDescending { get; }
}
