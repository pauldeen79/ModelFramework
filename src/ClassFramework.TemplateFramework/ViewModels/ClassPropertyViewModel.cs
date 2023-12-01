namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassPropertyViewModel : CsharpClassGeneratorViewModelBase<ClassProperty>
{
    public ClassPropertyViewModel(ClassProperty data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator)
        : base(data, settings, csharpExpressionCreator)
    {
    }
}
