namespace DatabaseFramework.TemplateFramework.Templates;

public class IndexFieldTemplate : DatabaseSchemaGeneratorBase<IndexFieldViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.Append($"\t[{Model.Name}] {Model.Direction}");

        if (!Model.IsLastIndexField)
        {
            builder.Append(",");
        }

        builder.AppendLine();
    }
}
