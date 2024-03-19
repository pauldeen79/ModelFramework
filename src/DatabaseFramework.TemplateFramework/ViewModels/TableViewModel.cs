namespace DatabaseFramework.TemplateFramework.ViewModels;

public class TableViewModel : DatabaseSchemaGeneratorViewModelBase<Table>, INameContainer
{
    public string Schema
        => GetModel().Schema.FormatAsDatabaseIdentifier();

    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string FileGroupName
        => GetModel().FileGroupName.WhenNullOrEmpty("PRIMARY").FormatAsDatabaseIdentifier();

    public IReadOnlyCollection<TableField> Fields
        => GetModel().Fields;

    public IReadOnlyCollection<PrimaryKeyConstraint> PrimaryKeyConstraints
        => GetModel().PrimaryKeyConstraints;

    public IReadOnlyCollection<UniqueConstraint> UniqueConstraints
        => GetModel().UniqueConstraints;

    public IReadOnlyCollection<CheckConstraint> CheckConstraints
        => GetModel().CheckConstraints;

    public IReadOnlyCollection<Domain.Index> Indexes
        => GetModel().Indexes;

    public IReadOnlyCollection<DefaultValueConstraint> DefaultValueConstraints
        => GetModel().DefaultValueConstraints;

    public CodeGenerationHeaderModel CodeGenerationHeaders
        => new CodeGenerationHeaderModel(GetModel(), Settings.CreateCodeGenerationHeader);
}
