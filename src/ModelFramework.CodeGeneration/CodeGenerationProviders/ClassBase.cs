namespace ModelFramework.CodeGeneration.CodeGenerationProviders;

public abstract class ClassBase : ICodeGenerationProvider
{
    public bool GenerateMultipleFiles { get; private set; }
    public string BasePath { get; private set; } = string.Empty;

    public abstract string Path { get; }
    public abstract string DefaultFileName { get; }
    public abstract bool RecurseOnDeleteGeneratedFiles { get; }
    public abstract object CreateModel();

    public virtual string LastGeneratedFilesFileName => $"*{FileNameSuffix}.cs";
    public virtual Action? AdditionalActionDelegate => null;

    protected abstract bool EnableNullableContext { get; }
    protected abstract bool CreateCodeGenerationHeader { get; }
    protected virtual string FileNameSuffix => ".generated";

    public void Initialize(bool generateMultipleFiles, string basePath)
    {
        GenerateMultipleFiles = generateMultipleFiles;
        BasePath = basePath;
    }

    public object CreateAdditionalParameters()
        => new Dictionary<string, object>
        {
            { nameof(CSharpClassGenerator.EnableNullableContext), EnableNullableContext },
            { nameof(CSharpClassGenerator.CreateCodeGenerationHeader), CreateCodeGenerationHeader },
            { nameof(CSharpClassGenerator.GenerateMultipleFiles), GenerateMultipleFiles },
            { nameof(CSharpClassGenerator.FileNamePrefix), FileNamePrefix },
            { nameof(CSharpClassGenerator.FileNameSuffix), FileNameSuffix }
        };

    private string FileNamePrefix => string.IsNullOrEmpty(Path)
        ? string.Empty
        : Path + "\\";

    public object CreateGenerator()
        => new CSharpClassGenerator();
}
