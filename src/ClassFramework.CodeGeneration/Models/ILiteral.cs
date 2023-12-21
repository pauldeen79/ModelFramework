namespace ClassFramework.CodeGeneration.Models;

internal interface ILiteral
{
    string? Value { get; }
    object? OriginalValue { get; }
}
