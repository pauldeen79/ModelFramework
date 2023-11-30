namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class MethodViewModelBase<T> : CsharpClassGeneratorViewModel<T>
    where T : IAttributesContainer, IParametersContainer, ICodeStatementsContainer
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    protected MethodViewModelBase(T data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator) : base(data, settings)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

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
