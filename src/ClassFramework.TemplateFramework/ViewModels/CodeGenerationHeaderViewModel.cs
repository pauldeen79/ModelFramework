﻿namespace ClassFramework.TemplateFramework.ViewModels;

public class CodeGenerationHeaderViewModel : CsharpClassGeneratorViewModelBase<CodeGenerationHeaderModel>
{
    public CodeGenerationHeaderViewModel(ICsharpExpressionCreator csharpExpressionCreator) : base(csharpExpressionCreator)
    {
    }

    public string Version
        => !string.IsNullOrEmpty(GetModel().EnvironmentVersion)
            ? Model!.EnvironmentVersion!
            : Environment.Version.ToString();

    public bool CreateCodeGenerationHeader => GetModel().CreateCodeGenerationHeader;
}
