namespace DatabaseFramework.TemplateFramework.ViewModels;

public class ForeignKeyConstraintFieldViewModel : DatabaseSchemaGeneratorViewModelBase<ForeignKeyConstraintField>
{
    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public bool IsLastForeignKeyConstraintField
        => Context.IsLastIteration
            ?? throw new InvalidOperationException("Can only render foreign key constraint fields as part of a foreign key constraint. There is no context with hierarchy.");
}
