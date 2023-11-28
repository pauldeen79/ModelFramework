namespace ClassFramework.TemplateFramework.Templates;

public class ClassConstructorTemplate : CsharpClassGeneratorBase<ClassConstructorViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
    }
}
