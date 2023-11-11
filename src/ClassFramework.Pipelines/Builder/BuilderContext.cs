namespace ClassFramework.Pipelines.Builder;

public record BuilderContext : ContextBase<TypeBase, PipelineBuilderSettings>
{
    public BuilderContext(TypeBase sourceModel, PipelineBuilderSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }

    public bool IsBuilderForAbstractEntity => Settings.EntitySettings.InheritanceSettings.EnableInheritance && (Settings.InheritanceSettings.BaseClass is null || Settings.InheritanceSettings.IsAbstract);
    public bool IsBuilderForOverrideEntity => Settings.EntitySettings.InheritanceSettings.EnableInheritance && Settings.InheritanceSettings.BaseClass is not null;
    public bool IsAbstractBuilder => Settings.InheritanceSettings.EnableBuilderInheritance && (Settings.InheritanceSettings.BaseClass is null || Settings.InheritanceSettings.IsAbstract) && !Settings.IsForAbstractBuilder;

    public string[] CreatePragmaWarningDisableStatements()
        => NeedsPragmas()
            ? new[]
            {
                "#pragma warning disable CS8604 // Possible null reference argument.",
                "#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.",
            }
            : Array.Empty<string>();

    public string[] CreatePragmaWarningRestoreStatements()
        => NeedsPragmas()
            ? new[]
            {
                "#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.",
                "#pragma warning restore CS8604 // Possible null reference argument.",
            }
            : Array.Empty<string>();

    public string MapTypeName(string typeName)
        => typeName;

    public Domain.Attribute MapAttribute(Domain.Attribute attribute)
        => attribute;

    private bool NeedsPragmas()
        => Settings.GenerationSettings.EnableNullableReferenceTypes
        && !IsBuilderForAbstractEntity
        && !Settings.GenerationSettings.AddNullChecks;
}
