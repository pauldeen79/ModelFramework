namespace DatabaseFramework.CodeGeneration.Models;

internal interface IPrimaryKeyConstraintField : INameContainer, IMetadataContainer
{
    bool IsDescending { get; }
}
