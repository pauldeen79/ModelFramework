using ModelFramework.Common.Contracts;

namespace ModelFramework.Objects.Contracts
{
    public interface IClassConstructor : IMetadataContainer, IExtendedVisibilityContainer, IAttributesContainer, IBodyContainer, ICodeStatementsContainer, IParametersContainer
    {
        string ChainCall { get; }
    }
}
