namespace DatabaseFramework.TemplateFramework.Templates;

public class PrimaryKeyConstraintTemplate : DatabaseSchemaGeneratorBase<PrimaryKeyConstraintViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.AppendLine($" CONSTRAINT [{Model.Name}] PRIMARY KEY CLUSTERED");
        builder.AppendLine("(");
        RenderChildTemplatesByModel(Model.Fields, builder);
        builder.AppendLine($") ON [{Model.FileGroupName}]");
    }
}
