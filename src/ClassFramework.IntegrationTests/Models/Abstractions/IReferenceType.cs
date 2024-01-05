namespace ClassFramework.IntegrationTests.Models.Abstractions;

internal interface IReferenceType : IType
{
    bool Static { get; }
    bool Sealed { get; }
    bool Abstract { get; }
    [Required] IReadOnlyCollection<IClass> SubClasses { get; }
    [Required] IReadOnlyCollection<IEnumeration> Enums { get; }
}
