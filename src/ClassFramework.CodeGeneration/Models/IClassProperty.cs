namespace ClassFramework.CodeGeneration.Models;

internal interface IClassProperty : IMetadataContainer, IExtendedVisibilityContainer, INameContainer, IAttributesContainer, ITypeContainer, IExplicitInterfaceNameContainer, IParentTypeContainer
{
    bool HasGetter { get; }
    bool HasSetter { get; }
    bool HasInitializer { get; }
    Visibility? GetterVisibility { get; }
    Visibility? SetterVisibility { get; }
    Visibility? InitializerVisibility { get; }
    [Required] IReadOnlyCollection<ICodeStatement> GetterCodeStatements { get; }
    [Required] IReadOnlyCollection<ICodeStatement> SetterCodeStatements { get; }
    [Required] IReadOnlyCollection<ICodeStatement> InitializerCodeStatements { get; }
}
