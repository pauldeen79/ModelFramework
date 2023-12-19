namespace ClassFramework.Pipelines;

public record PropertyContext : ContextBase<Property, IPipelineGenerationSettings>
{
    public PropertyContext(Property model, IPipelineGenerationSettings settings, IFormatProvider formatProvider, string typeName)
        : base(model, settings, formatProvider)
    {
        TypeName = typeName.IsNotNull(nameof(typeName));
    }

    public string TypeName { get; }
}
