﻿namespace ClassFramework.Pipelines.Entity;

public class PipelineTypeSettings : PipelineBuilderTypeSettingsBase
{
    public PipelineTypeSettings(
        string newCollectionTypeName = "System.Collections.Generic.List",
        bool enableNullableReferenceTypes = false,
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
        : base(newCollectionTypeName, enableNullableReferenceTypes, namespaceMappings, typenameMappings)
    {
    }
}
