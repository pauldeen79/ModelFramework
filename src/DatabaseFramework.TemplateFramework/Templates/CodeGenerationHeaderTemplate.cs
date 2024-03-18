namespace DatabaseFramework.TemplateFramework.Templates;

public sealed class CodeGenerationHeaderTemplate : DatabaseSchemaGeneratorBase<CodeGenerationHeaderViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        if (!Model.CreateCodeGenerationHeader)
        {
            return;
        }

        if (Model.Schema is not null)
        {
            builder.AppendLine($"/****** Object:  {Model.ObjectType} [{Model.Schema}].[{Model.Name}] ******/");
        }
        else
        {
            builder.AppendLine($"/****** Object:  {Model.ObjectType} [{Model.Name}] ******/");
        }
    }
}
