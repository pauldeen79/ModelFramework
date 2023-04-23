namespace ModelFramework.Objects.Extensions;

public static class TypeExtensions
{
    public static IClass ToClass(this Type instance)
        => instance.ToClass(new ClassSettings());

    public static IClass ToClass(this Type instance, ClassSettings settings)
        => instance.ToClassBuilder(settings).BuildTyped();

    public static ClassBuilder ToClassBuilder(this Type instance)
        => instance.ToClassBuilder(new ClassSettings());

    public static ClassBuilder ToClassBuilder(this Type instance, ClassSettings settings)
        => new ClassBuilder()
            .WithName(RemoveGenerics(instance.Name))
            .WithNamespace(instance.FullName.GetNamespaceWithDefault())
            .WithVisibility(instance.IsPublic
                ? Visibility.Public
                : Visibility.Private)
            .WithBaseClass(instance.BaseType == null || instance.BaseType == typeof(object)
                ? string.Empty
                : instance.BaseType.FullName)
            .WithStatic(instance.IsAbstract && instance.IsSealed)
            .WithSealed(instance.IsSealed)
            .WithPartial(settings.Partial)
            .WithRecord(instance.IsRecord())
            .AddInterfaces(GetInterfaces(instance))
            .AddFields(GetFields(instance, settings.AttributeInitializeDelegate).ToList())
            .AddProperties(GetProperties(instance, settings.AttributeInitializeDelegate))
            .AddMethods(GetMethods(instance, settings.AttributeInitializeDelegate))
            .AddConstructors(GetConstructors(instance, settings.AttributeInitializeDelegate, settings.CreateConstructors))
            .AddAttributes(GetAttributes(instance.GetCustomAttributes(false), settings.AttributeInitializeDelegate))
            .AddSubClasses(GetSubClasses(instance, settings.Partial))
            .AddGenericTypeArguments(GetGenericTypeArguments(instance));

    public static IInterface ToInterface(this Type instance)
        => instance.ToInterface(new InterfaceSettings());

    public static IInterface ToInterface(this Type instance, InterfaceSettings settings)
        => instance.ToInterfaceBuilder(settings).BuildTyped();

    public static InterfaceBuilder ToInterfaceBuilder(this Type instance)
        => instance.ToInterfaceBuilder(new InterfaceSettings());

    public static InterfaceBuilder ToInterfaceBuilder(this Type instance, InterfaceSettings settings)
        => new InterfaceBuilder()
            .WithName(RemoveGenerics(instance.Name))
            .WithNamespace(instance.FullName.GetNamespaceWithDefault())
            .WithVisibility(instance.IsPublic
                ? Visibility.Public
                : Visibility.Private)
            .AddInterfaces(GetInterfaces(instance))
            .AddProperties(GetProperties(instance, settings.AttributeInitializeDelegate))
            .AddMethods(GetMethods(instance, settings.AttributeInitializeDelegate))
            .AddGenericTypeArguments(GetGenericTypeArguments(instance));

    public static ITypeBase ToTypeBase(this Type instance)
        => instance.ToTypeBase(new ClassSettings());

    public static ITypeBase ToTypeBase(this Type instance, ClassSettings settings)
        => instance.IsInterface
            ? instance.ToInterface(new InterfaceSettings(attributeInitializeDelegate: settings.AttributeInitializeDelegate))
            : instance.ToClass(settings);

    public static TypeBaseBuilder ToTypeBaseBuilder(this Type instance)
        => instance.ToTypeBaseBuilder(new ClassSettings());

    public static TypeBaseBuilder ToTypeBaseBuilder(this Type instance, ClassSettings settings)
        => instance.IsInterface
            ? instance.ToInterfaceBuilder(new InterfaceSettings(attributeInitializeDelegate: settings.AttributeInitializeDelegate))
            : instance.ToClassBuilder(settings);

    public static IClass ToWrapperClass(this Type instance, WrapperClassSettings settings)
        => instance.ToWrapperClassBuilder(settings).BuildTyped();

