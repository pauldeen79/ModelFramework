namespace ClassFramework.Pipelines.Builder;

public record BuilderContext : ContextBase<TypeBase, PipelineBuilderSettings>
{
    public BuilderContext(TypeBase model, PipelineBuilderSettings settings, IFormatProvider formatProvider)
        : base(model, settings, formatProvider)
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
        => typeName.MapTypeName(Settings.TypeSettings);

    public Domain.Attribute MapAttribute(Domain.Attribute attribute)
    {
        attribute = attribute.IsNotNull(nameof(attribute));

        return new AttributeBuilder(attribute)
            .WithName(MapTypeName(attribute.Name))
            .Build();
    }

    private bool NeedsPragmas()
        => Settings.GenerationSettings.EnableNullableReferenceTypes
        && !IsBuilderForAbstractEntity
        && !Settings.GenerationSettings.AddNullChecks;
}
