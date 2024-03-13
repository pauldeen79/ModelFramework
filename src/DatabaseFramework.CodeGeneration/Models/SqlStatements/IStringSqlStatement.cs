namespace DatabaseFramework.CodeGeneration.Models.SqlStatements;

internal interface IStringSqlStatement : ISqlStatement
{
    [Required] string Statement { get; set; }
}
