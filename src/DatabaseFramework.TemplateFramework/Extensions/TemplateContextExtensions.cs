namespace DatabaseFramework.TemplateFramework.Extensions;

public static class TemplateContextExtensions
{
    public static DatabaseSchemaGeneratorSettings? GetDatabaseSchemaGeneratorSettings(this ITemplateContext instance)
    {
        var rootTemplate = instance.RootContext.Template;
        var modelProperty = rootTemplate.GetType().GetProperty(nameof(IModelContainer<object>.Model));
        if (modelProperty is not null)
        {
            var modelValue = modelProperty.GetValue(instance.RootContext.Template);
            if (modelValue is IDatabaseSchemaGeneratorSettingsContainer container && container.Settings is not null)
            {
                return container.Settings;
            }
        }

        return null;
    }
}
