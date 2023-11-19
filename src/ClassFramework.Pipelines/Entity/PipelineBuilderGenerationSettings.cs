﻿namespace ClassFramework.Pipelines.Entity;

public record PipelineBuilderGenerationSettings
{
    public bool AddSetters { get; }
    public Visibility? SetterVisibility { get; }
    public bool AddNullChecks { get; }
    public bool UseExceptionThrowIfNull { get; }
    public bool AllowGenerationWithoutProperties { get; }
    public Predicate<Domain.Attribute>? CopyAttributePredicate { get; }
    public Predicate<string>? CopyInterfacePredicate { get; }

    public PipelineBuilderGenerationSettings(
        bool addSetters = false,
        Visibility? setterVisibility = null,
        bool addNullChecks = false,
        bool useExceptionThrowIfNull = false,
        bool allowGenerationWithoutProperties = false,
        Predicate<Domain.Attribute>? copyAttributePredicate = null,
        Predicate<string>? copyInterfacePredicate = null)
    {
        AddSetters = addSetters;
        SetterVisibility = setterVisibility;
        AddNullChecks = addNullChecks;
        UseExceptionThrowIfNull = useExceptionThrowIfNull;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        CopyAttributePredicate = copyAttributePredicate;
        CopyInterfacePredicate = copyInterfacePredicate;
    }
}
