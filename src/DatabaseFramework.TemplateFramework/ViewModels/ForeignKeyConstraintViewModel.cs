namespace DatabaseFramework.TemplateFramework.ViewModels;

public class ForeignKeyConstraintViewModel : DatabaseSchemaGeneratorViewModelBase<ForeignKeyConstraintModel>
{
    public string Name
        => GetModel().ForeignKeyConstraint.Name.FormatAsDatabaseIdentifier();

    public string ForeignTableName
        => GetModel().ForeignKeyConstraint.ForeignTableName.FormatAsDatabaseIdentifier();

    public string CascadeUpdate
        => GetModel().ForeignKeyConstraint.CascadeUpdate.ToSql();

    public string CascadeDelete
        => GetModel().ForeignKeyConstraint.CascadeDelete.ToSql();

    public IReadOnlyCollection<ForeignKeyConstraintField> LocalFields
        => GetModel().ForeignKeyConstraint.LocalFields;

    public IReadOnlyCollection<ForeignKeyConstraintField> ForeignFields
        => GetModel().ForeignKeyConstraint.ForeignFields;

    public string TableEntityName
        => GetModel().Table.Name.FormatAsDatabaseIdentifier();

    public string Schema
        => GetModel().Table.Schema.FormatAsDatabaseIdentifier();

    public CodeGenerationHeaderModel CodeGenerationHeaders
        => new CodeGenerationHeaderModel(GetModel().ForeignKeyConstraint, Settings.CreateCodeGenerationHeader);
}
