namespace DatabaseFramework.TemplateFramework.Models;

public class ForeignKeyConstraintModel : INameContainer
{
    public ForeignKeyConstraintModel(ForeignKeyConstraint foreignKeyConstraint, Table table)
    {
        Guard.IsNotNull(foreignKeyConstraint);
        Guard.IsNotNull(table);

        ForeignKeyConstraint = foreignKeyConstraint;
        Table = table;
    }

    public ForeignKeyConstraint ForeignKeyConstraint { get; }
    public Table Table { get; }

    public string Name => ForeignKeyConstraint.Name;
}
