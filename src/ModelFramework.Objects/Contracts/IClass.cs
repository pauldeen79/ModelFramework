namespace ModelFramework.Objects.Contracts;

public interface IClass : ITypeBase, IEnumsContainer
{
    IReadOnlyCollection<IClassField> Fields { get; }
    bool Static { get; }
    bool Sealed { get; }
    IReadOnlyCollection<IClass> SubClasses { get; }
    IReadOnlyCollection<IClassConstructor> Constructors { get; }
    string BaseClass { get; }
    bool Record { get; }
}
