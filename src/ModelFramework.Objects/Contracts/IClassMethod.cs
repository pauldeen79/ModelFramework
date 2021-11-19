﻿using ModelFramework.Common.Contracts;

namespace ModelFramework.Objects.Contracts
{
    public interface IClassMethod : IMetadataContainer, IExtendedVisibilityContainer, INameContainer, IAttributesContainer, ICodeStatementsContainer, IParametersContainer, ITypeContainer, IExplicitInterfaceNameContainer
    {
        bool Partial { get; }
        bool ExtensionMethod { get; }
        bool Operator { get; }
    }
}
