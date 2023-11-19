namespace ClassFramework.Pipelines.Entity;

public record PipelineBuilderGenerationSettings
{
    public bool AddSetters { get; }
    public Visibility? SetterVisibility { get; }
    public bool AllowGenerationWithoutProperties { get; }
    public Predicate<Domain.Attribute>? CopyAttributePredicate { get; }
    public Predicate<string>? CopyInterfacePredicate { get; }

    public PipelineBuilderGenerationSettings(
        bool addSetters = false,
        Visibility? setterVisibility = null,
        bool allowGenerationWithoutProperties = false,
        Predicate<Domain.Attribute>? copyAttributePredicate = null,
        Predicate<string>? copyInterfacePredicate = null)
    {
        AddSetters = addSetters;
        SetterVisibility = setterVisibility;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        CopyAttributePredicate = copyAttributePredicate;
        CopyInterfacePredicate = copyInterfacePredicate;
    }
}
