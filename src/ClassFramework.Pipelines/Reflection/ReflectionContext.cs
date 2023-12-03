namespace ClassFramework.Pipelines.Reflection;

public record ReflectionContext : ContextBase<Type, PipelineBuilderSettings>
{
    public ReflectionContext(Type sourceModel, PipelineBuilderSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }
}
