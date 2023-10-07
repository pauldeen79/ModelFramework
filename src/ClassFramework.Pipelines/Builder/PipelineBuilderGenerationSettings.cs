namespace ClassFramework.Pipelines.Builder;

public record PipelineBuilderGenerationSettings
{
    public bool AddNullChecks { get; }
    public bool EnableNullableReferenceTypes { get; }
    public bool CopyFields { get; }
    public bool CopyAttributes { get; }

    public PipelineBuilderGenerationSettings(
        bool addNullChecks = false,
        bool enableNullableReferenceTypes = false,
        bool copyFields = true,
        bool copyAttributes = true)
    {
        AddNullChecks = addNullChecks;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        CopyFields = copyFields;
        CopyAttributes = copyAttributes;
    }
}
