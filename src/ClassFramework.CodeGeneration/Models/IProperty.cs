namespace ClassFramework.CodeGeneration.Models;

internal interface IProperty : IMetadataContainer, IExtendedVisibilityContainer, INameContainer, IAttributesContainer, ITypeContainer, IExplicitInterfaceNameContainer, IParentTypeContainer
{
    bool HasGetter { get; }
    bool HasSetter { get; }
    bool HasInitializer { get; }
    Visibility? GetterVisibility { get; }
    Visibility? SetterVisibility { get; }
    Visibility? InitializerVisibility { get; }
    [Required] IReadOnlyCollection<ICodeStatementBase> GetterCodeStatements { get; }
    [Required] IReadOnlyCollection<ICodeStatementBase> SetterCodeStatements { get; }
    [Required] IReadOnlyCollection<ICodeStatementBase> InitializerCodeStatements { get; }
}
