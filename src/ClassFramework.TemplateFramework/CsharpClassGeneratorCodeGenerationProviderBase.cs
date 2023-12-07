namespace ClassFramework.TemplateFramework;

public abstract class CsharpClassGeneratorCodeGenerationProviderBase : ICodeGenerationProvider
{
    public string Path { get; }
    public bool RecurseOnDeleteGeneratedFiles { get; }
    public string LastGeneratedFilesFilename { get; }
    public Encoding Encoding { get; }

    protected CsharpClassGeneratorCodeGenerationProviderBase(
        ICsharpExpressionCreator csharpExpressionCreator,
        IEnumerable<TypeBase> model,
        CsharpClassGeneratorSettings settings)
    {
        Guard.IsNotNull(csharpExpressionCreator);
        Guard.IsNotNull(model);
        Guard.IsNotNull(settings);

        _csharpExpressionCreator = csharpExpressionCreator;
        _model = model;
        _settings = settings;

        Path = _settings.Path;
        RecurseOnDeleteGeneratedFiles = settings.RecurseOnDeleteGeneratedFiles;
        LastGeneratedFilesFilename = settings.LastGeneratedFilesFilename;
        Encoding = settings.Encoding;
    }

    private readonly IEnumerable<TypeBase> _model;
    private readonly CsharpClassGeneratorSettings _settings;
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public object? CreateAdditionalParameters()
        => null;

    public Type GetGeneratorType()
        => typeof(CsharpClassGenerator);

    public object? CreateModel()
        => new CsharpClassGeneratorViewModel(_csharpExpressionCreator)
        {
            Model = _model,
            Settings = _settings
            //Context is filled in base class, on the property setter of Context (propagated to Model)
        };
}
