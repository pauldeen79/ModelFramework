namespace ModelFramework.Objects.Settings;

public record ImmutableBuilderClassGenerationSettings
{
    public bool UseLazyInitialization { get; }
    public bool CopyPropertyCode { get; }
    public bool AllowGenerationWithoutProperties { get; }
    public bool CopyFields { get; }
    public bool CopyAttributes { get; }

    public ImmutableBuilderClassGenerationSettings(bool useLazyInitialization = false,
                                                   bool copyPropertyCode = true,
                                                   bool allowGenerationWithoutProperties = false,
                                                   bool copyFields = true,
                                                   bool copyAttributes = true)
    {
        UseLazyInitialization = useLazyInitialization;
        CopyPropertyCode = copyPropertyCode;
        AllowGenerationWithoutProperties = allowGenerationWithoutProperties;
        CopyFields = copyFields;
        CopyAttributes = copyAttributes;
    }
}
