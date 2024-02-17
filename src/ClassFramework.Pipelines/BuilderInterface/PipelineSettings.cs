namespace ClassFramework.Pipelines.BuilderInterface;

public sealed class PipelineSettings : IBuilderPipelineGenerationSettings
{
    public PipelineNameSettings NameSettings { get; }
    public PipelineInheritanceSettings InheritanceSettings { get; }
    public IPipelineBuilderTypeSettings TypeSettings { get; }
    public Entity.PipelineSettings EntitySettings { get; }

    public bool EnableNullableReferenceTypes => TypeSettings.EnableNullableReferenceTypes;
    public bool AddNullChecks => EntitySettings.NullCheckSettings.AddNullChecks;
    public bool UseExceptionThrowIfNull => EntitySettings.NullCheckSettings.UseExceptionThrowIfNull;
    public bool EnableInheritance => EntitySettings.InheritanceSettings.EnableInheritance;
    public bool AddBackingFields => EntitySettings.GenerationSettings.AddBackingFields;
    public string CollectionTypeName => TypeSettings.NewCollectionTypeName;
    public string NamespaceFormatString => NameSettings.BuilderExtensionsNamespaceFormatString;
    public string NameFormatString => NameSettings.BuilderExtensionsNameFormatString;
    public ArgumentValidationType ValidateArguments => EntitySettings.ConstructorSettings.OriginalValidateArguments;
    public Func<IParentTypeContainer, IType, bool>? InheritanceComparisonDelegate => InheritanceSettings.InheritanceComparisonDelegate;

    public PipelineSettings(
        PipelineNameSettings? nameSettings,
        PipelineInheritanceSettings? inheritanceSettings,
        PipelineTypeSettings? typeSettings,
        Entity.PipelineSettings? entitySettings)
    {
        NameSettings = nameSettings ?? new();
        InheritanceSettings = inheritanceSettings ?? new();
        TypeSettings = typeSettings ?? new();
        EntitySettings = entitySettings ?? new();
    }
}
