namespace ClassFramework.Pipelines.Abstractions;

public interface IPipelineGenerationSettings
{
    bool EnableNullableReferenceTypes { get; }
    bool AddNullChecks { get; }
    bool UseExceptionThrowIfNull { get; }
    bool EnableInheritance { get; }
    bool AddBackingFields { get; }
    string CollectionTypeName { get; }
    string NamespaceFormatString { get; }
    string NameFormatString { get; }

    ArgumentValidationType ValidateArguments { get; }
    Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate { get; }
    IPipelineBuilderTypeSettings TypeSettings { get; }
}
