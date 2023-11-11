namespace ClassFramework.Pipelines;

public record ClassPropertyContext : ContextBase<ClassProperty, IPipelineBuilderGenerationSettings>
{
    public ClassPropertyContext(ClassProperty model, IPipelineBuilderGenerationSettings settings, IFormatProvider formatProvider, string typeName)
        : base(model, settings, formatProvider)
    {
        TypeName = typeName.IsNotNull(nameof(typeName));
    }

    public string TypeName { get; }
}
