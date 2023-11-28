namespace ClassFramework.TemplateFramework.Templates;

public class ClassPropertyTemplate : CsharpClassGeneratorBase<ClassPropertyViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
    }
}
