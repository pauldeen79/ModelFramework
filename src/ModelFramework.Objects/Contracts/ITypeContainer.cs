namespace ModelFramework.Objects.Contracts;

public interface ITypeContainer
{
    string TypeName { get; }
    bool IsNullable { get; }
}
