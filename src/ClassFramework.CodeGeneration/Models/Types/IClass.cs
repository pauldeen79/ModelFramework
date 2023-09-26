namespace ClassFramework.CodeGeneration.Models.Types;

public interface IClass : IType
{
    [Required] IReadOnlyCollection<IClassField> Fields { get; }
    bool Static { get; }
    bool Sealed { get; }
    bool Abstract { get; }
    [Required] IReadOnlyCollection<IClass> SubClasses { get; }
    [Required] IReadOnlyCollection<IClassConstructor> Constructors { get; }
    [Required] IReadOnlyCollection<IEnum> Enums { get; }
    string? BaseClass { get; }
    bool Record { get; }
}
