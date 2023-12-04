﻿namespace ClassFramework.TemplateFramework.ViewModels;

public class PropertyCodeBodyViewModel : CsharpClassGeneratorViewModelBase<PropertyCodeBodyModel>
{
    public PropertyCodeBodyViewModel(ICsharpExpressionCreator csharpExpressionCreator) : base(csharpExpressionCreator)
    {
    }

    public bool IsAvailable
        => GetModel().IsAvailable;

    public string Modifiers
        => GetModel().Modifiers;

    public string Verb
        => GetModel().Verb;

    public bool OmitCode
        => GetModel().OmitCode;

    public IEnumerable<CodeStatementBase> CodeStatementModels
        => GetModel().CodeStatementModels;
}