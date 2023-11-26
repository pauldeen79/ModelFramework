namespace ClassFramework.TemplateFramework;

public abstract class CsharpClassGeneratorCodeGenerationProviderBase : ICodeGenerationProvider, ITemplateComponentRegistryPlugin
{
    public string Path { get; }
    public bool RecurseOnDeleteGeneratedFiles { get; }
    public string LastGeneratedFilesFilename { get; }
    public Encoding Encoding { get; }

    protected CsharpClassGeneratorCodeGenerationProviderBase(
        IEnumerable<TypeBase> model,
        string path,
        bool recurseOnDeleteGeneratedFiles,
        string lastGeneratedFilesFilename,
        Encoding encoding,
        bool generateMultipleFiles,
        bool skipWhenFileExists,
        bool createCodeGenerationHeader,
        bool enableNullableContext,
        CultureInfo cultureInfo,
        string filenameSuffix = ".template.generated",
        string? environmentVersion = null)
    {
        Guard.IsNotNull(model);
        Guard.IsNotNull(path);
        Guard.IsNotNull(lastGeneratedFilesFilename);
        Guard.IsNotNull(encoding);
        Guard.IsNotNull(cultureInfo);
        Guard.IsNotNull(filenameSuffix);

        _model = model;
        Path = path;
        RecurseOnDeleteGeneratedFiles = recurseOnDeleteGeneratedFiles;
        LastGeneratedFilesFilename = lastGeneratedFilesFilename;
        Encoding = encoding;
        _generateMultipleFiles = generateMultipleFiles;
        _skipWhenFileExists = skipWhenFileExists;
        _createCodeGenerationHeader = createCodeGenerationHeader;
        _enableNullableContext = enableNullableContext;
        _cultureInfo = cultureInfo;
        _environmentVersion = environmentVersion;
        _filenameSuffix = filenameSuffix;
    }

    private readonly IEnumerable<TypeBase> _model;
    private readonly bool _generateMultipleFiles;
    private readonly bool _skipWhenFileExists;
    private readonly bool _createCodeGenerationHeader;
    private readonly bool _enableNullableContext;
    private readonly CultureInfo _cultureInfo;
    private readonly string? _environmentVersion;
    private readonly string _filenameSuffix;

    public object? CreateAdditionalParameters() => null;

    public Type GetGeneratorType() => typeof(CsharpClassGenerator);

    public object? CreateModel()
        => new CsharpClassGeneratorViewModel<IEnumerable<TypeBase>>(
            _model,
            new CsharpClassGeneratorSettings(
                _generateMultipleFiles,
                _skipWhenFileExists,
                _createCodeGenerationHeader,
                _environmentVersion,
                FilenamePrefix,
                _filenameSuffix,
                _enableNullableContext,
                0,
                _cultureInfo
            )
        );

    public void Initialize(ITemplateComponentRegistry registry)
    {
        Guard.IsNotNull(registry);

        var registrations = new List<ITemplateCreator>
        {
            new TemplateCreator<CodeGenerationHeaderTemplate>("CodeGenerationHeader"),
            new TemplateCreator<UsingsTemplate>("Usings"),
            new TemplateCreator<TypeBaseTemplate>(typeof(TypeBase))
        };

        registry.RegisterComponent(new ProviderComponent(registrations));
    }

    private string? FilenamePrefix => string.IsNullOrEmpty(Path)
        ? string.Empty
        : Path + "/";
}
