namespace DatabaseFramework.TemplateFramework.Templates;

public class ForeignKeyConstraintTemplate : DatabaseSchemaGeneratorBase<ForeignKeyConstraintViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        RenderChildTemplateByModel(Model.CodeGenerationHeaders, builder);

        builder.Append($"ALTER TABLE [{Model.Schema}].[{Model.TableEntityName}]  WITH CHECK ADD  CONSTRAINT [{Model.Name}] FOREIGN KEY(");

        RenderChildTemplatesByModel(Model.LocalFields, builder);

        builder.AppendLine(")");
        builder.Append($"REFERENCES [{Model.Schema}].[{Model.ForeignTableName}] (");

        RenderChildTemplatesByModel(Model.ForeignFields, builder);

        builder.AppendLine(")");
        builder.AppendLine($"ON UPDATE {Model.CascadeUpdate}");
        builder.AppendLine($"ON DELETE {Model.CascadeDelete}");
        builder.AppendLine("GO");
        builder.AppendLine($"ALTER TABLE [{Model.Schema}].[{Model.TableEntityName}] CHECK CONSTRAINT [{Model.Name}]");
        builder.AppendLine("GO");
    }
}
