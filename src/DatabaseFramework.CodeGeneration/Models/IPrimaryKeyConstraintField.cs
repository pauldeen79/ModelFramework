namespace DatabaseFramework.CodeGeneration.Models;

public interface IPrimaryKeyConstraintField : INameContainer, IMetadataContainer
{
    bool IsDescending { get; }
}
