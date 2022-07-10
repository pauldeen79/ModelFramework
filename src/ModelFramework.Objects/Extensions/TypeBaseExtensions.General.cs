namespace ModelFramework.Objects.Extensions;

public static partial class TypeBaseExtensions
{
    public static string GetInheritedClasses(this ITypeBase instance)
        => instance is IClass cls
            ? GetInheritedClassesForClass(cls)
            : GetInheritedClassesForTypeBase(instance);

    public static string GetContainerType(this ITypeBase type)
    {
        if (type is IClass cls)
        {
            return cls.Record
                ? "record"
                : "class";
        }
        if (type is IInterface)
        {
            return "interface";
        }

        throw new InvalidOperationException($"Unknown container type: [{type.GetType().FullName}]");
    }

    public static string GetGenericTypeArgumentsString(this ITypeBase instance)
        => instance.GenericTypeArguments.Count > 0
            ? $"<{string.Join(", ", instance.GenericTypeArguments)}>"
            : string.Empty;

    public static string GetGenericTypeArgumentConstraintsString(this ITypeBase instance)
        => instance.GenericTypeArgumentConstraints.Any()
            ? string.Concat(Environment.NewLine,
                            "        ",
                            string.Join(string.Concat(Environment.NewLine, "        "), instance.GenericTypeArgumentConstraints))
            : string.Empty;

    public static string GetFullName(this ITypeBase instance)
        => instance.GetNamespacePrefix() + instance.Name;

    public static string GetNamespacePrefix(this ITypeBase instance)
        => string.IsNullOrEmpty(instance.Namespace)
            ? string.Empty
            : instance.Namespace + ".";

    public static IEnumerable<IClassField> GetFields(this ITypeBase instance)
        => (instance as IClass)?.Fields ?? Enumerable.Empty<IClassField>();

    public static IClass ToClass(this ITypeBase instance)
        => instance is IClass c
            ? c
            : new Class
                (
                    Enumerable.Empty<IClassField>(),
                    false,
                    false,
                    false,
                    Enumerable.Empty<IClass>(),
                    Enumerable.Empty<IClassConstructor>(),
                    string.Empty,
                    false,
                    instance.Namespace,
                    instance.Partial,
                    instance.Interfaces,
                    instance.Properties,
                    instance.Methods,
                    instance.GenericTypeArguments,
                    instance.GenericTypeArgumentConstraints,
                    instance.Metadata,
                    instance.Visibility,
                    instance.Name,
                    instance.Attributes,
                    Enumerable.Empty<IEnum>()
                );

    public static IEnumerable<string> GetNamespacesToAbbreviate(this ITypeBase instance)
        => instance.Metadata.GetStringValues(MetadataNames.NamespaceToAbbreviate);

    public static bool IsPoco(this ITypeBase instance)
        => (!instance.Properties.Any() || instance.Properties.All(p => p.HasSetter || p.HasInitializer))
            && (!(instance is IClass) || instance is IClass cls && cls.HasPublicParameterlessConstructor());

    public static string GetCustomValueForInheritedClass(this ITypeBase instance, ImmutableBuilderClassSettings settings, Func<IClass, string> customValue)
        => instance.GetCustomValueForInheritedClass(settings.InheritanceSettings.EnableInheritance, customValue);

    public static string GetCustomValueForInheritedClass(this ITypeBase instance, ImmutableClassSettings settings, Func<IClass, string> customValue)
        => instance.GetCustomValueForInheritedClass(settings.InheritanceSettings.EnableInheritance, customValue);

    public static string GetCustomValueForInheritedClass(this ITypeBase instance, bool enableInheritance, Func<IClass, string> customValue)
    {
        if (!enableInheritance)
        {
            // Inheritance is not enabled
            return string.Empty;
        }

        var cls = instance as IClass;

        if (cls == null)
        {
            // Type is an interface
            return string.Empty;
        }

        if (string.IsNullOrEmpty(cls.BaseClass))
        {
            // Class is not inherited
            return string.Empty;
        }

        return customValue(cls);
    }

    private static string GetInheritedClassesForClass(IClass cls)
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

    private static string GetInheritedClassesForTypeBase(ITypeBase instance)
        => !instance.Interfaces.Any()
            ? string.Empty
            : $" : {string.Join(", ", instance.Interfaces.Select(x => x.GetCsharpFriendlyTypeName()))}";
}
