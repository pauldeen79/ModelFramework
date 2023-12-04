namespace ClassFramework.TemplateFramework;

public class ViewModelTemplateParameterConverter : ITemplateParameterConverter
{
    private readonly IEnumerable<IViewModel> _viewModels;

    public ViewModelTemplateParameterConverter(IEnumerable<IViewModel> viewModels)
    {
        Guard.IsNotNull(viewModels);

        _viewModels = viewModels;
    }

    public bool TryConvert(object? value, Type type, out object? convertedValue)
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
        return true;
    }

    private bool Supports(object viewModel, object? model)
    {
        var viewModelType = viewModel?.GetType();
        var prop = viewModelType?.GetProperty(nameof(IModelContainer<object>.Model));

        return prop is not null
            && prop.PropertyType.IsInstanceOfType(model);
    }
}
