using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModelFramework.Common.Contracts;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;

namespace ModelFramework.Objects.Extensions
{
    public static class TypeExtensions
    {
        public static IClass ToClass(this Type instance,
                                     string name = null,
                                     string @namespace = null,
                                     bool partial = false,
                                     bool createConstructors = false,
                                     bool autoGenerateInterface = false,
                                     bool? record = null)
            => new Class
            (
                name ?? instance.Name,
                @namespace ?? instance.FullName.GetNamespaceWithDefault(null),
                instance.IsPublic
                    ? Visibility.Public
                    : Visibility.Private,
                instance.BaseType == null || instance.BaseType == typeof(object)
                    ? null
                    : instance.BaseType.FullName,
                instance.IsAbstract && instance.IsSealed,
                instance.IsSealed,
                partial,
                autoGenerateInterface,
                record ?? instance.IsRecord(),
                GetInterfaces(instance),
                GetFields(instance),
                GetProperties(instance),
                GetMethods(instance),
                GetConstructors(instance, createConstructors),
                Array.Empty<IMetadata>(),
                GetAttributes(instance.GetCustomAttributes(false)),
                GetSubClasses(instance, partial),
                Array.Empty<IEnum>()
            );

        private static IEnumerable<string> GetInterfaces(Type instance)
            => instance.GetInterfaces().Where(t => !(instance.IsRecord() && t.FullName.StartsWith("System.IEquatable`1[[" + instance.FullName))).Select(t => t.FullName);

        private static IEnumerable<IClassField> GetFields(Type instance)
            => instance.GetFieldsRecursively().Select
                (
                    f =>
                    new ClassField
                    (
                        f.Name,
                        f.FieldType.FullName,
                        f.IsStatic,
                        f.IsLiteral,
                        false,
                        false,
                        false,
                        false,
                        false,
                        false,
                        null,
                        f.IsPublic
                            ? Visibility.Public
                            : Visibility.Private,
                        null,
                        GetAttributes(f.GetCustomAttributes(false))
                    )
                );

        private static IEnumerable<IClassProperty> GetProperties(Type instance) =>
            instance.GetPropertiesRecursively().Select
            (
                p =>
                new ClassProperty
                (
                    p.Name,
                    p.PropertyType.FullName,
                    false,
                    false,
                    false,
                    false,
                    false,
                    p.GetGetMethod() != null,
                    p.GetSetMethod() != null,
                    p.IsInitOnly(),
                    p.GetAccessors().Any(m => m.IsPublic)
                        ? Visibility.Public
                        : Visibility.Private,
                    p.GetGetMethod()?.IsPublic ?? false
                        ? Visibility.Public
                        : Visibility.Private,
                    p.GetSetMethod()?.IsPublic ?? false
                        ? Visibility.Public
                        : Visibility.Private,
                    Visibility.Public,
                    null,
                    null,
                    null,
                    null,
                    null, //metadata
                    GetAttributes(p.GetCustomAttributes(false)),
                    null, //getter code statements
                    null, //setter code statements
                    null //init code statements
                )
            );

        private static IEnumerable<IClassMethod> GetMethods(Type instance)
            => instance.GetMethodsRecursively()
                    .Where(m => !instance.IsRecord() && !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.Name != "GetType")
                    .Select
                    (
                        m =>
                        new ClassMethod
                        (
                            m.Name,
                            m.ReturnType.FullName.FixTypeName(),
                            m.IsPublic
                                ? Visibility.Public
                                : Visibility.Private,
                            m.IsStatic,
                            m.IsVirtual,
                            m.IsAbstract,
                            false,
                            false,
                            false, //override
                            false, //extension method
                            false, //operator
                            null, //body
                            null, //explicit interface name
                            m.GetParameters().Select
                            (
                                p =>
                                new Parameter
                                (
                                    p.Name,
                                    p.ParameterType.FullName.FixTypeName(),
                                    null,
                                    GetAttributes(p.GetCustomAttributes(true)),
                                    null //metadata
                                )
                            ),
                            GetAttributes(m.GetCustomAttributes(false)), //attributes
                            null, //code statements
                            null //metadata
                        )
                    );

        private static IEnumerable<IClassConstructor> GetConstructors(Type instance, bool createConstructors)
            => instance.GetConstructors().Where(_ => createConstructors).Select(x => new ClassConstructor
            (
                parameters: x.GetParameters().Select
                (
                    p =>
                    new Parameter
                    (
                        p.Name,
                        p.ParameterType.FullName.FixTypeName(),
                        null,
                        GetAttributes(p.GetCustomAttributes(true)),
                        null //metadata
                    )
                )
            ));

        private static IEnumerable<IAttribute> GetAttributes(object[] attributes)
            => attributes.OfType<System.Attribute>().Select(x => new Default.Attribute
            (
                x.GetType().FullName
            ));

