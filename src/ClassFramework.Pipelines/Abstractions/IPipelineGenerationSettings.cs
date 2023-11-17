namespace ClassFramework.Pipelines.Abstractions;

public interface IPipelineGenerationSettings
{
    bool EnableNullableReferenceTypes { get; }
    bool AddNullChecks { get; }
    bool EnableInheritance { get; }
    ArgumentValidationType ValidateArguments { get; }
    Func<IParentTypeContainer, TypeBase, bool>? InheritanceComparisonDelegate { get; }
}
