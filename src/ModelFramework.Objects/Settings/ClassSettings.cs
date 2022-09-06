namespace ModelFramework.Objects.Settings;

public record ClassSettings
{
    public bool Partial { get; }
    public bool CreateConstructors { get; }
    public Func<System.Attribute, AttributeBuilder>? AttributeInitializeDelegate { get; }

    public ClassSettings(bool partial = false,
                         bool createConstructors = false,
                         Func<System.Attribute, AttributeBuilder>? attributeInitializeDelegate = null)
    {
        Partial = partial;
        CreateConstructors = createConstructors;
        AttributeInitializeDelegate = attributeInitializeDelegate;
    }
}
