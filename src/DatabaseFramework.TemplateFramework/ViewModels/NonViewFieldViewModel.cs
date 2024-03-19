namespace DatabaseFramework.TemplateFramework.ViewModels;

public class NonViewFieldViewModel : DatabaseSchemaGeneratorViewModelBase<NonViewFieldModel>
{
    public string Type
        => GetModel().Value.Type.ToString().ToUpper(Settings.CultureInfo);

    public bool IsDatabaseStringType
        => GetModel().Value.Type.IsDatabaseStringType();

    public bool IsNumeric
    {
        get
        {
            var model = GetModel().Value;
            return model.NumericPrecision is not null
                || model.NumericScale is not null;
        }
    }

    public string? NumericPrecision
        => GetModel().Value.NumericPrecision?.ToString(Settings.CultureInfo);

    public string? NumericScale
        => GetModel().Value.NumericScale?.ToString(Settings.CultureInfo);

    public string StringLength
        => GetModel().Value.StringLength.GetValueOrDefault(32).ToString(Settings.CultureInfo);

    public bool HasStringCollation
        => !string.IsNullOrEmpty(GetModel().Value.StringCollation);

    public string StringCollation
        => GetModel().Value.StringCollation;

    public bool IsStringMaxLength
        => GetModel().Value.IsStringMaxLength.GetValueOrDefault();
}
