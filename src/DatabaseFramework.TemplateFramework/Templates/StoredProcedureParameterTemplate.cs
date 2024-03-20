namespace DatabaseFramework.TemplateFramework.Templates;

public class StoredProcedureParameterTemplate : DatabaseSchemaGeneratorBase<StoredProcedureParameterViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.Append($"\t@{Model.Name} ");

        RenderChildTemplateByModel(Model.NonViewField, builder);

        if (Model.HasDefaultValue)
        {
            builder.Append($" = {Model.DefaultValue}");
        }

        if (!Model.IsLastParameter)
        {
            builder.Append(",");
        }

        builder.AppendLine();
    }
}
