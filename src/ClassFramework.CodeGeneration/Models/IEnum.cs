﻿namespace ClassFramework.CodeGeneration.Models;

public interface IEnum : IAttributesContainer, IMetadataContainer, INameContainer, IVisibilityContainer
{
    [Required] IReadOnlyCollection<IEnumMember> Members { get; }
}
