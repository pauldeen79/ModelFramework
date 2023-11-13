namespace ClassFramework.Pipelines.Abstractions;

public interface IPropertyGenerationSettings
{
    bool EnableNullableReferenceTypes { get; }
    bool AddNullChecks { get; }
    ArgumentValidationType ValidateArguments { get; }
}
