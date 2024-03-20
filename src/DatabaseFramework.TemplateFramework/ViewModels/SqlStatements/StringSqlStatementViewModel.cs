namespace DatabaseFramework.TemplateFramework.ViewModels.SqlStatements;

public class StringSqlStatementViewModel : DatabaseSchemaGeneratorViewModelBase<StringSqlStatement>
{
    public string Statement
        => GetModel().Statement;
}
