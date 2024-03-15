namespace DatabaseFramework.TemplateFramework.ViewModels;

public class DatabaseSchemaGeneratorViewModel : DatabaseSchemaGeneratorViewModelBase<IEnumerable<IDatabaseObject>>
{
    public IOrderedEnumerable<IGrouping<string, IDatabaseObject>> Schemas
        => GetModel().GroupBy(x => x.Schema).OrderBy(x => x.Key);

    public IEnumerable<IDatabaseObject> GetDatabaseObjects(IEnumerable<IDatabaseObject> schema)
        => schema.OrderBy(item => item.Name);
}
