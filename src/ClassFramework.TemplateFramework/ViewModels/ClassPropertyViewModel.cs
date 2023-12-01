namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassPropertyViewModel : CsharpClassGeneratorViewModelBase<ClassProperty>
{
    public ClassPropertyViewModel(CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(settings, csharpExpressionCreator)
    {
    }
}
