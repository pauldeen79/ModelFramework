namespace ClassFramework.CodeGeneration.Models.Types;

internal interface IClass : ITypeBase, IFieldsContainer, IConstructorsContainer, IRecordContainer
{
    bool Static { get; }
    bool Sealed { get; }
    bool Abstract { get; }
    [Required] IReadOnlyCollection<IClass> SubClasses { get; }
    [Required] IReadOnlyCollection<IEnumeration> Enums { get; }
    string? BaseClass { get; }
}
