namespace DatabaseFramework.TemplateFramework.Templates;

public class StoredProcedureParameterTemplate : DatabaseSchemaGeneratorBase<StoredProcedureParameterViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.Append($"\t@{Model.Name} {Model.Type}{Model.DefaultValue}");

        if (!Model.IsLastParameter)
        {
            builder.Append(",");
        }

        builder.AppendLine();
    }
}
