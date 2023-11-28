namespace ClassFramework.TemplateFramework.Extensions;

public static class TemplateContextExtensions
{
    public static object? GetUnderlyingModel(this ITemplateContext templateContext)
    {
        var model = templateContext.Model;

        // Hacking here... If the parent context has iterations, then the model is wrapped in an anomyoust type (together with the index).
        // In this case, we need to get the Model property of the anonymous model instance.
        if (model is not null && model.GetType().FullName?.StartsWith("<>f__AnonymousType", StringComparison.Ordinal) == true)
        {
            var property = model.GetType().GetProperty("Model");
            if (property is not null)
            {
                model = property.GetValue(model);
            }
        }

        return model;
    }
}
