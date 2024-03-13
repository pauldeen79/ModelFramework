namespace DatabaseFramework.CodeGeneration.Models.SqlStatements;

internal interface IStringSqlStatement : ISqlStatementBase
{
    [Required] string Statement { get; set; }
}
