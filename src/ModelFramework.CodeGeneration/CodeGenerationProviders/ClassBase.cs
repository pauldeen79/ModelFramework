namespace ModelFramework.CodeGeneration.CodeGenerationProviders;

public abstract class ClassBase : ICodeGenerationProvider
{
    public bool GenerateMultipleFiles { get; private set; }
    public bool SkipWhenFileExists { get; private set; }
    public string BasePath { get; private set; } = string.Empty;

    public abstract string Path { get; }
    public abstract string DefaultFileName { get; }
    public abstract bool RecurseOnDeleteGeneratedFiles { get; }
    public abstract object CreateModel();

    public virtual string LastGeneratedFilesFileName => $"*{FileNameSuffix}.cs";
    public virtual Action? AdditionalActionDelegate => null;

    protected abstract bool EnableNullableContext { get; }
    protected abstract bool CreateCodeGenerationHeader { get; }
    protected virtual string FileNameSuffix => ".template.generated";
    protected virtual bool UseCustomInitializersOnAttributeBuilder => false;

    protected virtual AttributeBuilder? AttributeInitializeDelegate(Attribute sourceAttribute) => AttributeBuilder.DefaultInitializer(sourceAttribute);

    public virtual void Initialize(bool generateMultipleFiles, bool skipWhenFileExists, string basePath)
    {
        GenerateMultipleFiles = generateMultipleFiles;
        SkipWhenFileExists = skipWhenFileExists;
        BasePath = basePath;
    }

    public object CreateAdditionalParameters()
        => new Dictionary<string, object>
        {
            { nameof(CSharpClassGenerator.EnableNullableContext), EnableNullableContext },
            { nameof(CSharpClassGenerator.CreateCodeGenerationHeader), CreateCodeGenerationHeader },
            { nameof(CSharpClassGenerator.GenerateMultipleFiles), GenerateMultipleFiles },
            { nameof(CSharpClassGenerator.SkipWhenFileExists), SkipWhenFileExists },
            { nameof(CSharpClassGenerator.FileNamePrefix), FileNamePrefix },
            { nameof(CSharpClassGenerator.FileNameSuffix), FileNameSuffix }
        };

    public object CreateGenerator()
        => new CSharpClassGenerator();

    private string FileNamePrefix => string.IsNullOrEmpty(Path)
        ? string.Empty
        : Path + "/";
}
