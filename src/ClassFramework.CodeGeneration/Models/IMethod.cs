namespace ClassFramework.CodeGeneration.Models;

internal interface IMethod : IMetadataContainer, IExtendedVisibilityContainer, INameContainer, IAttributesContainer, ICodeStatementsContainer, IParametersContainer, ITypeContainer, IExplicitInterfaceNameContainer, IParentTypeContainer, IGenericTypeArgumentsContainer
{
    bool Partial { get; }
    bool ExtensionMethod { get; }
    bool Operator { get; }
    bool Async { get; }
}
