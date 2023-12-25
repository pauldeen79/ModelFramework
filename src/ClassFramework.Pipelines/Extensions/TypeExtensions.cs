﻿namespace ClassFramework.Pipelines.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<MethodInfo> GetMethodsRecursively(this Type instance)
    {
        var results = new List<MethodInfo>();
        results.AddRange(instance.GetMethods(BindingFlags.Public | BindingFlags.Instance));

        if (instance.BaseType is not null && instance.BaseType != typeof(object))
        {
            results.AddRange(instance.BaseType.GetMethodsRecursively().Where(mi => !results.Contains(mi)));
        }

        results.AddRange(instance.GetInterfaces().SelectMany(i => i.GetMethodsRecursively().Where(mi => !results.Contains(mi))));

        return results;
    }

    public static IEnumerable<PropertyInfo> GetPropertiesRecursively(this Type instance)
    {
        var results = new List<PropertyInfo>();
        results.AddRange(instance.GetProperties(BindingFlags.Public | BindingFlags.Instance));

        if (instance.BaseType is not null)
        {
            results.AddRange(instance.BaseType.GetPropertiesRecursively().Where(pi => !results.Exists(x => x.Name == pi.Name)));
        }

        results.AddRange(instance.GetInterfaces().SelectMany(i => i.GetPropertiesRecursively().Where(pi => !results.Exists(x => x.Name == pi.Name))));

        return results;
    }

    public static IEnumerable<FieldInfo> GetFieldsRecursively(this Type instance)
    {
        var results = new List<FieldInfo>();
        results.AddRange(instance.GetFields(BindingFlags.Public | BindingFlags.Instance));

        if (instance.BaseType is not null)
        {
            results.AddRange(instance.BaseType.GetFieldsRecursively().Where(fi => !results.Exists(x => x.Name == fi.Name)));
        }

        results.AddRange(instance.GetInterfaces().SelectMany(i => i.GetFieldsRecursively().Where(fi => !results.Exists(x => x.Name == fi.Name))));

        return results;
    }

    public static string GetParentTypeFullName(this Type declaringType)
        => declaringType.FullName == "System.Object"
            ? string.Empty
            : declaringType.FullName;

    public static bool IsValueType(this Type type)
        => type.IsValueType || type.IsEnum;
}
