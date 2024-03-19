namespace DatabaseFramework.TemplateFramework.ViewModels;

public class UniqueConstraintFieldViewModel : DatabaseSchemaGeneratorViewModelBase<UniqueConstraintField>
{
    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public bool IsLastUniqueConstraintField
        => Context.IsLastIteration
            ?? throw new InvalidOperationException("Can only render unique constraint fields as part of a unique constraint. There is no context with hierarchy.");
}
