namespace ClassFramework.CodeGeneration.Models;

internal interface IConstructor : IMetadataContainer, IExtendedVisibilityContainer, IAttributesContainer, ICodeStatementsContainer, IParametersContainer
{
    string? ChainCall { get; }
}
