﻿namespace ClassFramework.Domain.Builders.Extensions;

public static partial class TypeBuilderExtensions
{
    public static string GetFullName(this ITypeBuilder instance) => $"{instance.Namespace.GetNamespacePrefix()}{instance.Name}";

    public static T AddInterfaces<T>(this T instance, params Type[] interfaces)
        where T : ITypeBuilder
        => instance.AddInterfaces(interfaces.IsNotNull(nameof(interfaces)).Select(x => x.FullName));

    public static T AddInterfaces<T>(this T instance, IEnumerable<Type> interfaces)
        where T : ITypeBuilder
        => instance.AddInterfaces(interfaces.IsNotNull(nameof(interfaces)).ToArray());
}
