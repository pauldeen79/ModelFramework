namespace ClassFramework.Pipelines.BuilderInterface;

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

    public string CreateArgumentNullException(string argumentName)
    {
        if (Settings.EntitySettings.NullCheckSettings.UseExceptionThrowIfNull)
        {
            return $"{typeof(ArgumentNullException).FullName}.ThrowIfNull({argumentName});";
        }

        return $"if ({argumentName} is null) throw new {typeof(ArgumentNullException).FullName}(nameof({argumentName}));";
    }
}
