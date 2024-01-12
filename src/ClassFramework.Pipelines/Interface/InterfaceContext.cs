namespace ClassFramework.Pipelines.Interface;

public record InterfaceContext : ContextBase<IType, PipelineSettings>
{
    public InterfaceContext(IType sourceModel, PipelineSettings settings, IFormatProvider formatProvider)
        : base(sourceModel, settings, formatProvider)
    {
    }

    public string MapNamespace(string? ns)
        => ns.MapNamespace(Settings.TypeSettings);

    public string MapTypeName(string typeName)
        => typeName.IsNotNull(nameof(typeName)).MapTypeName(Settings.TypeSettings);

    public Domain.Attribute MapAttribute(Domain.Attribute attribute)
    {
        attribute = attribute.IsNotNull(nameof(attribute));

        return new AttributeBuilder(attribute)
            .WithName(MapTypeName(attribute.Name.FixTypeName()))
            .Build();
    }
}
