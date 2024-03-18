namespace DatabaseFramework.TemplateFramework.ViewModels;

public class IndexViewModel : DatabaseSchemaGeneratorViewModelBase<Domain.Index>
{
    public string Unique
        => GetModel().Unique
            ? "UNIQUE "
            : string.Empty;

    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string TableEntityName
        => (Context.GetModelFromContextByType<Table>() ?? throw new InvalidOperationException("Can only render indexes as part of a table. There is no context with a Table entity."))
            .Name.FormatAsDatabaseIdentifier();

    public string Schema
        => (Context.GetModelFromContextByType<Table>() ?? throw new InvalidOperationException("Can only render indexes as part of a table. There is no context with a Table entity."))
            .Schema.FormatAsDatabaseIdentifier();

    public string FileGroupName
        => GetModel().FileGroupName.WhenNullOrEmpty("PRIMARY").FormatAsDatabaseIdentifier();

    public IReadOnlyCollection<IndexField> Fields
        => GetModel().Fields;
}
