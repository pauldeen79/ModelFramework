namespace DatabaseFramework.TemplateFramework.ViewModels;

public class PrimaryKeyConstraintFieldViewModel : DatabaseSchemaGeneratorViewModelBase<PrimaryKeyConstraintField>
{
    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string Direction
        => GetModel().IsDescending
            ? "DESC"
            : "ASC";

    public bool IsLastPrimaryKeyConstraintField
        => Context.IsLastIteration
            ?? throw new InvalidOperationException("Can only render primary key constraint fields as part of a primary key constraint. There is no context with hierarchy.");
}
