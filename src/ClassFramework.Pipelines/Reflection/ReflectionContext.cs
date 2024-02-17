namespace ClassFramework.Pipelines.Reflection;

public class ReflectionContext : ContextBase<Type, PipelineSettings>
{
    public ReflectionContext(Type sourceModel, PipelineSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }
}
