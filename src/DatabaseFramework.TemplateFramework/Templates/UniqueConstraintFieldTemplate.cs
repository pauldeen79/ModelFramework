namespace DatabaseFramework.TemplateFramework.Templates;

public class UniqueConstraintFieldTemplate : DatabaseSchemaGeneratorBase<UniqueConstraintFieldViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.Append($"\t[{Model.Name}]");

        if (!Model.IsLastUniqueConstraintField)
        {
            builder.Append(",");
        }

        builder.AppendLine();
    }
}
