namespace ClassFramework.Pipelines;

public record BuilderPipelineBuilderTypeSettings
{
    public Func<TypeBaseBuilder, bool, string>? FormatInstanceTypeNameDelegate { get; }
}
