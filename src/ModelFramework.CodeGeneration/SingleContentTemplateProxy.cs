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
        var output = builder.ToString();
        builder.Clear();
        if (output.Contains(@"<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">"))
        {
            var multipleContentBuilder = TextTemplateTransformationFramework.Runtime.MultipleContentBuilder.FromString(output);
            foreach (var item in multipleContentBuilder.Contents)
            {
                builder.Append(item.Builder);
            }
        }
        else if (!string.IsNullOrEmpty(output))
        {
            builder.Append(output);
        }
    }
}
