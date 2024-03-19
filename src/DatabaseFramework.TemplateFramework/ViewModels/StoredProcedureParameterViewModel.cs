namespace DatabaseFramework.TemplateFramework.ViewModels;

public class StoredProcedureParameterViewModel : DatabaseSchemaGeneratorViewModelBase<StoredProcedureParameter>
{
    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public bool HasDefaultValue
        => !string.IsNullOrEmpty(GetModel().DefaultValue);

    public string DefaultValue
        => GetModel().DefaultValue;

    public bool IsLastParameter
        => Context.IsLastIteration
            ?? throw new InvalidOperationException("Can only render stored procedure parameters as part of a stored procedure. There is no context with hierarchy.");

    public NonViewFieldModel NonViewField
        => new NonViewFieldModel(GetModel());
}
