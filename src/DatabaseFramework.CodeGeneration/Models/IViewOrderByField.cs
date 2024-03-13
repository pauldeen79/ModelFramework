namespace DatabaseFramework.CodeGeneration.Models;

public interface IViewOrderByField : IViewField
{
    bool IsDescending { get; }
}
