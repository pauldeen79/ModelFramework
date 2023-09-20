namespace ClassFramework.CodeGeneration.Models.Abstractions;

public interface ICodeStatementsContainer
{
    IReadOnlyCollection<ICodeStatement> CodeStatements { get; }
}
