namespace ClassFramework.Pipelines;

public record ClassPropertyContext : ContextBase<ClassProperty, IPropertyGenerationSettings>
{
    public ClassPropertyContext(ClassProperty model, IPropertyGenerationSettings settings, IFormatProvider formatProvider, string typeName)
        : base(model, settings, formatProvider)
    {
        TypeName = typeName.IsNotNull(nameof(typeName));
    }

    public string TypeName { get; }
}
