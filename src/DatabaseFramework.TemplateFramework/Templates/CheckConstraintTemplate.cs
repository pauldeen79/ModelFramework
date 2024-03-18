namespace DatabaseFramework.TemplateFramework.Templates;

public class CheckConstraintTemplate : DatabaseSchemaGeneratorBase<CheckConstraintViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.AppendLine($"    CONSTRAINT [{Model.Name}]");
        builder.Append($"    CHECK ({Model.Expression})");
        if (!Model.IsLastCheckConstraint)
        {
            builder.Append(",");
        }

        if (Context.GetModelFromContextByType<TableField>() is null)
        {
            builder.AppendLine();
        }
    }
}
