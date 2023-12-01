namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class MethodViewModelBase<T> : AttributeContainerViewModelBase<T>
    where T : IAttributesContainer, IParametersContainer, ICodeStatementsContainer
{
    protected MethodViewModelBase(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }

    public IEnumerable<CsharpClassGeneratorViewModelBase> GetCodeStatementModels()
        => GetModel().CodeStatements
            .Select((codeStatement, index) => new CodeStatementViewModel(Settings, CsharpExpressionCreator)
            {
                Model = codeStatement,
                Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), codeStatement, index, Model!.CodeStatements.Count))
            })
            .SelectMany((item, index) => index + 1 < Model!.Parameters.Count ? [item, new NewLineViewModel(Settings)] : new CsharpClassGeneratorViewModelBase[] { item });

    public IEnumerable<CsharpClassGeneratorViewModelBase> GetParameterModels()
        => GetModel().Parameters
            .Select((parameter, index) => new ParameterViewModel(Settings, CsharpExpressionCreator)
            {
                Model = parameter,
                Context = Context.CreateChildContext(new ChildTemplateContext(new EmptyTemplateIdentifier(), parameter, index, Model!.Parameters.Count))
            })
            .SelectMany((item, index) => index + 1 < Model!.Parameters.Count ? [item, new SpaceAndCommaViewModel(Settings)] : new CsharpClassGeneratorViewModelBase[] { item });
}
