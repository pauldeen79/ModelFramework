﻿namespace ClassFramework.Domain.Extensions;

public static class TypeExtensions
{
    /// <summary>
    /// Removes generics from a typename. (`1)
    /// </summary>
    /// <param name="typeName">Typename with or without generics</param>
    /// <returns>Typename without generics (`1)</returns>
    public static string WithoutGenerics(this Type instance)
    {
        var name = instance.IsGenericParameter
            ? instance.Name
            : instance.FullName.WhenNullOrEmpty(() => $"{instance.Namespace}.{instance.Name}");
        var index = name.IndexOf('`');

        return index == -1
            ? name.FixTypeName()
            : name.Substring(0, index).FixTypeName();
    }

    public static string ReplaceGenericTypeName(this Type instance, Type genericArguments)
        => instance.WithoutGenerics().MakeGenericTypeName(genericArguments.IsNotNull(nameof(genericArguments)).FullName);

    public static string ReplaceGenericTypeName(this Type instance, string genericArgumentsTypeName)
        => instance.WithoutGenerics().MakeGenericTypeName(genericArgumentsTypeName.IsNotNull(nameof(genericArgumentsTypeName)));

    public static string GetTypeName(this Type type, MemberInfo declaringType)
    {
        declaringType = declaringType.IsNotNull(nameof(declaringType));

        if (!type.IsGenericType)
        {
            return type.FullName.FixTypeName().WhenNullOrEmpty(() => type.Name);
        }

        var typeName = type.FullName.FixTypeName();
        if (typeName.IsCollectionTypeName())
        {
            // for now, we will ignore nullability of the generic argument on generic lists
            return typeName.ReplaceGenericTypeName(typeName.GetGenericArguments());
        }

        var builder = new StringBuilder();
        builder.Append(type.WithoutGenerics());
        builder.Append("<");
        var first = true;
        var index = 0;
        foreach (var arg in type.GetGenericArguments())
        {
            if (first)
            {
                first = false;
            }
            else
            {
                builder.Append(",");
            }

            index++;
            builder.Append(arg.GetTypeName(type));
            if ((!arg.IsGenericParameter && NullableHelper.IsNullable(arg, arg, declaringType.CustomAttributes, index))
                || (arg.IsGenericParameter && NullableHelper.IsNullable(arg, declaringType, declaringType.CustomAttributes, index)))
            {
                builder.Append("?");
            }
        }
        builder.Append(">");
        return builder.ToString();
    }

    public static string GetFullName(this IType type) => $"{type.Namespace.GetNamespacePrefix()}{type.Name}";

    public static bool IsRecord(this Type type)
        => type.GetMethod("<Clone>$") != null;
}
