namespace DatabaseFramework.CodeGeneration.Models;

internal interface IStoredProcedure : ISchemaContainer, INameContainer, IMetadataContainer
{
    [Required] IReadOnlyCollection<ISqlStatementBase> Statements { get; }
    [Required] IReadOnlyCollection<IStoredProcedureParameter> Parameters { get; }
}
