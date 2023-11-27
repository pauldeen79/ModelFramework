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

    public string GetInheritedClasses()
        => Data is Class cls
            ? GetInheritedClassesForClass(cls)
            : GetInheritedClassesForTypeBase();

    public string GetContainerType()
    {
        if (Data is Class cls)
        {
            return cls.Record
                ? "record"
                : "class";
        }

        if (Data is Struct str)
        {
            return str.Record
                ? "record struct"
                : "struct";
        }

        if (Data is Interface)
        {
            return "interface";
        }

        throw new InvalidOperationException($"Unknown container type: [{Data.GetType().FullName}]");
    }

    private static string GetInheritedClassesForClass(Class cls)
    {
        var lst = new List<string>();
        if (!string.IsNullOrEmpty(cls.BaseClass))
        {
            lst.Add(cls.BaseClass);
        }

        lst.AddRange(cls.Interfaces);

        return lst.Count == 0
            ? string.Empty
            : $" : {string.Join(", ", lst.Select(x => x.GetCsharpFriendlyTypeName()))}";
    }

    private string GetInheritedClassesForTypeBase()
        => Data.Interfaces.Count == 0
            ? string.Empty
            : $" : {string.Join(", ", Data.Interfaces.Select(x => x.GetCsharpFriendlyTypeName()))}";
}
