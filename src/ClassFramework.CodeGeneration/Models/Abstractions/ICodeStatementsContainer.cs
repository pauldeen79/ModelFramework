namespace ClassFramework.CodeGeneration.Models.Abstractions;

internal interface ICodeStatementsContainer
{
    [Required] IReadOnlyCollection<ICodeStatement> CodeStatements { get; }
}
