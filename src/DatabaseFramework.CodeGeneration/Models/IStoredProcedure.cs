﻿namespace DatabaseFramework.CodeGeneration.Models;

internal interface IStoredProcedure : INameContainer, IMetadataContainer
{
    [Required] IReadOnlyCollection<ISqlStatementBase> Statements { get; }
    [Required] IReadOnlyCollection<IStoredProcedureParameter> Parameters { get; }
}
