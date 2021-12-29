namespace ModelFramework.Objects.Settings
{
    public class ImmutableBuilderClassConstructorSettings
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
