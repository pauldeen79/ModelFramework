using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;
using ModelFramework.Objects.Settings;

namespace ModelFramework.Objects.Extensions
{
    public static class TypeBaseExtensions
    {
        public static string GetInheritedClasses(this ITypeBase instance)
        {
            if (instance is IClass cls)
            {
                var lst = new List<string>();
                if (!string.IsNullOrEmpty(cls.BaseClass)) lst.Add(cls.BaseClass);
                if (cls.AutoGenerateInterface && !cls.Interfaces?.Any(i => i == $"I{cls.Name}") == true) lst.Add($"I{cls.Name}");
                if (!cls.AutoGenerateInterface && cls.Interfaces != null) lst.AddRange(cls.Interfaces);
                return lst.Count == 0
                    ? string.Empty
                    : $" : {string.Join(", ", lst.Select(x => x.GetCsharpFriendlyTypeName()))}";
            }
            else
            {
                return instance.Interfaces?.Any() != true
                    ? string.Empty
                    : $" : {string.Join(", ", instance.Interfaces.Select(x => x.GetCsharpFriendlyTypeName()))}";
            }
        }

        public static string GetContainerType(this ITypeBase typeBase)
        {
            if (typeBase is IClass cls)
            {
                return cls.Record
                    ? "record"
                    : "class";
            }
            if (typeBase is IInterface)
            {
                return "interface";
            }

            return $"[unknown container type: {typeBase.GetType().FullName}]";
        }

        public static InterfaceBuilder ToInterface(this ITypeBase instance, InterfaceSettings settings)
        => new InterfaceBuilder
            (
                "I" + instance.Name,
                instance.Namespace,
                instance.Visibility,
                instance.Partial,
                instance.Interfaces.Where(i => i != "I" + instance.Name && i != instance.Namespace + ".I" + instance.Name),
                settings.PropertyFilter == null
                    ? instance.Properties.Select(p => ChangeProperty(p, settings.ApplyGenericTypes, settings.ChangePropertiesToReadOnly))
                    : instance.Properties.Where(settings.PropertyFilter).Select(p => ChangeProperty(p, settings.ApplyGenericTypes, settings.ChangePropertiesToReadOnly)),
                settings.MethodFilter == null
                    ? instance.Methods.Where(m => !m.Static
                        && !m.Override
                        && !m.Operator
                        && !m.IsInterfaceMethod()
                        && !m.SkipOnAutoGenerateInterface()
                        && string.IsNullOrEmpty(m.ExplicitInterfaceName)
                        && m.Visibility == Visibility.Public).Select(m => ChangeArgumentsAndReturnType(m, settings.ApplyGenericTypes))
                    : instance.Methods.Where(settings.MethodFilter).Select(m => ChangeArgumentsAndReturnType(m, settings.ApplyGenericTypes)),
                settings.MetadataFilter == null
                    ? instance.Metadata
                    : instance.Metadata.Where(settings.MetadataFilter),
                settings.AttributeFilter == null
                    ? instance.Attributes
                    : instance.Attributes.Where(settings.AttributeFilter),
                GenerateGenericTypeArguments(settings.ApplyGenericTypes)
            );

