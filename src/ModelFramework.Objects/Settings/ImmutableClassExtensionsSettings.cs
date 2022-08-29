namespace ModelFramework.Objects.Settings;

public record ImmutableClassExtensionsSettings
{
    public string NewCollectionTypeName { get; }
    public bool AllowGenerationWithoutProperties { get; }

    public ImmutableClassExtensionsSettings(
        string newCollectionTypeName = "System.Collections.Immutable.IImmutableList",
        bool allowGenerationWithoutProperties = false)
    {
        NewCollectionTypeName = newCollectionTypeName;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
    }
}
