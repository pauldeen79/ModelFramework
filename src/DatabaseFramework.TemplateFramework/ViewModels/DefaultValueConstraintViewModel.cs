namespace DatabaseFramework.TemplateFramework.ViewModels;

public class DefaultValueConstraintViewModel : DatabaseSchemaGeneratorViewModelBase<DefaultValueConstraint>
{
    public string TableEntityName
        => (Context.GetModelFromContextByType<Table>() ?? throw new InvalidOperationException("Can only render default value constraints as part of a table. There is no context with a Table entity."))
            .Name.FormatAsDatabaseIdentifier();

    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string DefaultValue
        => GetModel().DefaultValue;

    public string FieldName
        => GetModel().FieldName.FormatAsDatabaseIdentifier();
}
