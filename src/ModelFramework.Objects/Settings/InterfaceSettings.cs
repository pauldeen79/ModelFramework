namespace ModelFramework.Objects.Settings;

public record InterfaceSettings
{
    public Func<IClassProperty, bool>? PropertyFilter { get; }
    public Func<IClassMethod, bool>? MethodFilter { get; }
    public Func<IMetadata, bool>? MetadataFilter { get; }
    public Func<IAttribute, bool>? AttributeFilter { get; }
    public IDictionary<string, string>? ApplyGenericTypes { get; }
    public Func<System.Attribute, AttributeBuilder>? AttributeInitializeDelegate { get; }
    public bool ChangePropertiesToReadOnly { get; }

    public InterfaceSettings(Func<IClassProperty, bool>? propertyFilter = null,
                             Func<IClassMethod, bool>? methodFilter = null,
                             Func<IMetadata, bool>? metadataFilter = null,
                             Func<IAttribute, bool>? attributeFilter = null,
                             Func<System.Attribute, AttributeBuilder>? attributeInitializeDelegate = null,
                             IDictionary<string, string>? applyGenericTypes = null,
                             bool changePropertiesToReadOnly = false)
    {
        PropertyFilter = propertyFilter;
        MethodFilter = methodFilter;
        MetadataFilter = metadataFilter;
        AttributeFilter = attributeFilter;
        AttributeInitializeDelegate = attributeInitializeDelegate;
        ApplyGenericTypes = applyGenericTypes;
        ChangePropertiesToReadOnly = changePropertiesToReadOnly;
    }
}
