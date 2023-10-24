namespace ClassFramework.Pipelines.Builder;

public record BuilderContext
{
    public BuilderContext(TypeBase sourceModel, PipelineBuilderSettings settings, IFormatProvider formatProvider)
    {
        SourceModel = sourceModel.IsNotNull(nameof(sourceModel));
        Settings = settings.IsNotNull(nameof(settings));
        FormatProvider = formatProvider.IsNotNull(nameof(formatProvider));
    }

    public TypeBase SourceModel { get; }
    public PipelineBuilderSettings Settings { get; }
    public IFormatProvider FormatProvider { get; }

    public bool IsBuilderForAbstractEntity => Settings.ClassSettings.InheritanceSettings.EnableInheritance && (Settings.InheritanceSettings.BaseClass is null || Settings.InheritanceSettings.IsAbstract);
    public bool IsBuilderForOverrideEntity => Settings.ClassSettings.InheritanceSettings.EnableInheritance && Settings.InheritanceSettings.BaseClass is not null;
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

    private bool NeedsPragmas()
        => Settings.GenerationSettings.EnableNullableReferenceTypes
        && !IsBuilderForAbstractEntity
        && !Settings.GenerationSettings.AddNullChecks;
}