    public static ClassBuilder ToWrapperClassBuilder(this Type instance, WrapperClassSettings settings)
        => new ClassBuilder()
            .AddFields(GetWrapperClassFields(instance))
            .AddProperties(GetWrapperClassProperties(instance, settings.PropertyCodeStatementsDelegate))
            .AddMethods(GetWrapperClassMethods(instance, settings.MethodCodeStatementsDelegate))
            .AddConstructors(GetWrapperClassConstructors(instance));

    /// <summary>
    /// Removes generics from a typename. (`1)
    /// </summary>
    /// <param name="typeName">Typename with or without generics</param>
    /// <returns>Typename without generics (`1)</returns>
    public static string WithoutGenerics(this Type instance)
    {
        var name = instance.IsGenericParameter
            ? instance.Name
            : instance.FullName.WhenNullOrEmpty($"{instance.Namespace}.{instance.Name}");
        var index = name.IndexOf('`');
        return index == -1
            ? name.FixTypeName()
            : name.Substring(0, index).FixTypeName();
    }

    public static string GetEntityClassName(this Type instance)
        => RemoveGenerics(instance.IsInterface && instance.Name.StartsWith("I")
            ? instance.Name.Substring(1)
            : instance.Name);

    private static IEnumerable<string> GetInterfaces(Type instance)
        => instance.GetInterfaces()
                   .Where(t => !(instance.IsRecord() && t.FullName.StartsWith("System.IEquatable`1[[" + instance.FullName)))
                   .Select(t => t.GetTypeName(instance));

    private static IEnumerable<ClassFieldBuilder> GetFields(
        Type instance,
        Func<System.Attribute, AttributeBuilder?>? attributeInitializeDelegate)
        => instance.GetFieldsRecursively().Select
            (
                f => new ClassFieldBuilder()
                    .WithName(f.Name)
                    .WithTypeName(f.FieldType.GetTypeName(f))
                    .WithStatic(f.IsStatic)
                    .WithConstant(f.IsLiteral)
                    .WithParentTypeFullName(f.DeclaringType.FullName == "System.Object" ? string.Empty : f.DeclaringType.FullName)
                    .WithIsNullable(f.IsNullable())
                    .WithIsValueType(f.FieldType.IsValueType || f.FieldType.IsEnum)
                    .WithVisibility(f.IsPublic
                        ? Visibility.Public
                        : Visibility.Private)
                    .AddAttributes(GetAttributes(f.GetCustomAttributes(false), attributeInitializeDelegate))
            );

    private static IEnumerable<ClassPropertyBuilder> GetProperties(
        Type instance,
        Func<System.Attribute, AttributeBuilder?>? attributeInitializeDelegate)
        => instance.GetPropertiesRecursively().Select
        (
            p => new ClassPropertyBuilder()
                .WithName(p.Name)
                .WithTypeName(p.PropertyType.GetTypeName(p))
                .WithHasGetter(p.GetGetMethod() != null)
                .WithHasSetter(p.GetSetMethod() != null)
                .WithHasInitializer(p.IsInitOnly())
                .WithParentTypeFullName(p.DeclaringType.FullName == "System.Object" ? string.Empty : p.DeclaringType.FullName.WithoutGenerics())
                .WithIsNullable(p.IsNullable())
                .WithIsValueType(p.PropertyType.IsValueType || p.PropertyType.IsEnum)
                .WithVisibility(p.GetAccessors().Any(m => m.IsPublic)
                    ? Visibility.Public
                    : Visibility.Private)
                .WithGetterVisibility(p.GetGetMethod()?.IsPublic ?? false
                    ? Visibility.Public
                    : Visibility.Private)
                .WithSetterVisibility(p.GetSetMethod()?.IsPublic ?? false
                    ? Visibility.Public
                    : Visibility.Private)
                .WithInitializerVisibility(p.GetSetMethod()?.IsPublic ?? false
                    ? Visibility.Public
                    : Visibility.Private)
                .AddAttributes(GetAttributes(p.GetCustomAttributes(false), attributeInitializeDelegate))
        );

