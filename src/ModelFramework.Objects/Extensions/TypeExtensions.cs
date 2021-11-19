using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Settings;

namespace ModelFramework.Objects.Extensions
{
    public static class TypeExtensions
    {
        public static ClassBuilder ToClass(this Type instance, ClassSettings settings)
            => new ClassBuilder
            {
                Name = instance.Name,
                Namespace = instance.FullName.GetNamespaceWithDefault(null),
                Visibility = instance.IsPublic
                    ? Visibility.Public
                    : Visibility.Private,
                BaseClass = instance.BaseType == null || instance.BaseType == typeof(object)
                    ? null
                    : instance.BaseType.FullName,
                Static = instance.IsAbstract && instance.IsSealed,
                Sealed = instance.IsSealed,
                Partial = settings.Partial,
                AutoGenerateInterface = settings.AutoGenerateInterface,
                AutoGenerateInterfaceSettings = settings.AutoGenerateInterfaceSettings == null
                    ? null
                    : new InterfaceSettingsBuilder(settings.AutoGenerateInterfaceSettings),
                Record = instance.IsRecord(),
                Interfaces = GetInterfaces(instance).ToList(),
                Fields = GetFields(instance).ToList(),
                Properties = GetProperties(instance).ToList(),
                Methods = GetMethods(instance).ToList(),
                Constructors = GetConstructors(instance, settings.CreateConstructors).ToList(),
                Attributes = GetAttributes(instance.GetCustomAttributes(false)).ToList(),
                SubClasses = GetSubClasses(instance, settings.Partial).ToList()
            };

        public static ClassBuilder ToWrapperClass(this Type type, WrapperClassSettings settings)
            => new ClassBuilder()
                .AddFields(GeneratedFields(type))
                .AddProperties(GeneratedProperties(type, settings.PropertyCodeStatementsDelegate))
                .AddMethods(GeneratedMethods(type, settings.MethodCodeStatementsDelegate))
                .AddConstructors(GeneratedCtors(type));

        private static IEnumerable<string> GetInterfaces(Type instance)
            => instance.GetInterfaces().Where(t => !(instance.IsRecord() && t.FullName.StartsWith("System.IEquatable`1[[" + instance.FullName))).Select(t => t.FullName);

        private static IEnumerable<ClassFieldBuilder> GetFields(Type instance)
            => instance.GetFieldsRecursively().Select
                (
                    f => new ClassFieldBuilder
                    {
                        Name = f.Name,
                        TypeName = f.FieldType.FullName,
                        Static = f.IsStatic,
                        Constant = f.IsLiteral,
                        IsNullable = f.IsNullable(),
                        Visibility = f.IsPublic
                            ? Visibility.Public
                            : Visibility.Private,
                        Attributes = GetAttributes(f.GetCustomAttributes(false)).ToList()
                    }
                );

        private static IEnumerable<ClassPropertyBuilder> GetProperties(Type instance) =>
            instance.GetPropertiesRecursively().Select
            (
                p => new ClassPropertyBuilder
                {
                    Name = p.Name,
                    TypeName = p.PropertyType.FullName,
                    HasGetter = p.GetGetMethod() != null,
                    HasSetter = p.GetSetMethod() != null,
                    HasInit = p.IsInitOnly(),
                    IsNullable = p.IsNullable(),
                    Visibility = p.GetAccessors().Any(m => m.IsPublic)
                        ? Visibility.Public
                        : Visibility.Private,
                    GetterVisibility = p.GetGetMethod()?.IsPublic ?? false
                        ? Visibility.Public
                        : Visibility.Private,
                    SetterVisibility = p.GetSetMethod()?.IsPublic ?? false
                        ? Visibility.Public
                        : Visibility.Private,
                    InitializerVisibility = p.GetSetMethod()?.IsPublic ?? false
                        ? Visibility.Public
                        : Visibility.Private,
                    Attributes = GetAttributes(p.GetCustomAttributes(false)).ToList()
                }
            );

        private static IEnumerable<ClassMethodBuilder> GetMethods(Type instance)
            => instance.GetMethodsRecursively()
                    .Where(m => !instance.IsRecord() && !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.Name != "GetType")
                    .Select
                    (
                        m => new ClassMethodBuilder
                        {
                            Name = m.Name,
                            TypeName = m.ReturnType.FullName.FixTypeName(),
                            Visibility = m.IsPublic
                                ? Visibility.Public
                                : Visibility.Private,
                            Static = m.IsStatic,
                            Virtual = m.IsVirtual,
                            Abstract = m.IsAbstract,
                            IsNullable = m.ReturnTypeIsNullable(),
                            Parameters = m.GetParameters().Select
                            (
                                p => new ParameterBuilder
                                {
                                    Name = p.Name,
                                    TypeName = p.ParameterType.FullName.FixTypeName(),
                                    IsNullable = p.IsNullable(),
                                    Attributes = GetAttributes(p.GetCustomAttributes(true)).ToList()
                                }
                            ).ToList(),
                            Attributes = GetAttributes(m.GetCustomAttributes(false)).ToList()
                        }
                    );

