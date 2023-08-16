namespace ModelFramework.CodeGeneration;

public class MultipleContentTemplateProxy : IMultipleContentBuilderTemplate, IParameterizedTemplate, IModelContainer<object?>
{
    public object Instance { get; }
    public object? Model { get; set; }

    private readonly Dictionary<string, object?> _session = new();

    public MultipleContentTemplateProxy(object instance)
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

    public void Render(TemplateFramework.Abstractions.IMultipleContentBuilder builder)
    {
        Guard.IsNotNull(builder);

        var env = new TextTemplateTransformationFramework.Runtime.MultipleContentBuilder();
        TemplateRenderHelper.RenderTemplateWithModel(Instance, env, Model, additionalParameters: _session);
        foreach (var item in env.Contents)
        {
            var filename = item.FileName;
            if (string.IsNullOrEmpty(filename))
            {
                filename = "file.txt";
            }
            var content = builder.AddContent(filename, item.SkipWhenFileExists, null);
            var result = item.Builder.ToString().NormalizeLineEndings();
            if (!string.IsNullOrEmpty(result))
            {
                if (result.EndsWith(Environment.NewLine))
                {
                    content.Builder.Append(result.AsSpan(0, item.Builder.Length - Environment.NewLine.Length));
                }
                else
                {
                    content.Builder.Append(result);
                }
            }
        }
    }
}
