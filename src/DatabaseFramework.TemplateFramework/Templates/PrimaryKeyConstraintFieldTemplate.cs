namespace DatabaseFramework.TemplateFramework.Templates;

public class PrimaryKeyConstraintFieldTemplate : DatabaseSchemaGeneratorBase<PrimaryKeyConstraintFieldViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.Append($"\t[{Model.Name}] {Model.Direction}");

        if (!Model.IsLastPrimaryKeyConstraintField)
        {
            builder.Append(",");
        }

        builder.AppendLine();
    }
}
