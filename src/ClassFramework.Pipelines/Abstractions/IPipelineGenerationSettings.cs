namespace ClassFramework.Pipelines.Abstractions;

public interface IPipelineGenerationSettings
{
    bool EnableNullableReferenceTypes { get; }
    bool AddNullChecks { get; }
    bool EnableInheritance { get; }
    bool AddBackingFields { get; }
    string CollectionTypeName { get; }
    ArgumentValidationType ValidateArguments { get; }
    Func<IParentTypeContainer, TypeBase, bool>? InheritanceComparisonDelegate { get; }
}
