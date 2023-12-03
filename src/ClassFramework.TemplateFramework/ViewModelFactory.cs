namespace ClassFramework.TemplateFramework;

public class ViewModelFactory : IViewModelFactory
{
    private readonly IEnumerable<IViewModel> _viewModels;

    public ViewModelFactory(IEnumerable<IViewModel> viewModelCreators)
    {
        Guard.IsNotNull(viewModelCreators);

        _viewModels = viewModelCreators;
    }

    public object Create(object model)
    {
        var viewModel = _viewModels.FirstOrDefault(x => Supports(x, model));
        if (viewModel is null)
        {
            throw new NotSupportedException($"Model of type {model?.GetType().FullName ?? "NULL"} is not supported");
        }

        return viewModel;
    }

    private bool Supports(object viewModel, object model)
    {
        var viewModelType = viewModel?.GetType();
        var prop = viewModelType?.GetProperty(nameof(IModelContainer<object>.Model));

        return prop is not null
            && prop.PropertyType.IsInstanceOfType(model);
    }
}