        private static IEnumerable<ClassConstructorBuilder> GetConstructors(Type instance, bool createConstructors)
            => instance.GetConstructors().Where(_ => createConstructors).Select(x => new ClassConstructorBuilder
            {
                Parameters = x.GetParameters().Select
                (
                    p =>
                    new ParameterBuilder
                    {
                        Name = p.Name,
                        TypeName = p.ParameterType.FullName.FixTypeName(),
                        IsNullable = p.IsNullable(),
                        Attributes = GetAttributes(p.GetCustomAttributes(true)).ToList()
                    }
                ).ToList()
            });

        private static IEnumerable<AttributeBuilder> GetAttributes(object[] attributes)
            => attributes.OfType<System.Attribute>().Select(x => new AttributeBuilder
            {
                Name = x.GetType().FullName
            });

        private static IEnumerable<ClassBuilder> GetSubClasses(Type instance, bool partial)
            => instance.GetNestedTypes().Select(t => t.ToClass(new ClassSettings(partial: partial)));

        private static IEnumerable<ClassPropertyBuilder> GeneratedProperties(Type type, Func<PropertyInfo, IEnumerable<ICodeStatement>> propertyCodeStatementsDelegate = null)
            => type.GetPropertiesRecursively()
                   .Where(p => p.GetGetMethod() != null && p.GetSetMethod() == null)
                   .Select(p => CreateMockProperty(p, propertyCodeStatementsDelegate));

        private static IEnumerable<ClassMethodBuilder> GeneratedMethods(Type type, Func<MethodInfo, IEnumerable<ICodeStatement>> methodCodeStatementsDelegate = null)
            => type.GetMethodsRecursively()
                   .Where(m => !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.Name != "GetType")
                   .Select(m => CreateMockMethod(m, methodCodeStatementsDelegate));

        private static IEnumerable<ClassConstructorBuilder> GeneratedCtors(Type type)
        {
            yield return new ClassConstructorBuilder
            {
                Parameters = new[]
                {
                    new ParameterBuilder
                    {
                        Name = "wrappedInstance",
                        TypeName = type.FullName.FixTypeName()
                    }
                }.ToList(),
            }.AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("_wrappedInstance = wrappedInstance;"));
        }

        private static IEnumerable<ClassFieldBuilder> GeneratedFields(Type type)
        {
            yield return new ClassFieldBuilder
            {
                Name = "_wrappedInstance",
                TypeName = type.FullName.FixTypeName(),
                Visibility = Visibility.Private,
                ReadOnly = true
            };
        }

        private static ClassPropertyBuilder CreateMockProperty(PropertyInfo propertyInfo, Func<PropertyInfo, IEnumerable<ICodeStatement>> propertyCodeStatementsDelegate = null)
            => new ClassPropertyBuilder
            {
                Name = propertyInfo.Name,
                TypeName = propertyInfo.PropertyType.FullName.FixTypeName(),
                HasSetter = false,
                GetterCodeStatements = (propertyCodeStatementsDelegate?.Invoke(propertyInfo) ?? Enumerable.Empty<ICodeStatement>())
                    .Select(x => x.CreateBuilder()).ToList()
            };

        private static ClassMethodBuilder CreateMockMethod(MethodInfo methodInfo, Func<MethodInfo, IEnumerable<ICodeStatement>> methodCodeStatementsDelegate = null)
            => new ClassMethodBuilder
            {
                Name = methodInfo.Name,
                TypeName = methodInfo.ReturnType.FullName.FixTypeName(),
                Parameters = CreateParameters(methodInfo).ToList(),
                Override = methodInfo.DeclaringType == typeof(object),
                CodeStatements = (methodCodeStatementsDelegate?.Invoke(methodInfo) ?? Enumerable.Empty<ICodeStatement>())
                    .Select(x => x.CreateBuilder()).ToList()
            };

        private static IEnumerable<ParameterBuilder> CreateParameters(MethodInfo methodInfo)
            => methodInfo.GetParameters().Select(p => new ParameterBuilder { Name = p.Name, TypeName = p.ParameterType.FullName.FixTypeName() });

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
