namespace ClassFramework.Pipelines.Entity;

public record EntityContext : ContextBase<TypeBase, PipelineBuilderSettings>
{
    public EntityContext(TypeBase model, PipelineBuilderSettings settings, IFormatProvider formatProvider)
        : base(model, settings, formatProvider)
    {
    }

    public bool IsAbstract
        => Settings.InheritanceSettings.EnableInheritance
        && Settings.InheritanceSettings.IsAbstract;

    public string MapTypeName(string typeName)
        => typeName.IsNotNull(nameof(typeName)).MapTypeName(Settings.TypeSettings);

    public string MapNamespace(string? ns)
        => ns.MapNamespace(Settings.TypeSettings);

    public Domain.Attribute MapAttribute(Domain.Attribute attribute)
    {
        attribute = attribute.IsNotNull(nameof(attribute));

        return new AttributeBuilder(attribute)
            .WithName(MapTypeName(attribute.Name))
            .Build();
    }

    public string CreateArgumentNullException(string argumentName)
    {
        if (Settings.NullCheckSettings.UseExceptionThrowIfNull)
        {
            return $"System.ArgumentNullException.ThrowIfNull({argumentName});";
        }

        return $"if ({argumentName} is null) throw new {typeof(ArgumentNullException).FullName}(nameof({argumentName}));";
    }
}
