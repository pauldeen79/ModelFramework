using CrossCutting.Common;
using ModelFramework.Common.Contracts;

namespace ModelFramework.Objects.Contracts
{
    public interface IClassProperty : IMetadataContainer, IExtendedVisibilityContainer, INameContainer, IAttributesContainer, ITypeContainer, IExplicitInterfaceNameContainer
    {
        bool HasGetter { get; }
        bool HasSetter { get; }
        bool HasInit { get; }
        Visibility GetterVisibility { get; }
        Visibility SetterVisibility { get; }
        Visibility InitVisibility { get; }
        string GetterBody { get; }
        string SetterBody { get; }
        string InitBody { get; }
        ValueCollection<ICodeStatement> GetterCodeStatements { get; }
        ValueCollection<ICodeStatement> SetterCodeStatements { get; }
        ValueCollection<ICodeStatement> InitCodeStatements { get; }
    }
}
