namespace ClassFramework.TemplateFramework.Templates;

public class EnumerationTemplate : CsharpClassGeneratorBase<EnumerationViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
    }
}
