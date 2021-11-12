﻿using CrossCutting.Common;

namespace ModelFramework.Objects.Contracts
{
    public interface ICodeStatementsContainer
    {
        ValueCollection<ICodeStatement> CodeStatements { get; }
    }
}
