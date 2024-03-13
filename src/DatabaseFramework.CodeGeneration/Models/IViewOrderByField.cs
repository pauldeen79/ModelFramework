namespace DatabaseFramework.CodeGeneration.Models;

internal interface IViewOrderByField : IViewField
{
    bool IsDescending { get; }
}
