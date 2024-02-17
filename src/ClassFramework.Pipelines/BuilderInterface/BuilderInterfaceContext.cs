namespace ClassFramework.Pipelines.BuilderInterface;

public class BuilderInterfaceContext : ContextBase<IType, PipelineSettings>
{
    public BuilderInterfaceContext(IType sourceModel, PipelineSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }

    public IEnumerable<Property> GetSourceProperties()
        => SourceModel.Properties.Where(x => SourceModel.IsMemberValidForBuilderClass(x, Settings));
}
