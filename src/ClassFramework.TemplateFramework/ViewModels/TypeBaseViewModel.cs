namespace ClassFramework.TemplateFramework.ViewModels;

public class TypeBaseViewModel : CsharpClassGeneratorViewModel<TypeBase>
{
    public TypeBaseViewModel(TypeBase data, CsharpClassGeneratorSettings settings) : base(data, settings)
    {
    }

    public bool ShouldRenderNullablePragmas
        => Settings.EnableNullableContext && Settings.IndentCount == 1; // note: only for root level, because it gets rendered in the same file

    public bool ShouldRenderNamespaceScope
        => Settings.GenerateMultipleFiles && !string.IsNullOrEmpty(Data.Namespace);
}
