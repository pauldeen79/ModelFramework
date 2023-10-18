namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderGenerationSettings
{
    public bool AddNullChecks { get; }
    public bool EnableNullableReferenceTypes { get; }
    public bool CopyAttributes { get; }

    public PipelineBuilderGenerationSettings(
        bool addNullChecks = false,
        bool enableNullableReferenceTypes = false,
        bool copyAttributes = false)
    {
        AddNullChecks = addNullChecks;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        CopyAttributes = copyAttributes;
    }
}
