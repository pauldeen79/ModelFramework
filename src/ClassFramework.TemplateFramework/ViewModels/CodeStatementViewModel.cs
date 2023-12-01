namespace ClassFramework.TemplateFramework.ViewModels;

public class CodeStatementViewModel : CsharpClassGeneratorViewModelBase<CodeStatementBase>
{
    public CodeStatementViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }
}
