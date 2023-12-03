namespace ClassFramework.TemplateFramework.ViewModels.CodeStatements;

public class StringCodeStatementViewModel : CsharpClassGeneratorViewModelBase<StringCodeStatement>
{
    public StringCodeStatementViewModel(ICsharpExpressionCreator csharpExpressionCreator) : base(csharpExpressionCreator)
    {
    }

    public string? Statement => GetModel().Statement;
}
