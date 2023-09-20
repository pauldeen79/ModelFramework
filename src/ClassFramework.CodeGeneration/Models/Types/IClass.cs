namespace ClassFramework.CodeGeneration.Models.Types;

public interface IClass : IType
{
    IReadOnlyCollection<IClassField> Fields { get; }
    bool Static { get; }
    bool Sealed { get; }
    bool Abstract { get; }
    IReadOnlyCollection<IClass> SubClasses { get; }
    IReadOnlyCollection<IClassConstructor> Constructors { get; }
    IReadOnlyCollection<IEnum> Enums { get; }
    string BaseClass { get; }
    bool Record { get; }
}
