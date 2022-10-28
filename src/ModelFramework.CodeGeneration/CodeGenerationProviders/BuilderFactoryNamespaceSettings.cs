namespace ModelFramework.CodeGeneration.CodeGenerationProviders;

public record BuilderFactoryNamespaceSettings
{
    public BuilderFactoryNamespaceSettings(
        string classNamespace,
        string className,
        string classTypeName,
        string builderNamespace,
        string builderTypeName,
        string overrideClassNamespace)
    {
        ClassNamespace = classNamespace;
        ClassName = className;
        ClassTypeName = classTypeName;
        BuilderNamespace = builderNamespace;
        BuilderTypeName = builderTypeName;
        OverrideClassNamespace = overrideClassNamespace;
    }

    public string ClassNamespace { get; }
    public string ClassName { get; }
    public string ClassTypeName { get; }
    public string BuilderNamespace { get; }
    public string BuilderTypeName { get; }
    public string OverrideClassNamespace { get; }
}
