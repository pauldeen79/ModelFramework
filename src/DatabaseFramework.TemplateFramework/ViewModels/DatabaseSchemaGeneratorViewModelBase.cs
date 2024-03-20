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
