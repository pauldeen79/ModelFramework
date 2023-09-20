namespace ClassFramework.CodeGeneration.Models;

public interface IClassMethod : IMetadataContainer, IExtendedVisibilityContainer, INameContainer, IAttributesContainer, ICodeStatementsContainer, IParametersContainer, ITypeContainer, IExplicitInterfaceNameContainer, IParentTypeContainer
{
    bool Partial { get; }
    bool ExtensionMethod { get; }
    bool Operator { get; }
    bool Async { get; }
    IReadOnlyCollection<string> GenericTypeArguments { get; }
    IReadOnlyCollection<string> GenericTypeArgumentConstraints { get; }
}
