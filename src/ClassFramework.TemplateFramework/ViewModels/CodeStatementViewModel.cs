namespace ClassFramework.TemplateFramework.ViewModels;

public class CodeStatementViewModel : CsharpClassGeneratorViewModelBase<CodeStatementBase>
{
    public CodeStatementViewModel(ICsharpExpressionCreator csharpExpressionCreator)
        : base(csharpExpressionCreator)
    {
    }
}

public class CodeStatementViewModelFactoryComponent : IViewModelFactoryComponent
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public CodeStatementViewModelFactoryComponent(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create()
        => new CodeStatementViewModel(_csharpExpressionCreator);

    public bool Supports(object model)
        => model is CodeStatementBase;
}
