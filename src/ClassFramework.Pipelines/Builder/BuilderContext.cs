namespace ClassFramework.Pipelines.Builder;

public class BuilderContext : ContextBase<IType, PipelineSettings>
{
    public BuilderContext(IType sourceModel, PipelineSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }

    public IEnumerable<Property> GetSourceProperties()
        => SourceModel.Properties.Where(x => SourceModel.IsMemberValidForBuilderClass(x, Settings));

    public bool IsBuilderForAbstractEntity => Settings.EntitySettings.InheritanceSettings.EnableInheritance && (Settings.InheritanceSettings.BaseClass is null || Settings.InheritanceSettings.IsAbstract);
    public bool IsBuilderForOverrideEntity => Settings.EntitySettings.InheritanceSettings.EnableInheritance && Settings.InheritanceSettings.BaseClass is not null;
    public bool IsAbstractBuilder => Settings.InheritanceSettings.EnableBuilderInheritance && (Settings.InheritanceSettings.BaseClass is null || Settings.InheritanceSettings.IsAbstract) && !Settings.IsForAbstractBuilder;

    public string[] CreatePragmaWarningDisableStatements()
        => NeedsPragmas()
            ?
            [
                "#pragma warning disable CS8604 // Possible null reference argument.",
                "#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.",
            ]
            : Array.Empty<string>();

    public string[] CreatePragmaWarningRestoreStatements()
        => NeedsPragmas()
            ?
            [
                "#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.",
                "#pragma warning restore CS8604 // Possible null reference argument.",
            ]
            : Array.Empty<string>();

    public bool HasBackingFields()
        => !(IsAbstractBuilder
        || !Settings.EntitySettings.NullCheckSettings.AddNullChecks
        || Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared)
        || Settings.EntitySettings.GenerationSettings.AddBackingFields;

    private bool NeedsPragmas()
        => Settings.TypeSettings.EnableNullableReferenceTypes
        && !IsBuilderForAbstractEntity
        && !Settings.EntitySettings.NullCheckSettings.AddNullChecks;

    public bool IsValidForFluentMethod(Property property)
    {
        property = property.IsNotNull(nameof(property));

        if (!Settings.EntitySettings.CopySettings.CopyInterfaces)
        {
            return true;
        }

        if (string.IsNullOrEmpty(property.ParentTypeFullName))
        {
            return true;
        }

        if (property.ParentTypeFullName == SourceModel.GetFullName())
        {
            return true;
        }

        var isInterfaced = SourceModel.Interfaces.Any(x => x == property.ParentTypeFullName);

        return !isInterfaced;
    }
}
