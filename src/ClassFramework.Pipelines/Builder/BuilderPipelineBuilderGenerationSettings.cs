namespace ClassFramework.Pipelines.Builder;

public record BuilderPipelineBuilderGenerationSettings
{
    public bool AddNullChecks { get; }
    public bool EnableNullableReferenceTypes { get; }
    public bool CopyFields { get; }
    public bool CopyAttributes { get; }

    public BuilderPipelineBuilderGenerationSettings(
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