    private static IEnumerable<ClassMethodBuilder> GetMethods(
        Type instance,
        Func<System.Attribute, AttributeBuilder?>? attributeInitializeDelegate)
        => instance.GetMethodsRecursively()
                .Where(m => !instance.IsRecord() && !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.Name != "GetType")
                .Select
                (
                    m => new ClassMethodBuilder()
                        .WithName(m.Name)
                        .WithTypeName(m.ReturnType.GetTypeName(m))
                        .WithVisibility(m.IsPublic
                            ? Visibility.Public
                            : Visibility.Private)
                        .WithStatic(m.IsStatic)
                        .WithVirtual(m.IsVirtual)
                        .WithAbstract(m.IsAbstract)
                        .WithParentTypeFullName(m.DeclaringType.FullName == "System.Object" ? string.Empty : m.DeclaringType.FullName)
                        .WithIsNullable(m.ReturnTypeIsNullable())
                        .WithIsValueType(m.ReturnType.IsValueType || m.ReturnType.IsEnum)
                        .AddParameters(m.GetParameters().Select
                        (
                            p => new ParameterBuilder()
                                .WithName(p.Name)
                                .WithTypeName(p.ParameterType.GetTypeName(m))
                                .WithIsNullable(p.IsNullable())
                                .WithIsValueType(p.ParameterType.IsValueType || p.ParameterType.IsEnum)
                                .AddAttributes(GetAttributes(p.GetCustomAttributes(true), attributeInitializeDelegate))
                        ))
                        .AddAttributes(GetAttributes(m.GetCustomAttributes(false), attributeInitializeDelegate))
                );

    private static IEnumerable<ClassConstructorBuilder> GetConstructors(
        Type instance,
        Func<System.Attribute, AttributeBuilder?>? attributeInitializeDelegate,
        bool createConstructors)
        => instance.GetConstructors()
            .Where(_ => createConstructors)
            .Select(x => new ClassConstructorBuilder()
                .AddParameters
                (
                    x.GetParameters().Select
                    (
                        p =>
                        new ParameterBuilder()
                            .WithName(p.Name)
                            .WithTypeName(p.ParameterType.FullName.FixTypeName())
                            .WithIsNullable(p.IsNullable())
                            .WithIsValueType(p.ParameterType.IsValueType || p.ParameterType.IsEnum)
                            .AddAttributes(GetAttributes(p.GetCustomAttributes(true), attributeInitializeDelegate))
                    )
                )
        );

    private static IEnumerable<AttributeBuilder> GetAttributes(
        object[] attributes,
        Func<System.Attribute, AttributeBuilder?>? initializeDelegate)
        => attributes.OfType<System.Attribute>().Where(x => x.GetType().FullName != "System.Runtime.CompilerServices.NullableContextAttribute"
                                                            && x.GetType().FullName != "System.Runtime.CompilerServices.NullableAttribute")
                     .Select(x => new AttributeBuilder(x, initializeDelegate));

    private static IEnumerable<ClassBuilder> GetSubClasses(Type instance, bool partial)
        => instance.GetNestedTypes().Select(t => t.ToClassBuilder(new ClassSettings(partial: partial)));

    private static IEnumerable<ClassPropertyBuilder> GetWrapperClassProperties(Type type,
                                                                               Func<PropertyInfo, IEnumerable<ICodeStatement>>? propertyCodeStatementsDelegate = null)
        => type.GetPropertiesRecursively()
               .Where(p => p.GetGetMethod() != null && p.GetSetMethod() == null)
               .Select(p => CreateMockProperty(p, propertyCodeStatementsDelegate));

