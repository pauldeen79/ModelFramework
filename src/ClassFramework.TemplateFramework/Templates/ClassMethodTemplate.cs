namespace ClassFramework.TemplateFramework.Templates;

public class ClassMethodTemplate : CsharpClassGeneratorBase<ClassMethodViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
    }
}
