namespace ClassFramework.Pipelines.Interface;

public class InterfaceContext : ContextBase<IType, PipelineSettings>
{
    public InterfaceContext(IType sourceModel, PipelineSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }
}
