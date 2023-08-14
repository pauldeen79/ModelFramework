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

        TemplateRenderHelper.RenderTemplateWithModel(Instance, builder, null, additionalParameters: _session);
    }

    public void SetParameter(string name, object? value) => _session[name] = value;
}
