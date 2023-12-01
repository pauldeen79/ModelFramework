namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class MethodViewModelBase<T> : AttributeContainerViewModelBase<T>
    where T : IAttributesContainer, IParametersContainer, ICodeStatementsContainer
{
    protected MethodViewModelBase(T data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator) : base(data, settings, csharpExpressionCreator)
    {
    }

    public IEnumerable<CsharpClassGeneratorViewModelBase> GetCodeStatementModels()
        => Data.CodeStatements
            .Select(codeStatement => new CodeStatementViewModel(codeStatement, Settings, CsharpExpressionCreator))
            .SelectMany((item, index) => index + 1 < Data.Parameters.Count ? [item, new NewLineViewModel(Settings)] : new CsharpClassGeneratorViewModelBase[] { item });

    public IEnumerable<CsharpClassGeneratorViewModelBase> GetParameterModels()
        => Data.Parameters
            .Select(parameter => new ParameterViewModel(parameter, Settings, CsharpExpressionCreator))
            .SelectMany((item, index) => index + 1 < Data.Parameters.Count ? [item, new SpaceAndCommaViewModel(Settings)] : new CsharpClassGeneratorViewModelBase[] { item });
}
