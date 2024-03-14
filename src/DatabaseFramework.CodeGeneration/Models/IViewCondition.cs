﻿namespace DatabaseFramework.CodeGeneration.Models;

internal interface IViewCondition : IMetadataContainer, IFileGroupNameContainer
{
    [Required] string Expression { get; }
    [Required(AllowEmptyStrings = true)] string Combination { get; } // required on first condition!
}