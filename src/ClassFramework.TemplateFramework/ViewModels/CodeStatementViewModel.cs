namespace ClassFramework.TemplateFramework.ViewModels;

public class CodeStatementViewModel : CsharpClassGeneratorViewModelBase<CodeStatementBase>
{
    public CodeStatementViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }
}

public class CodeStatementViewModelCreator : IViewModelCreator
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public CodeStatementViewModelCreator(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);

        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public object Create(object model, CsharpClassGeneratorSettings settings)
        => new CodeStatementViewModel(settings, _csharpExpressionCreator);

    public bool Supports(object model)
        => model is CodeStatementBase;
}
