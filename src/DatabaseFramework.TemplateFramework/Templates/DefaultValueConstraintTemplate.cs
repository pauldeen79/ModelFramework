namespace DatabaseFramework.TemplateFramework.Templates;

public class DefaultValueConstraintTemplate : DatabaseSchemaGeneratorBase<DefaultValueConstraintViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.AppendLine($"ALTER TABLE [{Model.TableEntityName}] ADD CONSTRAINT [{Model.Name}] DEFAULT ({Model.DefaultValue}) FOR [{Model.FieldName}]");
        builder.AppendLine("GO");
    }
}
