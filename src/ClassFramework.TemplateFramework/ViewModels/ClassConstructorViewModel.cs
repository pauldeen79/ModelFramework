namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassConstructorViewModel : CsharpClassGeneratorViewModel<ClassConstructor>
{
    private readonly TypeBase _parent;
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public ClassConstructorViewModel(ClassConstructor data, CsharpClassGeneratorSettings settings, TypeBase parent, ICsharpExpressionCreator csharpExpressionCreator) : base(data, settings)
    {
        Guard.IsNotNull(parent);
        Guard.IsNotNull(csharpExpressionCreator);

        _parent = parent;
        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public string Name => _parent.Name.Sanitize().GetCsharpFriendlyName();

    public string ChainCall => string.IsNullOrEmpty(Data.ChainCall)
        ? string.Empty
        : $" : {Data.ChainCall}";

    public bool OmitCode => _parent is Interface || Data.Abstract;

    public IEnumerable<CsharpClassGeneratorViewModel> GetCodeStatementModels()
        => Data.CodeStatements
            .Select(codeStatement => new CodeStatementViewModel(codeStatement, Settings))
            .SelectMany((item, index) => index + 1 < Data.Parameters.Count ? [item, new NewLineViewModel(Settings)] : new CsharpClassGeneratorViewModel[] { item });

    public IEnumerable<CsharpClassGeneratorViewModel> GetParameterModels()
        => Data.Parameters
            .Select(parameter => new ParameterViewModel(parameter, Settings, _csharpExpressionCreator))
            .SelectMany((item, index) => index + 1 < Data.Parameters.Count ? [item, new SpaceAndCommaViewModel(Settings)] : new CsharpClassGeneratorViewModel[] { item });

    public IEnumerable<CsharpClassGeneratorViewModel> GetAttributeModels()
        => Data.Attributes.Select(attribute => new AttributeViewModel(attribute, Settings, _csharpExpressionCreator, Data));
}
