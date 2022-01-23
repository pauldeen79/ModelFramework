namespace ModelFramework.Objects.Settings
{
    public record ImmutableClassExtensionsSettings
    {
        public string NewCollectionTypeName { get; }

        public ImmutableClassExtensionsSettings(string newCollectionTypeName = "System.Collections.Immutable.IImmutableList")
        {
            NewCollectionTypeName = newCollectionTypeName;
        }
    }
}
