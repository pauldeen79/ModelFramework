namespace ClassFramework.CodeGeneration.Models.CodeStatements;

public interface ILiteralCodeStatement : ICodeStatement
{
    [Required] string Statement { get; set; }
}
