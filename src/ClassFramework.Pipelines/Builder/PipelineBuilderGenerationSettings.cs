namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderGenerationSettings : IPipelineBuilderGenerationSettings
{
    public bool AddNullChecks { get; }
    public bool EnableNullableReferenceTypes { get; }
    public bool CopyAttributes { get; }
    public bool CopyInterfaces { get; }
    public Predicate<Domain.Attribute>? CopyAttributePredicate { get; }
    public Predicate<string>? CopyInterfacePredicate { get; }

    public PipelineBuilderGenerationSettings(
        bool addNullChecks = false,
        bool enableNullableReferenceTypes = false,
        bool copyAttributes = false,
        bool copyInterfaces = false,
        Predicate<Domain.Attribute>? copyAttributePredicate = null,
        Predicate<string>? copyInterfacePredicate = null)
    {
        AddNullChecks = addNullChecks;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        CopyAttributes = copyAttributes;
        CopyInterfaces = copyInterfaces;
        CopyAttributePredicate = copyAttributePredicate;
        CopyInterfacePredicate = copyInterfacePredicate;
    }
}
