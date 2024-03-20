namespace DatabaseFramework.TemplateFramework.Templates;

public class TableFieldTemplate : DatabaseSchemaGeneratorBase<TableFieldViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.Append($"\t[{Model.Name}] ");
        
        RenderChildTemplateByModel(Model.NonViewField, builder);
        
        builder.Append($"{Model.Identity} {Model.NullOrNotNull}");

        if (Model.HasCheckConstraints)
        {
            builder.AppendLine();
        }

        RenderChildTemplatesByModel(Model.CheckConstraints, builder);

        if (!Model.IsLastTableField)
        {
            builder.Append(",");
        }

        builder.AppendLine();
    }
}
