namespace ModelFramework.CodeGeneration.Tests;

public class ViewModelInitializer : ITemplateInitializerComponent
{
    private readonly IValueConverter _converter;

    public ViewModelInitializer(IValueConverter converter)
    {
        Guard.IsNotNull(converter);

        _converter = converter;
    }

    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        Guard.IsNotNull(request);
        Guard.IsNotNull(engine);

        TrySetViewModelOnTemplate(request);
    }

    private void TrySetViewModelOnTemplate(IRenderTemplateRequest request)
    {
        var wrapper = request.Template as TemplateProxy;
        if (wrapper is null)
        {
            return;
        }

        var prop = wrapper.Instance.GetType().GetProperty("ViewModel");
        if (prop is not null)
        {
            var viewModelValue = prop.GetValue(wrapper.Instance);
            if (viewModelValue is null)
            {
                // Initialize when value is null (needs public parameterless constructor)
                var viewModel = Activator.CreateInstance(prop.PropertyType);
                if (viewModel is null)
                {
                    return;
                }

                prop.SetValue(wrapper.Instance, viewModel);

                // Copy model to viewmodel
                var modelField = viewModel.GetType().GetProperty("Model");
                if (modelField is not null)
                {
                    modelField.SetValue(viewModel, _converter.Convert(request.Model, modelField.PropertyType));
                }

                // Copy additional properties to viewmodel
                var session = request.AdditionalParameters.ToKeyValuePairs();
                foreach (var property in viewModel.GetType().GetProperties().Where(x => x.CanWrite))
                {
                    var parameterValue = session.Select(x => new { x.Key, x.Value }).FirstOrDefault(x => x.Key == property.Name);
                    if (parameterValue is null)
                    {
                        continue;
                    }

                    property.SetValue(viewModel, _converter.Convert(parameterValue.Value, property.PropertyType));
                }

                //TODO: Check if we need stuff like TemplateContext in ViewModel... need a wrapper for that probably?
            }
        }
    }
}
