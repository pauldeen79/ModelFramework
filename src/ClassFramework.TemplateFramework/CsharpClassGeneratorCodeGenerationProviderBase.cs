namespace ClassFramework.TemplateFramework;

public abstract class CsharpClassGeneratorCodeGenerationProviderBase : ICodeGenerationProvider
{
    public string Path { get; }
    public bool RecurseOnDeleteGeneratedFiles { get; }
    public string LastGeneratedFilesFilename { get; }
    public Encoding Encoding { get; }

#pragma warning disable S107 // Methods should not have too many parameters
    protected CsharpClassGeneratorCodeGenerationProviderBase(
        ICsharpExpressionCreator csharpExpressionCreator,
        IEnumerable<TypeBase> model,
        //TODO: Refactor to inject CsharpClassGeneratorSettings instance instead of separate arguments, fixing S107. need to move FilenamePrefix to this class...
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
#pragma warning restore S107 // Methods should not have too many parameters
    {
        Guard.IsNotNull(csharpExpressionCreator);
        Guard.IsNotNull(model);
        Guard.IsNotNull(path);
        Guard.IsNotNull(lastGeneratedFilesFilename);
        Guard.IsNotNull(encoding);
        Guard.IsNotNull(cultureInfo);
        Guard.IsNotNull(filenameSuffix);

        _csharpExpressionCreator = csharpExpressionCreator;
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
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public object? CreateAdditionalParameters()
        => null;

    public Type GetGeneratorType()
        => typeof(CsharpClassGenerator);

    public object? CreateModel()
        => new CsharpClassGeneratorViewModel(_csharpExpressionCreator)
        {
            Model = _model,
            Settings = new CsharpClassGeneratorSettingsBuilder()
                .WithGenerateMultipleFiles(_generateMultipleFiles)
                .WithSkipWhenFileExists(_skipWhenFileExists)
                .WithCreateCodeGenerationHeader(_createCodeGenerationHeader)
                .WithEnvironmentVersion(_environmentVersion)
                .WithFilenamePrefix(FilenamePrefix)
                .WithFilenameSuffix(_filenameSuffix)
                .WithEnableNullableContext(_enableNullableContext)
                .WithIndentCount(1)
                .WithCultureInfo(_cultureInfo)
                .Build()
            //Context is filled in base class, on the property setter of Context (propagated to Model)
        };

    private string? FilenamePrefix
        => string.IsNullOrEmpty(Path)
            ? string.Empty
            : Path + "/";
}