    private static IEnumerable<ClassMethodBuilder> GetWrapperClassMethods(Type type,
                                                                          Func<MethodInfo, IEnumerable<ICodeStatement>>? methodCodeStatementsDelegate = null)
        => type.GetMethodsRecursively()
               .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.Name != "GetType")
               .Select(m => CreateMockMethod(m, methodCodeStatementsDelegate));

    private static IEnumerable<ClassConstructorBuilder> GetWrapperClassConstructors(Type type)
    {
        yield return new ClassConstructorBuilder()
            .AddParameters
            (
                new ParameterBuilder()
                    .WithName("wrappedInstance")
                    .WithTypeName(type.FullName.FixTypeName())
            )
            .AddLiteralCodeStatements("_wrappedInstance = wrappedInstance;");
    }

    private static IEnumerable<ClassFieldBuilder> GetWrapperClassFields(Type type)
    {
        yield return new ClassFieldBuilder()
            .WithName("_wrappedInstance")
            .WithTypeName(type.FullName.FixTypeName())
            .WithVisibility(Visibility.Private)
            .WithReadOnly(true);
    }

    private static ClassPropertyBuilder CreateMockProperty(PropertyInfo propertyInfo,
                                                           Func<PropertyInfo, IEnumerable<ICodeStatement>>? propertyCodeStatementsDelegate = null)
        => new ClassPropertyBuilder()
            .WithName(propertyInfo.Name)
            .WithTypeName(propertyInfo.PropertyType.FullName.FixTypeName())
            .AsReadOnly()
            .AddGetterCodeStatements((propertyCodeStatementsDelegate?.Invoke(propertyInfo) ?? Enumerable.Empty<ICodeStatement>()).Select(x => x.CreateBuilder()));

    private static ClassMethodBuilder CreateMockMethod(MethodInfo methodInfo,
                                                       Func<MethodInfo, IEnumerable<ICodeStatement>>? methodCodeStatementsDelegate = null)
        => new ClassMethodBuilder()
            .WithName(methodInfo.Name)
            .WithTypeName(methodInfo.ReturnType.FullName.FixTypeName())
            .AddParameters(CreateParameters(methodInfo))
            .WithOverride(methodInfo.DeclaringType == typeof(object))
            .AddCodeStatements((methodCodeStatementsDelegate?.Invoke(methodInfo) ?? Enumerable.Empty<ICodeStatement>()).Select(x => x.CreateBuilder()));

    private static IEnumerable<ParameterBuilder> CreateParameters(MethodInfo methodInfo)
        => methodInfo.GetParameters()
            .Select(p => new ParameterBuilder()
                .WithName(p.Name)
                .WithTypeName(p.ParameterType.FullName.FixTypeName()));

    private static IEnumerable<MethodInfo> GetMethodsRecursively(this Type instance)
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

    private static IEnumerable<PropertyInfo> GetPropertiesRecursively(this Type instance)
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
                if (!results.Any(x => x.Name == pi.Name))
                {
                    results.Add(pi);
                }
            }
        }

        foreach (var i in instance.GetInterfaces())
        {
            foreach (var pi in i.GetPropertiesRecursively())
            {
                if (!results.Any(x => x.Name == pi.Name))
                {
                    results.Add(pi);
                }
            }
        }

        return results;
    }

    private static IEnumerable<FieldInfo> GetFieldsRecursively(this Type instance)
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
                if (!results.Any(x => x.Name == fi.Name))
                {
                    results.Add(fi);
                }
            }
        }

        foreach (var i in instance.GetInterfaces())
        {
            foreach (var fi in i.GetFieldsRecursively())
            {
                if (!results.Any(x => x.Name == fi.Name))
                {
                    results.Add(fi);
                }
            }
        }

        return results;
    }

    private static IEnumerable<string> GetGenericTypeArguments(Type instance)
        => ((TypeInfo)instance).GenericTypeParameters.Select(x => x.Name);

    private static bool IsRecord(this Type type)
        => type.GetMethod("<Clone>$") != null;

    public static string GetTypeName(this Type type, MemberInfo declaringType)
    {
        if (!type.IsGenericType)
        {
            return type.FullName.FixTypeName().WhenNullOrEmpty(() => type.Name);
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
            if (!arg.IsGenericParameter && !builder.ToString().StartsWith("System.Collections.Generic.IReadOnlyCollection") && NullableHelper.IsNullable(arg, arg, declaringType.CustomAttributes, index))
            {
                builder.Append("?");
            }
            if (arg.IsGenericParameter && NullableHelper.IsNullable(arg, declaringType, declaringType.CustomAttributes, index))
            {
                builder.Append("?");
            }
        }
        builder.Append(">");
        return builder.ToString();
    }

    private static string RemoveGenerics(string name)
    {
        var index = name.IndexOf("`");
        if (index == -1)
        {
            return name;
        }

        return name.Substring(0, index);
    }
}
