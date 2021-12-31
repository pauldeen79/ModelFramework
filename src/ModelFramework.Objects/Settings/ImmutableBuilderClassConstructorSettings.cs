namespace ModelFramework.Objects.Settings
{
    public record ImmutableBuilderClassConstructorSettings
    {
        public bool AddCopyConstructor { get; }
        public bool AddConstructorWithAllProperties { get; }

        public ImmutableBuilderClassConstructorSettings(bool addCopyConstructor = false, bool addConstructorWithAllProperties = false)
        {
            AddCopyConstructor = addCopyConstructor;
            AddConstructorWithAllProperties = addConstructorWithAllProperties;
        }
    }
}
