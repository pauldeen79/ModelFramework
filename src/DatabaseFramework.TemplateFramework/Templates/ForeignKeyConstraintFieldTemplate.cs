namespace DatabaseFramework.TemplateFramework.Templates;

public class ForeignKeyConstraintFieldTemplate : DatabaseSchemaGeneratorBase<ForeignKeyConstraintFieldViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.Append($"[{Model.Name}]");

        if (!Model.IsLastForeignKeyConstraintField)
        {
            builder.Append(",");
        }
    }
}
