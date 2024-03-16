namespace DatabaseFramework.TemplateFramework.Templates;

public class NewLineTemplate : DatabaseSchemaGeneratorBase<NewLineViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        builder.AppendLine();
    }
}
