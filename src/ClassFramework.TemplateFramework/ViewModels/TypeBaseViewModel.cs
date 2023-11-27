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

    public string GetContainerType()
        => Data switch
        {
            Class cls => cls.Record
                ? "record"
                : "class",
            Struct str => str.Record
                ? "record struct"
                : "struct",
            Interface => "interface",
            _ => throw new InvalidOperationException($"Unknown container type: [{Data.GetType().FullName}]")
        };

    public string GetInheritedClasses()
    {
        var lst = new List<string>();

        var baseClassContainer = Data as IBaseClassContainer;
        if (!string.IsNullOrEmpty(baseClassContainer?.BaseClass))
        {
            lst.Add(baseClassContainer.BaseClass);
        }

        lst.AddRange(Data.Interfaces);

        return lst.Count == 0
            ? string.Empty
            : $" : {string.Join(", ", lst.Select(x => x.GetCsharpFriendlyTypeName()))}";
    }
}
