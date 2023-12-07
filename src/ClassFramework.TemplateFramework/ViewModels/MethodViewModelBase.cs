namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class MethodViewModelBase<T> : AttributeContainerViewModelBase<T>
    where T : IAttributesContainer, IParametersContainer, ICodeStatementsContainer, IVisibilityContainer, IMetadataContainer
{
    protected MethodViewModelBase(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }

    public string Modifiers
        => GetModel().GetModifiers(Settings.CultureInfo);

    public IEnumerable<StringCodeStatementViewModel> GetCodeStatementModels()
        => GetModel().CodeStatements.OfType<StringCodeStatement>().Select(x => new StringCodeStatementViewModel(CsharpExpressionCreator) { Model = x, Settings = Settings });

    public IEnumerable<object> GetParameterModels()
        => GetModel().Parameters
            .Select(x => new ParameterViewModel(CsharpExpressionCreator) { Model = x, Settings = Settings })
            .SelectMany((item, index) => index + 1 < Model!.Parameters.Count ? [item, new SpaceAndCommaViewModel(CsharpExpressionCreator) { Model = new SpaceAndCommaModel(), Settings = Settings }] : new object[] { item });
}
