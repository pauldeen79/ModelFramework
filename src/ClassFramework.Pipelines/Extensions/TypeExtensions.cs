namespace ClassFramework.Pipelines.Extensions;

public static class TypeExtensions
{
    public static IEnumerable<MethodInfo> GetMethodsRecursively(this Type instance)
    {
        var results = new List<MethodInfo>();
        foreach (var mi in instance.GetMethods(BindingFlags.Public | BindingFlags.Instance))
        {
            results.Add(mi);
        }

        if (instance.BaseType != null && instance.BaseType != typeof(object))
        {
            foreach (var mi in instance.BaseType.GetMethodsRecursively())
            {
                if (!results.Contains(mi))
                {
                    results.Add(mi);
                }
            }
        }

        foreach (var i in instance.GetInterfaces())
        {
            foreach (var mi in i.GetMethodsRecursively())
            {
                if (!results.Contains(mi))
                {
                    results.Add(mi);
                }
            }
        }

        return results;
    }

    public static IEnumerable<PropertyInfo> GetPropertiesRecursively(this Type instance)
    {
        var results = new List<PropertyInfo>();
        foreach (var pi in instance.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            results.Add(pi);
        }

        if (instance.BaseType != null)
        {
            foreach (var pi in instance.BaseType.GetPropertiesRecursively())
            {
                if (!results.Exists(x => x.Name == pi.Name))
                {
                    results.Add(pi);
                }
            }
        }

        foreach (var i in instance.GetInterfaces())
        {
            foreach (var pi in i.GetPropertiesRecursively())
            {
                if (!results.Exists(x => x.Name == pi.Name))
                {
                    results.Add(pi);
                }
            }
        }

        return results;
    }

    public static IEnumerable<FieldInfo> GetFieldsRecursively(this Type instance)
    {
        var results = new List<FieldInfo>();
        foreach (var fi in instance.GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            results.Add(fi);
        }

        if (instance.BaseType != null)
        {
            foreach (var fi in instance.BaseType.GetFieldsRecursively())
            {
                if (!results.Exists(x => x.Name == fi.Name))
                {
                    results.Add(fi);
                }
            }
        }

        foreach (var i in instance.GetInterfaces())
        {
            foreach (var fi in i.GetFieldsRecursively())
            {
                if (!results.Exists(x => x.Name == fi.Name))
                {
                    results.Add(fi);
                }
            }
        }

        return results;
    }
}
