﻿namespace ClassFramework.TemplateFramework.ViewModels;

public class UsingsViewModel : CsharpClassGeneratorViewModelBase<UsingsModel>
{
    public UsingsViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    private readonly static string[] DefaultUsings =
    [
        "System",
        "System.Collections.Generic",
        "System.Linq",
        "System.Text"
    ];

    public IEnumerable<string> Usings
        => DefaultUsings
            .Union(GetModel().Types.SelectMany(classItem => classItem.Metadata.GetStringValues(MetadataNames.CustomUsing)))
            .OrderBy(ns => ns)
            .Distinct();
}
