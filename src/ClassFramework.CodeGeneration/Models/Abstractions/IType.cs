namespace ClassFramework.CodeGeneration.Models.Abstractions;

internal interface IType : IMetadataContainer, IVisibilityContainer, INameContainer, IAttributesContainer, IGenericTypeArgumentsContainer
{
    [Required(AllowEmptyStrings = true)] string Namespace { get; }
    bool Partial { get; }
    [Required] IReadOnlyCollection<string> Interfaces { get; }
    [Required] IReadOnlyCollection<IClassField> Fields { get; }
    [Required] IReadOnlyCollection<IClassProperty> Properties { get; }
    [Required] IReadOnlyCollection<IClassMethod> Methods { get; }
    [Required] IReadOnlyCollection<string> SuppressWarningCodes { get; }
}
