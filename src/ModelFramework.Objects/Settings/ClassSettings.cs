namespace ModelFramework.Objects.Settings
{
    public class ClassSettings
    {
        public bool Partial { get; }
        public bool CreateConstructors { get; }

        public ClassSettings(bool partial = false,
                             bool createConstructors = false)
        {
            Partial = partial;
            CreateConstructors = createConstructors;
        }
    }
}
