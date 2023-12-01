namespace ClassFramework.TemplateFramework.ViewModels;

public class ClassPropertyViewModel : CsharpClassGeneratorViewModel<ClassProperty>
{
    public ClassPropertyViewModel(ClassProperty data, CsharpClassGeneratorSettings settings, ICsharpExpressionCreator csharpExpressionCreator) : base(data, settings, csharpExpressionCreator)
    {
    }
}
