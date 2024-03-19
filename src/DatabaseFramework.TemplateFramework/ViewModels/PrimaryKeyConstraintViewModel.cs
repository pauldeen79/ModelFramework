namespace DatabaseFramework.TemplateFramework.ViewModels;

public class PrimaryKeyConstraintViewModel : DatabaseSchemaGeneratorViewModelBase<PrimaryKeyConstraint>
{
    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string FileGroupName
        => GetModel().FileGroupName.WhenNullOrEmpty("PRIMARY").FormatAsDatabaseIdentifier();

    public IReadOnlyCollection<PrimaryKeyConstraintField> Fields
        => GetModel().Fields;
}
