namespace DatabaseFramework.CodeGeneration.Models;

internal interface IPrimaryKeyConstraintField : INameContainer
{
    bool IsDescending { get; }
}
