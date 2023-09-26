namespace ClassFramework.CodeGeneration.Models;

public interface IClassMethod : IMetadataContainer, IExtendedVisibilityContainer, INameContainer, IAttributesContainer, ICodeStatementsContainer, IParametersContainer, ITypeContainer, IExplicitInterfaceNameContainer, IParentTypeContainer
{
    bool Partial { get; }
    bool ExtensionMethod { get; }
    bool Operator { get; }
    bool Async { get; }
    [Required] IReadOnlyCollection<string> GenericTypeArguments { get; }
    [Required] IReadOnlyCollection<string> GenericTypeArgumentConstraints { get; }
}
