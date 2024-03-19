namespace DatabaseFramework.TemplateFramework.ViewModels;

public class TableFieldViewModel : DatabaseSchemaGeneratorViewModelBase<TableField>
{
    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string Identity
        => GetModel().IsIdentity
            ? " IDENTITY(1, 1)"
            : string.Empty;

    public string NullOrNotNull
        => GetModel().IsRequired
            ? "NOT NULL"
            : "NULL";

    public bool HasCheckConstraints
        => GetModel().CheckConstraints.Count > 0;

    public bool IsLastTableField
        => Context.IsLastIteration
            ?? throw new InvalidOperationException("Can only render table fields as part of a table. There is no context with hierarchy.");

    public IReadOnlyCollection<CheckConstraint> CheckConstraints
        => GetModel().CheckConstraints;

    public NonViewFieldModel NonViewField
        => new NonViewFieldModel(GetModel());
}
