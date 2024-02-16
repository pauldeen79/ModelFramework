namespace ClassFramework.Pipelines.Builder;

public class BuilderInterfaceContext : ContextBase<IType, PipelineSettings>
{
    public BuilderInterfaceContext(IType sourceModel, PipelineSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }

    public IEnumerable<Property> GetSourceProperties()
        => SourceModel.Properties.Where(x => SourceModel.IsMemberValidForBuilderClass(x, Settings));

    public string MapTypeName(string typeName)
        => typeName.IsNotNull(nameof(typeName)).MapTypeName(Settings.TypeSettings);

    public string MapNamespace(string? ns)
        => ns.MapNamespace(Settings.TypeSettings);

    public Domain.Attribute MapAttribute(Domain.Attribute attribute)
    {
        attribute = attribute.IsNotNull(nameof(attribute));

        return new AttributeBuilder(attribute)
            .WithName(MapTypeName(attribute.Name.FixTypeName()))
            .Build();
    }
}
