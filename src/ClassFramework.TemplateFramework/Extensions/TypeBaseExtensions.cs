namespace ClassFramework.TemplateFramework.Extensions;

public static class TypeBaseExtensions
{
    public static string GetInheritedClasses(this TypeBase instance)
        => instance is Class cls
            ? GetInheritedClassesForClass(cls)
            : GetInheritedClassesForTypeBase(instance);

    public static string GetContainerType(this TypeBase type)
    {
        if (type is Class cls)
        {
            return cls.Record
                ? "record"
                : "class";
        }

        if (type is Struct str)
        {
            return str.Record
                ? "record struct"
                : "struct";
        }

        if (type is Interface)
        {
            return "interface";
        }

        throw new InvalidOperationException($"Unknown container type: [{type.GetType().FullName}]");
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

    private static string GetInheritedClassesForTypeBase(TypeBase instance)
        => instance.Interfaces.Count == 0
            ? string.Empty
            : $" : {string.Join(", ", instance.Interfaces.Select(x => x.GetCsharpFriendlyTypeName()))}";
}
