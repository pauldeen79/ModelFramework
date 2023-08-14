namespace ModelFramework.CodeGeneration;

public class TemplateProxy : IStringBuilderTemplate, IParameterizedTemplate
{
    public object Instance { get; }
    
    private readonly Dictionary<string, object?> _session = new();

    public TemplateProxy(object instance)
    {
        Guard.IsNotNull(instance);
        Instance = instance;
    }

    public ITemplateParameter[] GetParameters()
        => Instance.GetType()
            .GetProperties()
            .Where(x => x.CanRead)
            .Select(x => new TemplateParameter(x.Name, x.PropertyType))
            .ToArray();

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        var sessionProperty = Instance.GetType().GetProperty("Session");
        if (sessionProperty is not null)
        {
            sessionProperty.SetValue(Instance, _session);
        }

        // First initialize the TTTF template...
        var initializeMethod = Instance.GetType().GetMethod("Initialize");
        if (initializeMethod is not null)
        {
            initializeMethod.Invoke(Instance, new object?[] { null });
        }

        var renderMethod = Instance.GetType().GetMethod(nameof(Render));
        if (renderMethod is null)
        {
            throw new NotSupportedException("No Render method found");
        }

        renderMethod.Invoke(Instance, new object?[] { builder });
    }

    public void SetParameter(string name, object? value) => _session[name] = value;
}
