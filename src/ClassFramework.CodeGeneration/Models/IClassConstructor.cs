namespace ClassFramework.CodeGeneration.Models;

public interface IClassConstructor : IMetadataContainer, IExtendedVisibilityContainer, IAttributesContainer, ICodeStatementsContainer, IParametersContainer
{
    string? ChainCall { get; }
}