        public static ClassBuilder ToImmutableClass(this ITypeBase instance, ImmutableClassSettings settings)
        {
            if (!instance.Properties.Any())
            {
                throw new InvalidOperationException("To create an immutable class, there must be at least one property");
            }

            return new ClassBuilder
            (
                instance.Name,
                instance.Namespace,
                properties:
                    instance
                        .Properties
                        .Select
                        (
                            p => new ClassProperty
                            (
                                p.Name,
                                p.TypeName.FixImmutableCollectionTypeName(settings.NewCollectionTypeName),
                                p.Static,
                                p.Virtual,
                                p.Abstract,
                                p.Protected,
                                p.Override,
                                p.HasGetter,
                                p.HasInit,
                                false,
                                p.Visibility,
                                p.GetterVisibility,
                                p.SetterVisibility,
                                p.InitVisibility,
                                p.GetterBody,
                                p.SetterBody,
                                p.InitBody,
                                p.ExplicitInterfaceName,
                                p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)),
                                p.Attributes,
                                p.GetterCodeStatements,
                                p.SetterCodeStatements,
                                p.InitCodeStatements
                            )
                        ),
                constructors:
                    new[]
                    {
                        new ClassConstructor
                        (
                            parameters: instance.Properties.Select(p => new Parameter(p.Name.ToPascalCase(), p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)).GetMetadataStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixImmutableCollectionTypeName(settings.NewCollectionTypeName), o => string.Format(o?.ToString() ?? string.Empty, p.Name.ToPascalCase().GetCsharpFriendlyName(), p.TypeName.GetGenericArguments())))),
                            codeStatements: instance.Properties.Select(p => new LiteralCodeStatement($"this.{p.Name.GetCsharpFriendlyName()} = {p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)).GetMetadataStringValue(MetadataNames.CustomImmutableDefaultValue, p.Name.ToPascalCase().GetCsharpFriendlyName(), o => string.Format(o?.ToString() ?? string.Empty, p.Name.ToPascalCase().GetCsharpFriendlyName(), p.TypeName.GetGenericArguments()))};"))
                            .Concat(settings.ValidateArgumentsInConstructor
                                ? new[] { new LiteralCodeStatement("System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new ValidationContext(this, null, null), true);") }
                                : Enumerable.Empty<LiteralCodeStatement>())
                        )
                    },
                methods: GetImmutableClassMethods(instance, settings.NewCollectionTypeName, settings.CreateWithMethod, settings.ImplementIEquatable, false),
                interfaces: settings.ImplementIEquatable
                    ? new[] { $"IEquatable<{instance.Name}>" }
                    : Enumerable.Empty<string>()
            );
        }

        public static ClassBuilder ToImmutableExtensionClass(this ITypeBase instance,
                                                             string newCollectionTypeName = "System.Collections.Immutable.IImmutableList")
        => new ClassBuilder
            (
                instance.Name + "Extensions",
                instance.Namespace,
                methods: GetImmutableClassMethods(instance, newCollectionTypeName, true, false, true),
                @static: true
            );

        public static ClassBuilder ToPocoClass(this ITypeBase instance, string newCollectionTypeName = "System.Collections.Generic.ICollection")
            => new ClassBuilder
            (
                instance.Name,
                instance.Namespace,
                properties:
                    instance
                        .Properties
                        .Select
                        (
                            p => new ClassProperty
                            (
                                p.Name,
                                p.TypeName.FixBuilderCollectionTypeName(newCollectionTypeName),
                                p.Static,
                                p.Virtual,
                                p.Abstract,
                                p.Protected,
                                p.Override,
                                p.HasGetter,
                                true,
                                false,
                                p.Visibility,
                                p.GetterVisibility,
                                p.SetterVisibility,
                                p.InitVisibility,
                                p.GetterBody,
                                p.SetterBody,
                                null,
                                p.ExplicitInterfaceName,
                                p.Metadata.Concat(p.GetBuilderCollectionMetadata(newCollectionTypeName)),
                                p.Attributes,
                                p.GetterCodeStatements,
                                p.SetterCodeStatements,
                                null
                            )
                        )
            );

        public static ClassBuilder ToObservableClass(this ITypeBase instance, string newCollectionTypeName = "System.Collections.ObjectModel.ObservableCollection")
            => new ClassBuilder
            (
                instance.Name,
                instance.Namespace,
                properties:
                    instance
                        .Properties
                        .Select
                        (
                            p => new ClassProperty
                            (
                                p.Name,
                                p.TypeName.FixObservableCollectionTypeName(newCollectionTypeName),
                                p.Static,
                                p.Virtual,
                                p.Abstract,
                                p.Protected,
                                p.Override,
                                p.HasGetter,
                                true,
                                false,
                                p.Visibility,
                                p.GetterVisibility,
                                p.SetterVisibility,
                                p.InitVisibility,
                                p.FixObservablePropertyGetterBody(newCollectionTypeName),
                                p.FixObservablePropertySetterBody(newCollectionTypeName),
                                null,
                                p.ExplicitInterfaceName,
                                p.Metadata.Concat(p.GetObservableCollectionMetadata(newCollectionTypeName)),
                                p.Attributes,
                                p.GetterCodeStatements,
                                p.SetterCodeStatements,
                                null
                            )
                        ),
                fields: instance.Properties
                        .Where(p => p.TypeName.FixObservableCollectionTypeName(newCollectionTypeName).StartsWith("System.Collections.ObjectModel.ObservableCollection<"))
                        .Select
                        (
                            p => new ClassField
                            (
                                "_" + p.Name.ToPascalCase(),
                                p.TypeName,
                                visibility: Visibility.Private
                            )
                        ),
                constructors: new[]
                {
                    new ClassConstructor
                    (
                        codeStatements: instance.Properties
                            .Where(p => p.TypeName.FixObservableCollectionTypeName(newCollectionTypeName).StartsWith("System.Collections.ObjectModel.ObservableCollection<"))
                            .Select(p => new LiteralCodeStatement($"this.{p.Name} = Utilities.Extensions.InitializeObservableCollection(default({p.TypeName.FixObservableCollectionTypeName(newCollectionTypeName).GetCsharpFriendlyTypeName()}));"))
                    )
                }
            );

        public static ClassBuilder ToImmutableBuilderClass(this ITypeBase instance, ImmutableBuilderClassSettings settings)
        {
            if (!instance.Properties.Any())
            {
                throw new InvalidOperationException("To create an immutable builder class, there must be at least one property");
            }

            return new ClassBuilder
            (
                instance.Name + "Builder",
                settings.NewNamespace ?? instance.Namespace,
                fields:
                    instance
                        .Properties
                        .Select
                        (
                            p => new ClassField
                            (
                                "_" + p.Name.ToPascalCase(),
                                p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, p.TypeName, o => string.Format(o?.ToString() ?? string.Empty, p.TypeName, p.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName(settings.NewCollectionTypeName),
                                visibility: Visibility.Private
                            )
                        ),
                constructors: GetImmutableBuilderClassConstructors(instance, settings),
                methods: GetImmutableBuilderClassMethods(instance, settings),
                properties: GetImmutableBuilderClassProperties(instance, settings)
            );
        }

        public static string GetGenericTypeArgumentsString(this ITypeBase instance)
        {
            if (instance.GenericTypeArguments == null || instance.GenericTypeArguments.Length == 0)
            {
                return string.Empty;
            }

            return $"<{string.Join(", ", instance.GenericTypeArguments)}>";
        }

        private static IEnumerable<IClassConstructor> GetImmutableBuilderClassConstructors(ITypeBase instance, ImmutableBuilderClassSettings settings)
        {
            yield return new ClassConstructor
            (
                codeStatements: instance.Properties
                    .Where(p => p.TypeName.IsCollectionTypeName())
                    .Select(p => new LiteralCodeStatement($"_{p.Name.ToPascalCase()} = new {p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, p.TypeName, o => string.Format(o?.ToString() ?? string.Empty, p.TypeName, p.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName(settings.NewCollectionTypeName).GetCsharpFriendlyTypeName()}();"))
            );

            yield return new ClassConstructor
            (
                parameters: new[] { new Parameter("source", FormatInstanceName(instance, false, settings.FormatInstanceTypeNameDelegate)) },
                codeStatements: instance.Properties
                    .Where(p => p.TypeName.IsCollectionTypeName())
                    .Select(p => new LiteralCodeStatement($"_{p.Name.ToPascalCase()} = new {p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, p.TypeName, o => string.Format(o?.ToString() ?? string.Empty, p.TypeName, p.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName(settings.NewCollectionTypeName).GetCsharpFriendlyTypeName()}();"))
                    .Concat(instance.Properties.Select(p => new LiteralCodeStatement(p.CreateImmutableBuilderInitializationCode(settings.AddNullChecks))))
            );

            if (settings.AddCopyConstructor)
            {
                var cls = instance as IClass;
                var ctors = cls?.Constructors ?? new ValueCollection<IClassConstructor>();
                var properties = settings.Poco
                    ? instance.Properties
                    : ctors.First(x => x.Parameters.Count > 0).Parameters
                        .Select(x => instance.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
                        .Where(x => x != null);

                yield return new ClassConstructor
                (
                    parameters: properties.Select(p => new Parameter(p.Name.ToPascalCase(), p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)).GetMetadataStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixImmutableCollectionTypeName(settings.NewCollectionTypeName), o => string.Format(o?.ToString() ?? string.Empty, p.Name.ToPascalCase().GetCsharpFriendlyName(), p.TypeName.GetGenericArguments())))),
                    codeStatements: properties.Where(p => p.TypeName.IsCollectionTypeName())
                    .Select(p => new LiteralCodeStatement($"_{p.Name.ToPascalCase()} = new {p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, p.TypeName, o => string.Format(o?.ToString() ?? string.Empty, p.TypeName, p.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName(settings.NewCollectionTypeName).GetCsharpFriendlyTypeName()}();"))
                    .Concat(instance.Properties
                        .Select(p => p.TypeName.IsCollectionTypeName()
                            ? new LiteralCodeStatement(CreateCtorStatementForCollection(p, settings))
                            : new LiteralCodeStatement($"_{p.Name.ToPascalCase()} = {p.Name.ToPascalCase()};")))
                );
            }
        }

        private static string CreateCtorStatementForCollection(IClassProperty p, ImmutableBuilderClassSettings settings)
            => settings.AddNullChecks
                ? $"if ({p.Name.ToPascalCase()} != null) foreach (var x in {p.Name.ToPascalCase()}) _{p.Name.ToPascalCase()}.Add(x);"
                : $"foreach (var x in {p.Name.ToPascalCase()}) _{p.Name.ToPascalCase()}.Add(x);";

        private static IEnumerable<IClassMethod> GetImmutableBuilderClassMethods(ITypeBase instance, ImmutableBuilderClassSettings settings)
        {
            var openSign = GetImmutableBuilderPocoOpenSign(settings.Poco);
            var closeSign = GetImmutableBuilderPocoCloseSign(settings.Poco);
            yield return new ClassMethod
            (
                "Build",
                FormatInstanceName(instance, false, settings.FormatInstanceTypeNameDelegate),
                codeStatements: new[]
                {
                    new LiteralCodeStatement($"return new {FormatInstanceName(instance, true, settings.FormatInstanceTypeNameDelegate)}{openSign}{GetBuildMethodParameters(instance, settings.Poco)}{closeSign};")
                }
            );
            yield return new ClassMethod
            (
                "Clear",
                $"{instance.Name}Builder",
                codeStatements:
                    instance.Properties
                        .Select(p => new LiteralCodeStatement(p.CreateImmutableBuilderClearCode()))
                        .Concat(new[] { "return this;" }.ToLiteralCodeStatements())

            );

            if (settings.AddCopyConstructor)
            {
                yield return new ClassMethod
                (
                    "Update",
                    $"{instance.Name}Builder",
                    parameters: new[] { new Parameter("source", FormatInstanceName(instance, false, settings.FormatInstanceTypeNameDelegate)) },
                    codeStatements:
                    instance.Properties
                        .Select
                        (
                            p => p.TypeName.IsCollectionTypeName()
                                ? $"_{p.Name.ToPascalCase()} = new {p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, p.TypeName, o => string.Format(o?.ToString() ?? string.Empty, p.TypeName, p.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName(settings.NewCollectionTypeName).GetCsharpFriendlyTypeName()}();"
                                : $"_{p.Name.ToPascalCase()} = default;"
                        )
                        .Concat(instance.Properties.Select(p => p.CreateImmutableBuilderInitializationCode(settings.AddNullChecks)))
                        .Concat(new[]
                        {
                            "return this;"
                        })
                        .ToLiteralCodeStatements()
                );
            }
            foreach (var property in instance.Properties)
            {
                var overloads = property.Metadata.GetMetadataStringValues(MetadataNames.CustomImmutableBuilderWithOverloadArgumentType)
                    .Zip(property.Metadata.GetMetadataStringValues(MetadataNames.CustomImmutableBuilderWithOverloadInitializeExpression),
                        (typeName, expression) => new { TypeName = typeName, Expression = expression });
                if (property.TypeName.IsCollectionTypeName())
                {
                    // collection
                    yield return new ClassMethod
                    (
                        $"Clear{property.Name}",
                        $"{instance.Name}Builder",
                        codeStatements: new[]
                        {
                            $"_{property.Name.ToPascalCase()}.Clear();",
                            "return this;"
                        }.ToLiteralCodeStatements()
                    );
                    yield return new ClassMethod
                    (
                        $"Add{property.Name}",
                        $"{instance.Name}Builder",
                        parameters: new[]
                        {
                            new Parameter(property.Name.ToPascalCase(), property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, property.TypeName, o => string.Format(o?.ToString() ?? string.Empty, property.TypeName, property.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName("System.Collections.Generic.IEnumerable"))
                        },
                        codeStatements: new[]
                        {
                            $"return Add{property.Name}({property.Name.ToPascalCase()}.ToArray());"
                        }.ToLiteralCodeStatements()
                    );
                    yield return new ClassMethod
                    (
                        $"Add{property.Name}",
                        $"{instance.Name}Builder",
                        parameters: new[]
                        {
                            new Parameter(property.Name.ToPascalCase(), "params " + property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, property.TypeName, o => string.Format(o?.ToString() ?? string.Empty, property.TypeName, property.TypeName.GetGenericArguments())).FixTypeName().ConvertTypeNameToArray())
                        },
                        codeStatements: new[]
                        {
                            $"if ({property.Name.ToPascalCase()} != null)",
                             "{",
                            $"    foreach(var itemToAdd in {property.Name.ToPascalCase()})",
                             "    {",
                             property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderAddExpression, $"        _{property.Name.ToPascalCase()}.Add(itemToAdd);", o => string.Format(o?.ToString() ?? string.Empty, property.Name.ToPascalCase(), property.TypeName, property.TypeName.GetGenericArguments())),
                             "    }",
                             "}",
                             "return this;"
                        }.ToLiteralCodeStatements()
                    );
                    foreach (var overload in overloads)
                    {
                        yield return new ClassMethod
                        (
                            $"Add{property.Name}",
                            $"{instance.Name}Builder",
                            parameters: new[]
                            {
                                new Parameter(property.Name.ToPascalCase(), string.Format(overload.TypeName, property.TypeName, property.TypeName.GetGenericArguments()).FixBuilderCollectionTypeName("System.Collections.Generic.IEnumerable"))
                            },
                            codeStatements: new[]
                            {
                                $"return Add{property.Name}({property.Name.ToPascalCase()}.ToArray());"
                            }.ToLiteralCodeStatements()
                        );
                        yield return new ClassMethod
                        (
                            $"Add{property.Name}",
                            $"{instance.Name}Builder",
                            parameters: new[]
                            {
                                new Parameter(property.Name.ToPascalCase(), "params " + string.Format(overload.TypeName, property.TypeName, property.TypeName.GetGenericArguments()).ConvertTypeNameToArray())
                            },
                            codeStatements: new[]
                            {
                                $"if ({property.Name.ToPascalCase()} != null)",
                                 "{",
                                $"    foreach(var itemToAdd in {property.Name.ToPascalCase()})",
                                 "    {",
                                 string.Format(overload.Expression, property.Name.ToPascalCase(), property.TypeName.FixTypeName(), property.TypeName.GetGenericArguments()),
                                 "    }",
                                 "}",
                                 "return this;"
                            }.ToLiteralCodeStatements()
                        );
                    }
                }
                else
                {
                    // single
                    yield return new ClassMethod
                    (
                        $"With{property.Name}",
                        $"{instance.Name}Builder",
                        parameters: new[]
                        {
                            new Parameter(property.Name.ToPascalCase(), property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, property.TypeName, o => string.Format(o?.ToString() ?? string.Empty, property.TypeName, property.TypeName.GetGenericArguments())))
                        },
                        codeStatements: new[]
                        {
                            property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderWithExpression, $"_{property.Name.ToPascalCase()} = {property.Name.ToPascalCase().GetCsharpFriendlyName()};", o => string.Format(o?.ToString() ?? string.Empty, property.Name.ToPascalCase(), property.Name.ToPascalCase().GetCsharpFriendlyName(), property.TypeName, property.TypeName.GetGenericArguments())),
                            "return this;"
                        }.ToLiteralCodeStatements()
                    );
                    foreach (var overload in overloads)
                    {
                        yield return new ClassMethod
                        (
                            $"With{property.Name}",
                            $"{instance.Name}Builder",
                            parameters: new[]
                            {
                                new Parameter(property.Name.ToPascalCase(), string.Format(overload.TypeName, property.TypeName))
                            },
                            codeStatements: new[]
                            {
                                string.Format(overload.Expression, property.Name.ToPascalCase(), property.TypeName.FixTypeName().GetCsharpFriendlyTypeName()),
                                "return this;"
                            }.ToLiteralCodeStatements()
                        );
                    }
                }
            }
        }

        private static string GetImmutableBuilderPocoCloseSign(bool poco)
            => poco
                ? " }"
                : ")";

        private static string GetImmutableBuilderPocoOpenSign(bool poco)
            => poco
                ? " { "
                : "(";

        private static string FormatInstanceName(ITypeBase instance, bool forCreate, Func<ITypeBase, bool, string> formatInstanceTypeNameDelegate)
        {
            if (formatInstanceTypeNameDelegate != null)
            {
                var retVal = formatInstanceTypeNameDelegate(instance, forCreate);
                if (retVal != null) return retVal;
            }

            var ns = string.IsNullOrEmpty(instance.Namespace)
                ? string.Empty
                : instance.Namespace + ".";

            return (ns + instance.Name).GetCsharpFriendlyTypeName();
        }

        private static IEnumerable<IClassProperty> GetImmutableBuilderClassProperties(ITypeBase instance, ImmutableBuilderClassSettings settings)
        {
            if (!settings.AddProperties)
            {
                yield break;
            }

            foreach (var property in instance.Properties)
            {
                yield return new ClassProperty
                (
                    property.Name,
                    property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, property.TypeName, o => string.Format(o?.ToString() ?? string.Empty, property.TypeName, property.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName(settings.NewCollectionTypeName),
                    getterBody: $"return _{property.Name.ToPascalCase()};",
                    setterBody: $"_{property.Name.ToPascalCase()} = value;"
                );
            }
        }

        private static string GetBuildMethodParameters(ITypeBase instance, bool poco)
        {
            var cls = instance as IClass;
            var ctors = cls?.Constructors ?? new ValueCollection<IClassConstructor>();

            var properties = poco
                ? instance.Properties
                : ctors.First(x => x.Parameters.Count > 0).Parameters
                    .Select(x => instance.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
                    .Where(x => x != null);

            var defaultValueDelegate = poco
                ? new Func<IClassProperty, string>(p => $"{p.Name} = _{p.Name.ToPascalCase()}")
                : new Func<IClassProperty, string>(p => $"_{p.Name.ToPascalCase()}");

            return string.Join(", ", properties.Select(p => p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderMethodParameterExpression, defaultValueDelegate(p), o => string.Format(o?.ToString() ?? string.Empty, p.Name, p.Name.ToPascalCase()))));
        }

        private static IClassProperty ChangeProperty(IClassProperty property, IDictionary<string, string> applyGenericTypes, bool changePropertiesToReadOnly)
            => !changePropertiesToReadOnly
                ? property
                : new ClassProperty(property.Name,
                                    ReplaceGenericType(property.TypeName, applyGenericTypes),
                                    property.Static,
                                    property.Virtual,
                                    property.Abstract,
                                    property.Protected,
                                    property.Override,
                                    property.HasGetter,
                                    false,
                                    false,
                                    property.Visibility,
                                    property.GetterVisibility,
                                    property.SetterVisibility,
                                    property.InitVisibility,
                                    property.GetterBody,
                                    property.SetterBody,
                                    null,
                                    property.ExplicitInterfaceName,
                                    property.Metadata,
                                    property.Attributes,
                                    property.GetterCodeStatements,
                                    property.SetterCodeStatements,
                                    null);

        private static IClassMethod ChangeArgumentsAndReturnType(IClassMethod m, IDictionary<string, string> applyGenericTypes)
        {
            if (applyGenericTypes == null || applyGenericTypes.Count == 0)
            {
                return m;
            }

            return new ClassMethod
            (
                m.Name,
                ReplaceGenericType(m.TypeName, applyGenericTypes),
                m.Visibility,
                m.Static,
                m.Virtual,
                m.Abstract,
                m.Protected,
                m.Partial,
                m.Override,
                m.ExtensionMethod,
                m.Operator,
                m.Body,
                m.ExplicitInterfaceName,
                m.Parameters.Select(p => ChangeParameter(p, applyGenericTypes)),
                m.Attributes,
                m.CodeStatements,
                m.Metadata
            );
        }

        private static IParameter ChangeParameter(IParameter p, IDictionary<string, string> applyGenericTypes)
        {
            if (applyGenericTypes == null || applyGenericTypes.Count == 0)
            {
                return p;
            }

            return new Parameter
            (
                p.Name,
                ReplaceGenericType(p.TypeName, applyGenericTypes),
                p.DefaultValue,
                p.Attributes,
                p.Metadata
            );
        }

        private static string ReplaceGenericType(string typeName, IDictionary<string, string> applyGenericTypes)
        {
            if (applyGenericTypes == null || applyGenericTypes.Count == 0)
            {
                return typeName;
            }

            if (!applyGenericTypes.ContainsKey(typeName))
            {
                return typeName;
            }

            return applyGenericTypes[typeName];
        }

        private static IEnumerable<string> GenerateGenericTypeArguments(IDictionary<string, string> applyGenericTypes)
        {
            if (applyGenericTypes == null || applyGenericTypes.Count == 0)
            {
                return Enumerable.Empty<string>();
            }

            return applyGenericTypes.Values;
        }

        private static IEnumerable<ClassMethod> GetImmutableClassMethods
        (
            ITypeBase instance,
            string newCollectionTypeName,
            bool createWithMethod,
            bool implementIEquatable,
            bool extensionMethod
        )
        {
            if (createWithMethod)
            {
                yield return
                    new ClassMethod
                    (
                        "With",
                        instance.Name,
                        @static: extensionMethod,
                        codeStatements:
                            new[]
                            {
                            $"return new {instance.Name}",
                            "(",
                            }
                            .Concat
                            (
                                instance
                                    .Properties
                                    .Select
                                    (p => new
                                    {
                                        p.Name,
                                        TypeName = p.TypeName.FixImmutableCollectionTypeName(newCollectionTypeName),
                                        OriginalMetadata = p.Metadata,
                                        Metadata = p.Metadata.Concat(p.GetImmutableCollectionMetadata(newCollectionTypeName)),
                                        Suffix = p.Name != instance.Properties.Last().Name
                                                    ? ","
                                                    : string.Empty
                                    }
                                    )
                                    .Select(p => $"    {p.Name.ToPascalCase()} == default({p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName, o => string.Format(o?.ToString() ?? string.Empty, p.TypeName)).GetCsharpFriendlyTypeName()}) ? {GetInstanceName(extensionMethod)}.{p.Name} : {p.OriginalMetadata.GetMetadataStringValue(MetadataNames.CustomImmutableDefaultValue, p.Name.ToPascalCase(), o => string.Format(o?.ToString() ?? string.Empty, p.Name.ToPascalCase()))}{p.Suffix}")
                            )
                            .Concat
                            (
                                new[]
                                {
                                ");"
                                }
                            )
                            .ToLiteralCodeStatements(),
                        parameters:
                            (extensionMethod
                                ? new[] { new Parameter("instance", instance.Name) }
                                : Enumerable.Empty<Parameter>())
                            .Concat(
                            instance
                                .Properties
                                .Select
                                (
                                    p => new Parameter
                                    (
                                        p.Name.ToPascalCase().GetCsharpFriendlyName(),
                                        p.Metadata
                                            .Concat(p.GetImmutableCollectionMetadata(newCollectionTypeName))
                                            .GetMetadataStringValue
                                            (
                                                MetadataNames.CustomImmutableArgumentType,
                                                p.TypeName.FixImmutableCollectionTypeName(newCollectionTypeName),
                                                o => string.Format(o?.ToString() ?? string.Empty, p.TypeName.FixImmutableCollectionTypeName(newCollectionTypeName))
                                            ).GetCsharpFriendlyTypeName(),
                                        new Literal($"default({p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixImmutableCollectionTypeName(newCollectionTypeName)).GetCsharpFriendlyTypeName()})")
                                    )
                                )),
                        extensionMethod: extensionMethod
                    );
            }

            if (implementIEquatable)
            {
                yield return new ClassMethod("Equals", typeof(bool).FullName, @override: true, parameters: new[] { new Parameter("obj", typeof(object).FullName) }, codeStatements: new[] { new LiteralCodeStatement($"return Equals(obj as {instance.Name});") });
                yield return new ClassMethod($"IEquatable<{instance.Name}>.Equals", typeof(bool).FullName, parameters: new[] { new Parameter("other", instance.Name) }, codeStatements: new[] { new LiteralCodeStatement($"return other != null &&{Environment.NewLine}       {GetEqualsProperties(instance)};") });
                yield return new ClassMethod("GetHashCode", typeof(int).FullName, @override: true,
                    codeStatements: new[] { "int hashCode = 235838129;" }
                       .Concat(instance.Properties.Select(p => Type.GetType(p.TypeName.FixTypeName())?.IsValueType == true
                            ? $"hashCode = hashCode * -1521134295 + {p.Name}.GetHashCode();"
                            : $"hashCode = hashCode * -1521134295 + EqualityComparer<{p.TypeName.FixTypeName()}>.Default.GetHashCode({p.Name});"))
                        .Concat(new[] { "return hashCode;" })
                        .Select(x => new LiteralCodeStatement(x)));
                yield return new ClassMethod("==", typeof(bool).FullName, @static: true, @operator: true, parameters: new[] { new Parameter("left", instance.Name), new Parameter("right", instance.Name) }, codeStatements: new[] { new LiteralCodeStatement($"return EqualityComparer<{instance.Name}>.Default.Equals(left, right);") });
                yield return new ClassMethod("!=", typeof(bool).FullName, @static: true, @operator: true, parameters: new[] { new Parameter("left", instance.Name), new Parameter("right", instance.Name) }, codeStatements: new[] { new LiteralCodeStatement("return !(left == right);") });
            }
        }

        private static string GetEqualsProperties(ITypeBase instance)
            => string.Join(" &&" + Environment.NewLine + "       ", instance.Properties.Select(p => $"{p.Name} == other.{p.Name}"));

        private static string GetInstanceName(bool extensionMethod)
            => extensionMethod
                ? "instance"
                : "this";
    }
}
