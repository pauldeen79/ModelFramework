namespace ClassFramework.Pipelines;

public abstract record ContextBase<TModel, TSettings>
{
    protected ContextBase(TModel model, TSettings settings, IFormatProvider formatProvider)
    {
        Model = model.IsNotNull(nameof(model));
        Settings = settings.IsNotNull(nameof(settings));
        FormatProvider = formatProvider.IsNotNull(nameof(formatProvider));
    }

    public TModel Model { get; }
    public TSettings Settings { get; }
    public IFormatProvider FormatProvider { get; }
}
