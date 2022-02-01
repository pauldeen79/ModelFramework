namespace ModelFramework.Objects.Contracts;

public interface IClassConstructor : IMetadataContainer, IExtendedVisibilityContainer, IAttributesContainer, ICodeStatementsContainer, IParametersContainer
{
    string ChainCall { get; }
}
