namespace ModelFramework.Objects.Settings
{
    public class ClassSettings
    {
        public string Name { get; }
        public string Namespace { get; }
        public bool Partial { get; }
        public bool CreateConstructors { get; }
        public bool AutoGenerateInterface { get; }
        public InterfaceSettings AutoGenerateInterfaceSettings { get; }
        public bool? Record { get; }

        public ClassSettings(string name = null,
                             string @namespace = null,
                             bool partial = false,
                             bool createConstructors = false,
                             bool autoGenerateInterface = false,
                             InterfaceSettings autoGenerateInterfaceSettings = null,
                             bool? record = null)
        {
            Name = name;
            Namespace = @namespace;
            Partial = partial;
            CreateConstructors = createConstructors;
            AutoGenerateInterface = autoGenerateInterface;
            AutoGenerateInterfaceSettings = autoGenerateInterfaceSettings;
            Record = record;
        }
    }
}
