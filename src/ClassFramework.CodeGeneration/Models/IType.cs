﻿namespace ClassFramework.CodeGeneration.Models;

public interface IType : IMetadataContainer, IVisibilityContainer, INameContainer, IAttributesContainer
{
    [Required] string Namespace { get; }
    bool Partial { get; }
    [Required] IReadOnlyCollection<string> Interfaces { get; }
    [Required] IReadOnlyCollection<IClassProperty> Properties { get; }
    [Required] IReadOnlyCollection<IClassMethod> Methods { get; }
    [Required] IReadOnlyCollection<string> GenericTypeArguments { get; }
    [Required] IReadOnlyCollection<string> GenericTypeArgumentConstraints { get; }
    [Required] IReadOnlyCollection<string> SuppressWarningCodes { get; }
}
