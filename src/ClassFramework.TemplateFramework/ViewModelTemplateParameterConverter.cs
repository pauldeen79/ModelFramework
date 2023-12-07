namespace ClassFramework.TemplateFramework;

public class ViewModelTemplateParameterConverter : ITemplateParameterConverter
{
    private readonly IEnumerable<IViewModel> _viewModels;

    public ViewModelTemplateParameterConverter(IEnumerable<IViewModel> viewModels)
    {
        Guard.IsNotNull(viewModels);

        _viewModels = viewModels;
    }

    public bool TryConvert(object? value, Type type, ITemplateEngineContext context, out object? convertedValue)
    {
        if (value is null)
        {
            convertedValue = null;
            return false;
        }

        var viewModel = _viewModels.FirstOrDefault(x => Supports(x, value));
        if (viewModel is null)
        {
            convertedValue = null;
            return false;
        }

        convertedValue = viewModel;

        // Copy Model to ViewModel
        var prop = viewModel.GetType().GetProperty(nameof(IModelContainer<object>.Model));
        if (prop is not null && prop.GetValue(viewModel) is null && prop.PropertyType.IsInstanceOfType(value))
        {
            prop.SetValue(viewModel, value);
        }
        
        // Copy Settings to ViewUodel
        //TODO: Do something so we can get the settings from the context, or something...
        if (viewModel is ICsharpClassGeneratorSettingsContainer container && container.Settings is null && context?.AdditionalParameters is ICsharpClassGeneratorSettingsContainer container2)
        {
            container.Settings = container2.Settings;
        }

        return true;
    }

    private bool Supports(object viewModel, object? model)
    {
        var viewModelType = viewModel.GetType();
        var prop = viewModelType.GetProperty(nameof(IModelContainer<object>.Model));

        return prop is not null
            && prop.PropertyType.IsInstanceOfType(model);
    }
}
