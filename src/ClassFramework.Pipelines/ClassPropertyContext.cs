namespace ClassFramework.Pipelines;

public record ClassPropertyContext : ContextBase<ClassProperty, IPipelineBuilderGenerationSettings>
{
    public ClassPropertyContext(ClassProperty sourceModel, IPipelineBuilderGenerationSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }
}
