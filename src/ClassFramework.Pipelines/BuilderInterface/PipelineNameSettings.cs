﻿namespace ClassFramework.Pipelines.BuilderInterface;

public class PipelineNameSettings
{
    public string SetMethodNameFormatString { get; }
    public string AddMethodNameFormatString { get; }
    public string BuilderNamespaceFormatString { get; }
    public string BuilderNameFormatString { get; }
    public string BuilderExtensionsNamespaceFormatString { get; }
    public string BuilderExtensionsNameFormatString { get; }

    public PipelineNameSettings(string setMethodNameFormatString = "With{Name}",
                                string addMethodNameFormatString = "Add{Name}",
                                string builderNamespaceFormatString = "{Namespace}.Builders",
                                string builderNameFormatString = "{Class.Name}Builder",
                                string builderExtensionsNamespaceFormatString = "{Namespace}.Builders.Extensions",
                                string builderExtensionsNameFormatString = "{Class.NameNoInterfacePrefix}BuilderExtensions")
    {
        SetMethodNameFormatString = setMethodNameFormatString.IsNotNull(nameof(setMethodNameFormatString));
        AddMethodNameFormatString = addMethodNameFormatString.IsNotNull(nameof(addMethodNameFormatString));
        BuilderNamespaceFormatString = builderNamespaceFormatString.IsNotNull(nameof(builderNamespaceFormatString));
        BuilderNameFormatString = builderNameFormatString.IsNotNull(nameof(builderNameFormatString));
        BuilderExtensionsNamespaceFormatString = builderExtensionsNamespaceFormatString.IsNotNull(nameof(builderExtensionsNamespaceFormatString));
        BuilderExtensionsNameFormatString = builderExtensionsNameFormatString.IsNotNull(nameof(builderExtensionsNameFormatString));
    }
}