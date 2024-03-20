namespace DatabaseFramework.CodeGeneration.Models;

internal interface IIndexField : INameContainer
{
    bool IsDescending { get; }
}
