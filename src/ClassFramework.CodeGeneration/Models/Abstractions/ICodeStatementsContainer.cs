namespace ClassFramework.CodeGeneration.Models.Abstractions;

public interface ICodeStatementsContainer
{
    [Required] IReadOnlyCollection<ICodeStatement> CodeStatements { get; }
}
