namespace ModelFramework.CodeGeneration.Tests;

public class ViewModelTemplateInitializer : ITemplateInitializer
{
    public void Initialize(IRenderTemplateRequest request, ITemplateEngine engine)
    {
        var prop = request?.Template.GetType().GetProperty("ViewModel");
        if (prop is not null)
        {
            var value = prop.GetValue(request!.Template);
            if (value is null)
            {
                prop.SetValue(request.Template, Activator.CreateInstance(prop.PropertyType));
            }
        }
    }
}
