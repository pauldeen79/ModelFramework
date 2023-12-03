namespace ClassFramework.TemplateFramework.ViewModels;

public abstract class MethodViewModelBase<T> : AttributeContainerViewModelBase<T>
    where T : IAttributesContainer, IParametersContainer, ICodeStatementsContainer, IVisibilityContainer, IMetadataContainer
{
    protected MethodViewModelBase(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }

    public string Modifiers
        => GetModel().GetModifiers();

    public IEnumerable GetCodeStatementModels()
        => GetModel().CodeStatements
            .SelectMany((item, index) => index + 1 < Model!.Parameters.Count ? [item, new NewLineViewModel(Settings)] : new object[] { item });

    public IEnumerable GetParameterModels()
        => GetModel().Parameters
            .SelectMany((item, index) => index + 1 < Model!.Parameters.Count ? [item, new SpaceAndCommaViewModel(Settings)] : new object[] { item });
}
