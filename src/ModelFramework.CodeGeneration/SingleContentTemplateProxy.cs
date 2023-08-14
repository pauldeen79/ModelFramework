namespace ModelFramework.CodeGeneration;

public class SingleContentTemplateProxy : IStringBuilderTemplate, IParameterizedTemplate, IModelContainer<object?>
{
    public object Instance { get; }
    public object? Model { get; set; }

    private readonly Dictionary<string, object?> _session = new();

    public SingleContentTemplateProxy(object instance)
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

    public void SetParameter(string name, object? value) => _session[name] = value;

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        TemplateRenderHelper.RenderTemplateWithModel(Instance, builder, Model, additionalParameters: _session);
        var contents = TextTemplateTransformationFramework.Runtime.MultipleContentBuilder.FromString(builder.ToString()).Contents.First().Builder.ToString();
        builder.Clear();
        builder.Append(contents.AsSpan(0, contents.Length - Environment.NewLine.Length));
    }
}
