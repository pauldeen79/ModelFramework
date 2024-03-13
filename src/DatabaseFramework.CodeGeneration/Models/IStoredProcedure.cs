namespace DatabaseFramework.CodeGeneration.Models;

internal interface IStoredProcedure : Abstractions.INameContainer, Abstractions.IMetadataContainer
{
    [Required] IReadOnlyCollection<ISqlStatementBase> Statements { get; }
    [Required] IReadOnlyCollection<IStoredProcedureParameter> Parameters { get; }
}