        private static IEnumerable<IClass> GetSubClasses(Type instance, bool partial)
            => instance.GetNestedTypes().Select(t => t.ToClass(null, null, partial));

#pragma warning disable S107 // Methods should not have too many parameters
        public static IClass ToWrapperClass(this Type type,
                                            string name,
                                            string @namespace,
                                            Visibility visibility = Visibility.Public,
                                            string baseClass = null,
                                            bool @static = false,
                                            bool @sealed = false,
                                            bool partial = false,
                                            bool autoGenerateInterface = false,
                                            bool record = false,
                                            IEnumerable<string> interfaces = null,
                                            IEnumerable<IClassField> fields = null,
                                            IEnumerable<IClassProperty> properties = null,
                                            IEnumerable<IClassMethod> methods = null,
                                            IEnumerable<IClassConstructor> constructors = null,
                                            IEnumerable<IMetadata> metadata = null,
                                            IEnumerable<IAttribute> attributes = null,
                                            IEnumerable<IClass> subClasses = null,
                                            IEnumerable<IEnum> enums = null,
                                            Func<MethodInfo, IEnumerable<ICodeStatement>> methodCodeStatementsDelegate = null,
                                            Func<PropertyInfo, IEnumerable<ICodeStatement>> propertyCodeStatementsDelegate = null
        ) => new Class(name.WhenNullOrEmpty(type.Name),
#pragma warning restore S107 // Methods should not have too many parameters
                       type.FullName.GetNamespaceWithDefault(@namespace),
                       visibility,
                       baseClass,
                       @static,
                       @sealed,
                       partial,
                       autoGenerateInterface,
                       record,
                       interfaces,
                       GeneratedFields(type).Concat(fields ?? Enumerable.Empty<IClassField>()),
                       GeneratedProperties(type, propertyCodeStatementsDelegate).Concat(properties ?? Enumerable.Empty<IClassProperty>()),
                       GeneratedMethods(type, methodCodeStatementsDelegate).Concat(methods ?? Enumerable.Empty<IClassMethod>()),
                       GeneratedCtors(type).Concat(constructors ?? Enumerable.Empty<IClassConstructor>()),
                       metadata,
                       attributes,
                       subClasses,
                       enums);

        private static IEnumerable<IClassProperty> GeneratedProperties(Type type, Func<PropertyInfo, IEnumerable<ICodeStatement>> propertyCodeStatementsDelegate = null)
            => type.GetPropertiesRecursively()
                   .Where(p => p.GetGetMethod() != null && p.GetSetMethod() == null)
                   .Select(p => CreateMockProperty(p, propertyCodeStatementsDelegate));

        private static IEnumerable<IClassMethod> GeneratedMethods(Type type, Func<MethodInfo, IEnumerable<ICodeStatement>> methodCodeStatementsDelegate = null)
            => type.GetMethodsRecursively()
                   .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.Name != "GetType")
                   .Select(m => CreateMockMethod(m, methodCodeStatementsDelegate));

        private static IEnumerable<IClassConstructor> GeneratedCtors(Type type)
        {
            yield return new ClassConstructor(parameters: new[] { new Parameter("wrappedInstance", type.FullName.FixTypeName()) }, body: "_wrappedInstance = wrappedInstance;");
        }

        private static IEnumerable<IClassField> GeneratedFields(Type type)
        {
            yield return new ClassField("_wrappedInstance", type.FullName.FixTypeName(), visibility: Visibility.Private, readOnly: true);
        }

        private static IClassProperty CreateMockProperty(PropertyInfo propertyInfo, Func<PropertyInfo, IEnumerable<ICodeStatement>> propertyCodeStatementsDelegate = null)
            => new ClassProperty(propertyInfo.Name,
                                 propertyInfo.PropertyType.FullName.FixTypeName(),
                                 hasSetter: false,
                                 getterCodeStatements: propertyCodeStatementsDelegate?.Invoke(propertyInfo));

        private static IClassMethod CreateMockMethod(MethodInfo methodInfo, Func<MethodInfo, IEnumerable<ICodeStatement>> methodCodeStatementsDelegate = null)
            => new ClassMethod(methodInfo.Name,
                               methodInfo.ReturnType.FullName.FixTypeName(),
                               parameters: CreateParameters(methodInfo),
                               @override: methodInfo.DeclaringType == typeof(object),
                               codeStatements: methodCodeStatementsDelegate?.Invoke(methodInfo));

        private static IEnumerable<IParameter> CreateParameters(MethodInfo methodInfo)
            => methodInfo.GetParameters().Select(p => new Parameter(p.Name, p.ParameterType.FullName.FixTypeName()));

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

            foreach(var i in instance.GetInterfaces())
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

        private static bool IsRecord(this Type type)
            => type.GetMethod("<Clone>$") != null;
    }
}
