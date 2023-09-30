namespace ClassFramework.CodeGeneration.Models.CodeStatements;

internal interface IStringCodeStatement : ICodeStatement
{
    [Required] string Statement { get; set; }
}
