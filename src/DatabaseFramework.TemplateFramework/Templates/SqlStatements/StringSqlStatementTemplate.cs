namespace DatabaseFramework.TemplateFramework.Templates.SqlStatements;

public class StringSqlStatementTemplate : DatabaseSchemaGeneratorBase<StringSqlStatementViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.AppendLine($"    {Model.Statement}");
    }
}
