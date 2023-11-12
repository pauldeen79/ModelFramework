namespace ClassFramework.Pipelines.Abstractions;

public interface IPipelineBuilderGenerationSettings
{
    bool EnableNullableReferenceTypes { get; }
    bool AddNullChecks { get; }
}
