namespace ClassFramework.TemplateFramework;

public class ViewModelTemplateProviderComponent : ITemplateProviderComponent
{
    private readonly IEnumerable<IViewModel> _viewModels;
    private readonly IEnumerable<ITemplateCreator> _childTemplateCreators;

    public ViewModelTemplateProviderComponent(
        IEnumerable<IViewModel> viewModels,
        IEnumerable<ITemplateCreator> childTemplateCreators)
    {
        Guard.IsNotNull(viewModels);
        Guard.IsNotNull(childTemplateCreators);

        _viewModels = viewModels;
        _childTemplateCreators = childTemplateCreators;
    }

    public object Create(ITemplateIdentifier identifier)
    {
        Guard.IsNotNull(identifier);

        if (identifier is ViewModelTemplateByModelIdentifier templateByModelIdentifier && templateByModelIdentifier.Model is not null)
        {
            return CreateByModel(templateByModelIdentifier.Model, templateByModelIdentifier.Settings);
        }

        throw new NotSupportedException($"Unsupported template identifier: {identifier.GetType().FullName}");
    }

    public bool Supports(ITemplateIdentifier identifier)
        => (identifier is ViewModelTemplateByModelIdentifier viewModelIdentifier)
        && viewModelIdentifier.Model is not null;

    private object CreateByModel(object model, CsharpClassGeneratorSettings settings)
    {
        PropertyInfo? prop = null;
        var viewModel = _viewModels.FirstOrDefault(x => Supports(x, model, out prop) && _childTemplateCreators.Any(y => y.SupportsModel(x)));
        if (viewModel is null)
        {
            throw new NotSupportedException($"There is no viewmodel which supports a model of type {model?.GetType()}");
        }
        
        // Copy Model to ViewModel
        prop!.SetValue(viewModel, model);

        // Copy Settings to ViewModel
        if (viewModel is ICsharpClassGeneratorSettingsContainer settingsContainer)
        {
            settingsContainer.Settings = settings;
        }

        // Copied from ChildTemplateProvider project... (TemplateProvider class)
        var creator = _childTemplateCreators.FirstOrDefault(x => x.SupportsModel(viewModel));
        if (creator is null)
        {
            throw new NotSupportedException($"Model of type {model?.GetType()} is not supported");
        }

        return creator.CreateByModel(viewModel) ?? throw new InvalidOperationException("Child template creator returned a null instance");
    }

    private bool Supports(object viewModel, object model, out PropertyInfo? prop)
    {
        var viewModelType = viewModel?.GetType();
        prop = viewModelType?.GetProperty(nameof(IModelContainer<object>.Model));

        return prop is not null
            && prop.PropertyType.IsInstanceOfType(model);
    }
}
