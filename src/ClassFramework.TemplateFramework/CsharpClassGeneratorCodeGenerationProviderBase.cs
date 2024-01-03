namespace ClassFramework.TemplateFramework;

public abstract class CsharpClassGeneratorCodeGenerationProviderBase : ICodeGenerationProvider
{
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
    }

    private readonly IEnumerable<TypeBase> _model;
    private readonly CsharpClassGeneratorSettings _settings;
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public string Path => _settings.Path;
    public bool RecurseOnDeleteGeneratedFiles => _settings.RecurseOnDeleteGeneratedFiles;
    public string LastGeneratedFilesFilename => _settings.LastGeneratedFilesFilename;
    public Encoding Encoding => _settings.Encoding;

    public object? CreateAdditionalParameters() => null;

    public Type GetGeneratorType() => typeof(CsharpClassGenerator);

    public object? CreateModel()
        => new CsharpClassGeneratorViewModel(_csharpExpressionCreator)
        {
            Model = _model,
            Settings = _settings
            //Context is filled in base class, on the property setter of Context (propagated to Model)
        };
}
