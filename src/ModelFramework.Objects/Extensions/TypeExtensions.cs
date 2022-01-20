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
        public static IClass ToClass(this Type instance, ClassSettings settings)
            => instance.ToClassBuilder(settings).Build();

        public static ClassBuilder ToClassBuilder(this Type instance, ClassSettings settings)
            => new ClassBuilder()
                .WithName(instance.Name)
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
                .AddFields(GetFields(instance).ToList())
                .AddProperties(GetProperties(instance))
                .AddMethods(GetMethods(instance))
                .AddConstructors(GetConstructors(instance, settings.CreateConstructors))
                .AddAttributes(GetAttributes(instance.GetCustomAttributes(false)))
                .AddSubClasses(GetSubClasses(instance, settings.Partial));

        public static IClass ToWrapperClass(this Type instance, WrapperClassSettings settings)
            => instance.ToWrapperClassBuilder(settings).Build();

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
            if (instance.FullName == null)
            {
                throw new ArgumentException("Can't get typename without generics when the FullName of this type is null. Could not determine typename.");
            }

            var index = instance.FullName.IndexOf('`');
            return index == -1
                ? instance.FullName
                : instance.FullName.Substring(0, index);
        }

        private static IEnumerable<string> GetInterfaces(Type instance)
            => instance.GetInterfaces()
                       .Where(t => !(instance.IsRecord() && t.FullName.StartsWith("System.IEquatable`1[[" + instance.FullName)))
                       .Select(t => t.FullName);

        private static IEnumerable<ClassFieldBuilder> GetFields(Type instance)
            => instance.GetFieldsRecursively().Select
                (
                    f => new ClassFieldBuilder()
                        .WithName(f.Name)
                        .WithTypeName(f.FieldType.FullName)
                        .WithStatic(f.IsStatic)
                        .WithConstant(f.IsLiteral)
                        .WithIsNullable(f.IsNullable())
                        .WithVisibility(f.IsPublic
                            ? Visibility.Public
                            : Visibility.Private)
                        .AddAttributes(GetAttributes(f.GetCustomAttributes(false)))
                );

        private static IEnumerable<ClassPropertyBuilder> GetProperties(Type instance) =>
            instance.GetPropertiesRecursively().Select
            (
                p => new ClassPropertyBuilder()
                    .WithName(p.Name)
                    .WithTypeName(p.PropertyType.FullName)
                    .WithHasGetter(p.GetGetMethod() != null)
                    .WithHasSetter(p.GetSetMethod() != null)
                    .WithHasInitializer(p.IsInitOnly())
                    .WithIsNullable(p.IsNullable())
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
                    .AddAttributes(GetAttributes(p.GetCustomAttributes(false)))
            );

        private static IEnumerable<ClassMethodBuilder> GetMethods(Type instance)
            => instance.GetMethodsRecursively()
                    .Where(m => !instance.IsRecord() && !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.Name != "GetType")
                    .Select
                    (
                        m => new ClassMethodBuilder()
                            .WithName(m.Name)
                            .WithTypeName(m.ReturnType.FullName.FixTypeName())
                            .WithVisibility(m.IsPublic
                                ? Visibility.Public
                                : Visibility.Private)
                            .WithStatic(m.IsStatic)
                            .WithVirtual(m.IsVirtual)
                            .WithAbstract(m.IsAbstract)
                            .WithIsNullable(m.ReturnTypeIsNullable())
                            .AddParameters(m.GetParameters().Select
                            (
                                p => new ParameterBuilder()
                                    .WithName(p.Name)
                                    .WithTypeName(p.ParameterType.FullName.FixTypeName())
                                    .WithIsNullable(p.IsNullable())
                                    .AddAttributes(GetAttributes(p.GetCustomAttributes(true)))
                            ))
                            .AddAttributes(GetAttributes(m.GetCustomAttributes(false)))
                    );

        private static IEnumerable<ClassConstructorBuilder> GetConstructors(Type instance, bool createConstructors)
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
                                .AddAttributes(GetAttributes(p.GetCustomAttributes(true)))
                        )
                    )
            );

        private static IEnumerable<AttributeBuilder> GetAttributes(object[] attributes)
            => attributes.OfType<Attribute>().Where(x => x.GetType().FullName != "System.Runtime.CompilerServices.NullableContextAttribute"
                                                    && x.GetType().FullName != "System.Runtime.CompilerServices.NullableAttribute")
                         .Select(x => new AttributeBuilder().WithName(x.GetType().FullName));

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
                .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("_wrappedInstance = wrappedInstance;"));
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
