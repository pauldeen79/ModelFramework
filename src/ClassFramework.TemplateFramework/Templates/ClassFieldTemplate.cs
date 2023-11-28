namespace ClassFramework.TemplateFramework.Templates;

public class ClassFieldTemplate : CsharpClassGeneratorBase<ClassFieldViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
    }
}
