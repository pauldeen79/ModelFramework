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

        builder.AppendLine($"/****** Object:  {Model.ObjectType} [{Model.Schema}].[{Model.Name}] ******/");
    }
}
