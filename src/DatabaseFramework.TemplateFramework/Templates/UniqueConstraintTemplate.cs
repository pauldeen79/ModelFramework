namespace DatabaseFramework.TemplateFramework.Templates;

public class UniqueConstraintTemplate : DatabaseSchemaGeneratorBase<UniqueConstraintViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.AppendLine(@$" CONSTRAINT [{Model.Name}] UNIQUE NONCLUSTERED
(");

        RenderChildTemplatesByModel(Model.Fields, builder);

        builder.AppendLine($") ON [{Model.FileGroupName}]");
    }
}
