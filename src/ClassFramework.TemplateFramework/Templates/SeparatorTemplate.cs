namespace ClassFramework.TemplateFramework.Templates;

public class SeparatorTemplate : CsharpClassGeneratorBase<SeparatorViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        builder.AppendLine();
    }
}
