namespace DatabaseFramework.TemplateFramework.ViewModels;

public class IndexFieldViewModel : DatabaseSchemaGeneratorViewModelBase<IndexField>
{
    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string Direction
        => GetModel().IsDescending
            ? "DESC"
            : "ASC";

    public bool IsLastIndexField
        => Context.IsLastIteration
            ?? throw new InvalidOperationException("Can only render index fields as part of an index. There is no context with hierarchy.");
}
