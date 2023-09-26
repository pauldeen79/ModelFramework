namespace ClassFramework.CodeGeneration.Models.Abstractions;

public interface ITypeContainer
{
    [Required] string TypeName { get; }
    bool IsNullable { get; }
    bool IsValueType { get; }
}
