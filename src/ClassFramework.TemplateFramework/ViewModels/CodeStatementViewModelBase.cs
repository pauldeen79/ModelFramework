﻿namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class CodeStatementViewModelBase<T> : CsharpClassGeneratorViewModelBase<T>
    where T : CodeStatementBase
{
    protected CodeStatementViewModelBase(ICsharpExpressionCreator csharpExpressionCreator) : base(csharpExpressionCreator)
    {
    }

    public int AdditionalIndents
    {
        get
        {
            var parentModel = GetParentModel();
            return parentModel switch
            {
                ClassProperty => 3,
                ClassMethod or ClassConstructor => 2,
                _ => throw new NotSupportedException($"Don't know how {parentModel?.GetType().FullName ?? "NULL"} should be indented")
            };
        }
    }
}
