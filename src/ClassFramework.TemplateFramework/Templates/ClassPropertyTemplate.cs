namespace ClassFramework.TemplateFramework.Templates;

public class ClassPropertyTemplate : CsharpClassGeneratorBase<ClassPropertyViewModel>, IStringBuilderTemplate
{
    public ClassPropertyTemplate(IViewModelFactory viewModelFactory) : base(viewModelFactory)
    {
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
    }
}
