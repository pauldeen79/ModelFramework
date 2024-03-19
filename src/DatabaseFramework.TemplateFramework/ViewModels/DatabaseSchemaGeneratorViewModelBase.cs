namespace DatabaseFramework.TemplateFramework.ViewModels;

public abstract class DatabaseSchemaGeneratorViewModelBase : IDatabaseSchemaGeneratorSettingsContainer, IViewModel
{
    public DatabaseSchemaGeneratorSettings Settings { get; set; } = default!; // will always be injected in CreateModel (root viewmodel) or OnSetContext (child viewmodels) method

    protected DatabaseSchemaGeneratorSettings GetSettings()
    {
        Guard.IsNotNull(Settings);

        return Settings;
    }

    public string FilenamePrefix
        => string.IsNullOrEmpty(GetSettings().Path)
            ? string.Empty
            : $"{Settings.Path}{Path.DirectorySeparatorChar}";

    protected string GetTypeString(INonViewField field)
    {
        Guard.IsNotNull(field);

        var builder = new StringBuilder();
        builder.Append($"{field.Type.ToString().ToUpper(Settings.CultureInfo)}");

        if (field.Type.IsDatabaseStringType())
        {
            builder.Append("(");
            if (field.IsStringMaxLength == true)
            {
                builder.Append("max");
            }
            else
            {
                builder.Append(field.StringLength.GetValueOrDefault(32).ToString(Settings.CultureInfo));
            }
            builder.Append(")");
            if (!string.IsNullOrEmpty(field.StringCollation))
            {
                builder.Append($" COLLATE {field.StringCollation}");
            }
        }
        else if (field.NumericPrecision is not null && field.NumericScale is not null)
        {
            builder.Append($"({field.NumericPrecision.GetValueOrDefault(8).ToString(Settings.CultureInfo)},{field.NumericScale.GetValueOrDefault(0).ToString(Settings.CultureInfo)})");
        }

        return builder.ToString();
    }
}

public abstract class DatabaseSchemaGeneratorViewModelBase<TModel> : DatabaseSchemaGeneratorViewModelBase, IModelContainer<TModel>, ITemplateContextContainer
{
    public TModel? Model { get; set; }
    public ITemplateContext Context { get; set; } = default!; // will always be injected in OnSetContext method

    protected ITemplateContext GetContext()
    {
        Guard.IsNotNull(Context);

        return Context;
    }

    protected TModel GetModel()
    {
        Guard.IsNotNull(Model);

        return Model;
    }

    protected object? GetParentModel() => GetContext().ParentContext?.Model;
}
