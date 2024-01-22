﻿namespace ClassFramework.Pipelines.Interface;

public record PipelineGenerationSettings
{
    public bool AddSetters { get; }
    public bool AllowGenerationWithoutProperties { get; }

    public PipelineGenerationSettings(
        bool addSetters = false,
        bool allowGenerationWithoutProperties = false)
    {
        AddSetters = addSetters;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
    }
}