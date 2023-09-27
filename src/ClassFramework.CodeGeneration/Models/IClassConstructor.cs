namespace ClassFramework.CodeGeneration.Models;

internal interface IClassConstructor : IMetadataContainer, IExtendedVisibilityContainer, IAttributesContainer, ICodeStatementsContainer, IParametersContainer
{
    string? ChainCall { get; }
}
