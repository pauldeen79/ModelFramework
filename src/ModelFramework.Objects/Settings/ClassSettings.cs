namespace ModelFramework.Objects.Settings
{
    public class ClassSettings
    {
        public bool Partial { get; }
        public bool CreateConstructors { get; }
        public bool AutoGenerateInterface { get; }
        public InterfaceSettings AutoGenerateInterfaceSettings { get; }

        public ClassSettings(bool partial = false,
                             bool createConstructors = false,
                             bool autoGenerateInterface = false,
                             InterfaceSettings autoGenerateInterfaceSettings = null)
        {
            Partial = partial;
            CreateConstructors = createConstructors;
            AutoGenerateInterface = autoGenerateInterface;
            AutoGenerateInterfaceSettings = autoGenerateInterfaceSettings;
        }
    }
}
