namespace ClassFramework.Pipelines.BuilderInterface;

public class BuilderInterfaceContext : ContextBase<IType>
{
    public BuilderInterfaceContext(IType sourceModel, PipelineSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }

    protected override string NewCollectionTypeName => Settings.BuilderNewCollectionTypeName;

    public IEnumerable<Property> GetSourceProperties()
        => SourceModel.Properties.Where(x => SourceModel.IsMemberValidForBuilderClass(x, Settings));
}
