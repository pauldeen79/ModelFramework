namespace DatabaseFramework.CodeGeneration.Models;

internal interface IStoredProcedure : IDatabaseObject
{
    [Required] IReadOnlyCollection<ISqlStatementBase> Statements { get; }
    [Required] IReadOnlyCollection<IStoredProcedureParameter> Parameters { get; }
}
