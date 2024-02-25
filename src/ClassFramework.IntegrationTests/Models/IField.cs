﻿namespace ClassFramework.IntegrationTests.Models;

internal interface IField : Abstractions.IMetadataContainer, Abstractions.IExtendedVisibilityContainer, Abstractions.INameContainer, Abstractions.IAttributesContainer, Abstractions.ITypeContainer, Abstractions.IDefaultValueContainer, Abstractions.IParentTypeContainer
{
    bool ReadOnly { get; }
    bool Constant { get; }
    bool Event { get; }
}
