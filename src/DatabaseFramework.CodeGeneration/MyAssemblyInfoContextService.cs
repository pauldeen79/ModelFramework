﻿namespace DatabaseFramework.CodeGeneration;

[ExcludeFromCodeCoverage]
public class MyAssemblyInfoContextService : IAssemblyInfoContextService
{
    public string[] GetExcludedAssemblies() =>
    [
        "System.Runtime",
        "System.Collections",
        "System.ComponentModel",
        "TemplateFramework.Abstractions",
        "TemplateFramework.Console",
        "TemplateFramework.Core",
        "TemplateFramework.Core.CodeGeneration",
        "TemplateFramework.Runtime",
        "TemplateFramework.TemplateProviders.ChildTemplateProvider",
        "TemplateFramework.TemplateProviders.CompiledTemplateProvider",
        "TemplateFramework.TemplateProviders.StringTemplateProvider",
        "CrossCutting.Common",
        "CrossCutting.ProcessingPipeline",
        "CrossCutting.Utilities.Aggregators",
        "CrossCutting.Utilities.Operators",
        "CrossCutting.Utilities.Parsers",
        "Microsoft.Extensions.DependencyInjection",
        "Microsoft.Extensions.DependencyInjection.Abstractions",
        "ClassFramework.CodeGeneration",
        "ClassFramework.CsharpExpressionCreator",
        "ClassFramework.Domain",
        "ClassFramework.Pipelines",
        "ClassFramework.TemplateFramework",
        "CsharpExpressionDumper.Abstractions",
        "CsharpExpressionDumper.Core",
        "DatabaseFramework.CodeGeneration"
    ];
}
