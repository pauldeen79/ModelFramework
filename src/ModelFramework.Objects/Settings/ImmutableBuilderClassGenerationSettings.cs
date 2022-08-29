namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassGenerationSettings
{
    public bool UseLazyInitialization { get; }
    public bool CopyPropertyCode { get; }
    public bool AllowGenerationWithoutProperties { get; }

    public ImmutableBuilderClassGenerationSettings(bool useLazyInitialization = false,
                                                   bool copyPropertyCode = true,
                                                   bool allowGenerationWithoutProperties = false)
    {
        UseLazyInitialization = useLazyInitialization;
        CopyPropertyCode = copyPropertyCode;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
    }
}
