namespace DatabaseFramework.CodeGeneration.Models;

internal interface IPrimaryKeyConstraintField : Abstractions.INameContainer, Abstractions.IMetadataContainer
{
    bool IsDescending { get; }
}
