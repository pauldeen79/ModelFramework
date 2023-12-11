namespace ClassFramework.TemplateFramework;

public class ViewModelTemplateParameterConverter : ITemplateParameterConverter
{
    private readonly Func<IEnumerable<IViewModel>> _factory;

    public ViewModelTemplateParameterConverter(Func<IEnumerable<IViewModel>> factory)
    {
        Guard.IsNotNull(factory);

        _factory = factory;
    }

    public bool TryConvert(object? value, Type type, ITemplateEngineContext context, out object? convertedValue)
    {
        if (value is null)
        {
            convertedValue = null;
            return false;
        }

        var viewModel = _factory.Invoke().FirstOrDefault(x => Supports(x, value));
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
