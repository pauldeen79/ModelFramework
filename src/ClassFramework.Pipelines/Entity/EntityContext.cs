﻿namespace ClassFramework.Pipelines.Entity;

public record EntityContext : ContextBase<TypeBase, PipelineBuilderSettings>
{
    public EntityContext(TypeBase model, PipelineBuilderSettings settings, IFormatProvider formatProvider)
        : base(model, settings, formatProvider)
    {
    }

    public string MapTypeName(string typeName)
        => typeName.MapTypeName(Settings.TypeSettings);

    public Domain.Attribute MapAttribute(Domain.Attribute attribute)
    {
        attribute = attribute.IsNotNull(nameof(attribute));

        return new AttributeBuilder(attribute)
            .WithName(MapTypeName(attribute.Name))
            .Build();
    }
}
