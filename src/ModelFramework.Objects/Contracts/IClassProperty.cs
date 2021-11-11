using ModelFramework.Common.Contracts;
using System.Collections.Generic;

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
        IReadOnlyCollection<ICodeStatement> GetterCodeStatements { get; }
        IReadOnlyCollection<ICodeStatement> SetterCodeStatements { get; }
        IReadOnlyCollection<ICodeStatement> InitCodeStatements { get; }
    }
}
