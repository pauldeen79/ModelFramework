namespace ClassFramework.CodeGeneration.Models.CodeStatements;

internal interface ILiteralCodeStatement : ICodeStatement
{
    [Required] string Statement { get; set; }
}
