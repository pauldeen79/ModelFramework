namespace ClassFramework.Pipelines;

public record BuilderPipelineBuilderTypeSettings
{
    public Func<TypeBuilder, bool, string>? FormatInstanceTypeNameDelegate { get; }
}
