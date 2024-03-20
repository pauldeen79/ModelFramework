namespace DatabaseFramework.TemplateFramework.Templates;

public class IndexTemplate : DatabaseSchemaGeneratorBase<IndexViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.AppendLine($"CREATE {Model.Unique}NONCLUSTERED INDEX [{Model.Name}] ON [{Model.Schema}].[{Model.TableEntityName}]");
        builder.AppendLine("(");

        RenderChildTemplatesByModel(Model.Fields, builder);

        builder.AppendLine($") ON [{Model.FileGroupName}]");
        builder.AppendLine("GO");
    }
}
