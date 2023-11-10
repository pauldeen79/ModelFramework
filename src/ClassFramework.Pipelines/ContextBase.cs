namespace ClassFramework.Pipelines;

public abstract record ContextBase<TModel, TSettings>
{
    protected ContextBase(TModel sourceModel, TSettings settings, IFormatProvider formatProvider)
    {
        SourceModel = sourceModel.IsNotNull(nameof(sourceModel));
        Settings = settings.IsNotNull(nameof(settings));
        FormatProvider = formatProvider.IsNotNull(nameof(formatProvider));
    }

    public TModel SourceModel { get; }
    public TSettings Settings { get; }
    public IFormatProvider FormatProvider { get; }
}
