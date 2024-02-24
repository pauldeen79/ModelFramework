namespace ClassFramework.Pipelines;

public abstract class ContextBase<TModel>
{
    protected ContextBase(TModel sourceModel, PipelineSettings settings, IFormatProvider formatProvider)
    {
        SourceModel = sourceModel.IsNotNull(nameof(sourceModel));
        Settings = settings.IsNotNull(nameof(settings));
        FormatProvider = formatProvider.IsNotNull(nameof(formatProvider));
    }

    protected abstract string NewCollectionTypeName { get; }

    public TModel SourceModel { get; }
    public PipelineSettings Settings { get; }
    public IFormatProvider FormatProvider { get; }

    public string CreateArgumentNullException(string argumentName)
    {
        if (Settings.UseExceptionThrowIfNull)
        {
            return $"{typeof(ArgumentNullException).FullName}.ThrowIfNull({argumentName});";
        }

        return $"if ({argumentName} is null) throw new {typeof(ArgumentNullException).FullName}(nameof({argumentName}));";
    }

    public string MapTypeName(string typeName)
        => typeName.IsNotNull(nameof(typeName)).MapTypeName(Settings, NewCollectionTypeName);

    public string MapNamespace(string? ns)
        => ns.MapNamespace(Settings);

    public Domain.Attribute MapAttribute(Domain.Attribute attribute)
    {
        attribute = attribute.IsNotNull(nameof(attribute));

        return new AttributeBuilder(attribute)
            .WithName(MapTypeName(attribute.Name.FixTypeName()))
            .Build();
    }
}
