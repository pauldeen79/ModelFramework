namespace DatabaseFramework.TemplateFramework.ViewModels;

public class StoredProcedureParameterViewModel : DatabaseSchemaGeneratorViewModelBase<StoredProcedureParameter>
{
    public string Name
        => GetModel().Name.FormatAsDatabaseIdentifier();

    public string Type
        => GetTypeString(GetModel());

    public string DefaultValue
    {
        get
        {
            var model = GetModel();
            if (!string.IsNullOrEmpty(model.DefaultValue))
            {
                return $" = {model.DefaultValue}";
            }

            return string.Empty;
        }
    }

    public bool IsLastParameter
        => Context.IsLastIteration
            ?? throw new InvalidOperationException("Can only render stored procedure parameters as part of a stored procedure. There is no context with hierarchy.");
}
