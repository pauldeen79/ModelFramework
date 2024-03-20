namespace DatabaseFramework.TemplateFramework.ViewModels;

public class UniqueConstraintViewModel : DatabaseSchemaGeneratorViewModelBase<UniqueConstraint>
{
    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string FileGroupName
        => GetModel().FileGroupName.WhenNullOrEmpty("PRIMARY").FormatAsDatabaseIdentifier();

    public IReadOnlyCollection<UniqueConstraintField> Fields
        => GetModel().Fields;
}
