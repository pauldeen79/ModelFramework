namespace ClassFramework.CodeGeneration.Models.Abstractions;

public interface ITypeContainer
{
    string TypeName { get; }
    bool IsNullable { get; }
    bool IsValueType { get; }
}
