namespace ClassFramework.Pipelines;

public record BuilderPipelineBuilderGenerationSettings
{
    public bool UseLazyInitialization { get; }
    public bool CopyPropertyCode { get; }
    public bool CopyFields { get; }
    public bool CopyAttributes { get; }

    public BuilderPipelineBuilderGenerationSettings(
        bool useLazyInitialization = false,
        bool copyPropertyCode = true,
        bool copyFields = true,
        bool copyAttributes = true)
    {
        UseLazyInitialization = useLazyInitialization;
        CopyPropertyCode = copyPropertyCode;
        CopyFields = copyFields;
        CopyAttributes = copyAttributes;
    }
}
