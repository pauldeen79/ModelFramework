using System.Reflection;

namespace ClassFramework.TemplateFramework;

public sealed class CsharpClassGeneratorViewModel<TModel>
{
    public CsharpClassGeneratorViewModel(TModel data, CsharpClassGeneratorSettings settings)
    {
        Data = data;
        Settings = settings;
    }

    public TModel Data { get; }
    public CsharpClassGeneratorSettings Settings { get; }

    public bool ShouldRenderNullablePragmas => Settings.EnableNullableContext && Settings.IndentCount == 1; // note: only for root level, because it gets rendered in the same file
}
