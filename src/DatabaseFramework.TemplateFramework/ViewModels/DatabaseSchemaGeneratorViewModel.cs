namespace DatabaseFramework.TemplateFramework.ViewModels;

public class DatabaseSchemaGeneratorViewModel : DatabaseSchemaGeneratorViewModelBase<IEnumerable<IDatabaseObject>>
{
    public IOrderedEnumerable<IGrouping<string, IDatabaseObject>> Schemas
        => GetModel().GroupBy(x => x.Schema).OrderBy(x => x.Key);

    public IEnumerable<INameContainer> GetDatabaseObjects(IEnumerable<INameContainer> objects)
        => objects.SelectMany(GetItemsWithForeignKeyConstraints).OrderBy(GetOrder).ThenBy(item => item.Name);

    private IEnumerable<INameContainer> GetItemsWithForeignKeyConstraints(INameContainer item)
    {
        yield return item;

        if (item is Table table)
        {
            foreach (var fk in table.ForeignKeyConstraints)
            {
                yield return new ForeignKeyConstraintModel(fk, table);
            }
        }
    }

    private static int GetOrder(INameContainer item)
        => item switch
        {
            Table => 1,
            ForeignKeyConstraint => 2,
            StoredProcedure => 3,
            View => 4,
            _ => 99
        }; // note that this order is backwards compatible with ModelFramework.Database :-)
}
