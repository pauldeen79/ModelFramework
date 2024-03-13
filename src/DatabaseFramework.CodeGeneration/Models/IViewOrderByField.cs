namespace DatabaseFramework.CodeGeneration.Models;

internal interface IViewOrderByField : Abstractions.IViewField
{
    bool IsDescending { get; }
}
