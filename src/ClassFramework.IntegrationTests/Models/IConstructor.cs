namespace ClassFramework.IntegrationTests.Models;

internal interface IConstructor : IMetadataContainer, IExtendedVisibilityContainer, IAttributesContainer, ICodeStatementsContainer, IParametersContainer
{
    [Required(AllowEmptyStrings = true)] string ChainCall { get; }
}
