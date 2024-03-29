﻿namespace ModelFramework.Objects.Contracts;

public interface ICodeStatementsContainer
{
    IReadOnlyCollection<ICodeStatement> CodeStatements { get; }
}
