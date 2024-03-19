namespace DatabaseFramework.TemplateFramework.ViewModels;

public class StoredProcedureViewModel : DatabaseSchemaGeneratorViewModelBase<StoredProcedure>, INameContainer
{
    public string Schema
        => GetModel().Schema.FormatAsDatabaseIdentifier();

    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public CodeGenerationHeaderModel CodeGenerationHeaders
        => new CodeGenerationHeaderModel(GetModel(), Settings.CreateCodeGenerationHeader);

    public IReadOnlyCollection<StoredProcedureParameter> Parameters
        => GetModel().Parameters;

    public IReadOnlyCollection<SqlStatementBase> Statements
        => GetModel().Statements;
}
