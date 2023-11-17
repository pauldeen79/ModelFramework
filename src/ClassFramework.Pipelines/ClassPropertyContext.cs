namespace ClassFramework.Pipelines;

public record ClassPropertyContext : ContextBase<ClassProperty, IPipelineGenerationSettings>
{
    public ClassPropertyContext(ClassProperty model, IPipelineGenerationSettings settings, IFormatProvider formatProvider, string typeName)
        : base(model, settings, formatProvider)
    {
        TypeName = typeName.IsNotNull(nameof(typeName));
    }

    public string TypeName { get; }
}
