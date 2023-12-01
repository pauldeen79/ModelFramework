﻿namespace ClassFramework.TemplateFramework.ViewModels;

public class CodeStatementViewModel : CsharpClassGeneratorViewModel<CodeStatementBase>
{
    public CodeStatementViewModel(CodeStatementBase data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator) : base(data, settings, csharpExpressionCreator)
    {
    }
}
