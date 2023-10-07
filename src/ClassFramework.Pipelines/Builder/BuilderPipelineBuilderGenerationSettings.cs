namespace ClassFramework.Pipelines.Builder;

public record BuilderPipelineBuilderGenerationSettings
{
    public bool CopyFields { get; }
    public bool CopyAttributes { get; }

    public BuilderPipelineBuilderGenerationSettings(
        bool copyFields = true,
        bool copyAttributes = true)
    {
        CopyFields = copyFields;
        CopyAttributes = copyAttributes;
    }
}
