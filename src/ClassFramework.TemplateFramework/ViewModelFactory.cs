namespace ClassFramework.TemplateFramework;

public class ViewModelFactory : IViewModelFactory
{
    private readonly IEnumerable<IViewModelFactoryComponent> _viewModelCreators;

    public ViewModelFactory(IEnumerable<IViewModelFactoryComponent> viewModelCreators)
    {
        Guard.IsNotNull(viewModelCreators);

        _viewModelCreators = viewModelCreators;
    }

    public object Create(object model)
    {
        var creator = _viewModelCreators.FirstOrDefault(x => x.Supports(model));
        if (creator is null)
        {
            throw new NotSupportedException($"Model of type {model?.GetType().FullName ?? "NULL"} is not supported");
        }

        return creator.Create();
    }
}
