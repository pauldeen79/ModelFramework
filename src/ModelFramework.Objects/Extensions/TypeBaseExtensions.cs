using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using ModelFramework.Common;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements.Builders;
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

        public static IInterface ToInterface(this ITypeBase instance, InterfaceSettings settings)
            => instance.ToInterfaceBuilder(settings).Build();

        public static InterfaceBuilder ToInterfaceBuilder(this ITypeBase instance, InterfaceSettings settings)
            => new InterfaceBuilder
            {
                Name = "I" + instance.Name,
                Namespace = instance.Namespace,
                Visibility = instance.Visibility,
                Partial = instance.Partial,
                Interfaces = instance.Interfaces.Where(i => i != "I" + instance.Name && i != instance.Namespace + ".I" + instance.Name)
                                                .ToList(),
                Properties = settings.PropertyFilter == null
                    ? instance.Properties.Select(p => ChangeProperty(p, settings.ApplyGenericTypes, settings.ChangePropertiesToReadOnly))
                                         .Select(x => new ClassPropertyBuilder(x))
                                         .ToList()
                    : instance.Properties.Where(settings.PropertyFilter)
                                         .Select(p => ChangeProperty(p, settings.ApplyGenericTypes, settings.ChangePropertiesToReadOnly))
                                         .Select(x => new ClassPropertyBuilder(x))
                                         .ToList(),
                Methods = settings.MethodFilter == null
                    ? instance.Methods
                        .Where(m => !m.Static
                            && !m.Override
                            && !m.Operator
                            && !m.IsInterfaceMethod()
                            && !m.SkipOnAutoGenerateInterface()
                            && string.IsNullOrEmpty(m.ExplicitInterfaceName)
                            && m.Visibility == Visibility.Public)
                        .Select(m => ChangeArgumentsAndReturnType(m, settings.ApplyGenericTypes))
                        .Select(x => new ClassMethodBuilder(x))
                        .ToList()
                    : instance.Methods.Where(settings.MethodFilter)
                                      .Select(m => ChangeArgumentsAndReturnType(m, settings.ApplyGenericTypes))
                                      .Select(x => new ClassMethodBuilder(x))
                                      .ToList(),
                Metadata = settings.MetadataFilter == null
                    ? instance.Metadata.Select(x => new MetadataBuilder(x))
                                       .ToList()
                    : instance.Metadata.Where(settings.MetadataFilter)
                                       .Select(x => new MetadataBuilder(x))
                                       .ToList(),
                Attributes = settings.AttributeFilter == null
                    ? instance.Attributes.Select(x => new AttributeBuilder(x))
                                         .ToList()
                    : instance.Attributes.Where(settings.AttributeFilter)
                                         .Select(x => new AttributeBuilder(x))
                                         .ToList(),
                GenericTypeArguments = GenerateGenericTypeArguments(settings.ApplyGenericTypes)
            };

        public static IClass ToImmutableClass(this ITypeBase instance, ImmutableClassSettings settings)
            => instance.ToImmutableClassBuilder(settings).Build();
        
        public static ClassBuilder ToImmutableClassBuilder(this ITypeBase instance, ImmutableClassSettings settings)
        {
            if (!instance.Properties.Any())
            {
                throw new InvalidOperationException("To create an immutable class, there must be at least one property");
            }

            return new ClassBuilder
            {
                Name = instance.Name,
                Namespace = instance.Namespace,
                Properties =
                    instance
                        .Properties
                        .Select
                        (
                            p => new ClassPropertyBuilder
                            {
                                Name = p.Name,
                                TypeName = p.TypeName.FixImmutableCollectionTypeName(settings.NewCollectionTypeName),
                                Static = p.Static,
                                Virtual = p.Virtual,
                                Abstract = p.Abstract,
                                Protected = p.Protected,
                                Override = p.Override,
                                HasGetter = p.HasGetter,
                                HasInitializer = p.HasInitializer,
                                HasSetter = false,
                                IsNullable = p.IsNullable,
                                Visibility = p.Visibility,
                                GetterVisibility = p.GetterVisibility,
                                SetterVisibility = p.SetterVisibility,
                                InitializerVisibility = p.InitializerVisibility,
                                ExplicitInterfaceName = p.ExplicitInterfaceName,
                                Metadata = p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName))
                                            .Select(x => new MetadataBuilder(x))
                                            .ToList(),
                                Attributes = p.Attributes.Select(x => new AttributeBuilder(x)).ToList(),
                                GetterCodeStatements = p.GetterCodeStatements.Select(x => x.CreateBuilder()).ToList(),
                                SetterCodeStatements = p.SetterCodeStatements.Select(x => x.CreateBuilder()).ToList(),
                                InitializerCodeStatements = p.InitializerCodeStatements.Select(x => x.CreateBuilder()).ToList()
                            }
                        ).ToList(),
                Constructors = 
                    new[]
                    {
                        new ClassConstructorBuilder
                        {
                            Parameters = instance.Properties.Select(p => new ParameterBuilder
                            {
                                Name = p.Name.ToPascalCase(),
                                TypeName = p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName))
                                                     .GetMetadataStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixImmutableCollectionTypeName(settings.NewCollectionTypeName), o => string.Format(o?.ToString() ?? string.Empty, p.Name.ToPascalCase().GetCsharpFriendlyName(), p.TypeName.GetGenericArguments()))
                                                     
                            }).ToList(),
                            CodeStatements = instance.Properties.Select(p => new LiteralCodeStatementBuilder
                            {
                                Statement = $"this.{p.Name.GetCsharpFriendlyName()} = {p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)).GetMetadataStringValue(MetadataNames.CustomImmutableDefaultValue, p.Name.ToPascalCase().GetCsharpFriendlyName(), o => string.Format(o?.ToString() ?? string.Empty, p.Name.ToPascalCase().GetCsharpFriendlyName(), p.TypeName.GetGenericArguments()))};"
                            })
                            .Cast<ICodeStatementBuilder>()
                            .Concat
                            (
                                settings.ValidateArgumentsInConstructor
                                    ? new[]
                                        {
                                            new LiteralCodeStatementBuilder
                                            {
                                                Statement = "System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new ValidationContext(this, null, null), true);"
                                            }
                                        }
                                    : Enumerable.Empty<LiteralCodeStatementBuilder>()
                            )
                            .ToList()
                        }
                    }.ToList(),
                Methods = GetImmutableClassMethods(instance, settings.NewCollectionTypeName, settings.CreateWithMethod, settings.ImplementIEquatable, false).ToList(),
                Interfaces = settings.ImplementIEquatable
                    ? new[] { $"IEquatable<{instance.Name}>" }.ToList()
                    : new List<string>()
            };
        }

        public static IClass ToImmutableExtensionClass(this ITypeBase instance,
                                                       string newCollectionTypeName = "System.Collections.Immutable.IImmutableList")
            => instance.ToImmutableExtensionClassBuilder(newCollectionTypeName).Build();

        public static ClassBuilder ToImmutableExtensionClassBuilder(this ITypeBase instance,
                                                                    string newCollectionTypeName = "System.Collections.Immutable.IImmutableList")
            => new ClassBuilder
            {
                Name = instance.Name + "Extensions",
                Namespace = instance.Namespace,
                Methods = GetImmutableClassMethods(instance, newCollectionTypeName, true, false, true).ToList(),
                Static = true
            };

        public static IClass ToPocoClass(this ITypeBase instance,
                                         string newCollectionTypeName = "System.Collections.Generic.ICollection")
            => instance.ToPocoClassBuilder(newCollectionTypeName).Build();

        public static ClassBuilder ToPocoClassBuilder(this ITypeBase instance,
                                                      string newCollectionTypeName = "System.Collections.Generic.ICollection")
            => new ClassBuilder
            {
                Name = instance.Name,
                Namespace = instance.Namespace,
                Properties =
                    instance
                        .Properties
                        .Select
                        (
                            p => new ClassPropertyBuilder
                            {
                                Name = p.Name,
                                TypeName = p.TypeName.FixBuilderCollectionTypeName(newCollectionTypeName),
                                Static = p.Static,
                                Virtual = p.Virtual,
                                Abstract = p.Abstract,
                                Protected = p.Protected,
                                Override = p.Override,
                                HasGetter = p.HasGetter,
                                HasSetter = true,
                                HasInitializer = false,
                                IsNullable =  p.IsNullable,
                                Visibility = p.Visibility,
                                GetterVisibility = p.GetterVisibility,
                                SetterVisibility = p.SetterVisibility,
                                InitializerVisibility = p.InitializerVisibility,
                                ExplicitInterfaceName = p.ExplicitInterfaceName,
                                Metadata = p.Metadata.Concat(p.GetBuilderCollectionMetadata(newCollectionTypeName))
                                                     .Select(x => new MetadataBuilder(x))
                                                     .ToList(),
                                Attributes = p.Attributes.Select(x => new AttributeBuilder(x)).ToList(),
                                GetterCodeStatements = p.GetterCodeStatements.Select(x => x.CreateBuilder()).ToList(),
                                SetterCodeStatements = p.SetterCodeStatements.Select(x => x.CreateBuilder()).ToList()
                            }
                        ).ToList()
            };

        public static IClass ToObservableClass(this ITypeBase instance,
                                               string newCollectionTypeName = "System.Collections.ObjectModel.ObservableCollection")
            => instance.ToObservableClassBuilder(newCollectionTypeName).Build();

        public static ClassBuilder ToObservableClassBuilder(this ITypeBase instance,
                                                            string newCollectionTypeName = "System.Collections.ObjectModel.ObservableCollection")
            => new ClassBuilder
            {
                Name = instance.Name,
                Namespace = instance.Namespace,
                Properties =
                    instance
                        .Properties
                        .Select
                        (
                            p => new ClassPropertyBuilder
                            {
                                Name = p.Name,
                                TypeName = p.TypeName.FixObservableCollectionTypeName(newCollectionTypeName),
                                Static = p.Static,
                                Virtual = p.Virtual,
                                Abstract = p.Abstract,
                                Protected = p.Protected,
                                Override = p.Override,
                                HasGetter = p.HasGetter,
                                HasSetter = true,
                                HasInitializer = false,
                                IsNullable = p.IsNullable,
                                Visibility = p.Visibility,
                                GetterVisibility = p.GetterVisibility,
                                SetterVisibility = p.SetterVisibility,
                                InitializerVisibility = p.InitializerVisibility,
                                GetterCodeStatements = p.FixObservablePropertyGetterBody(newCollectionTypeName).Select(x => x.CreateBuilder()).ToList(),
                                SetterCodeStatements = p.FixObservablePropertySetterBody(newCollectionTypeName).Select(x => x.CreateBuilder()).ToList(),
                                ExplicitInterfaceName = p.ExplicitInterfaceName,
                                Metadata = p.Metadata.Concat(p.GetObservableCollectionMetadata(newCollectionTypeName))
                                                     .Select(x => new MetadataBuilder(x))
                                                     .ToList(),
                                Attributes = p.Attributes.Select(x => new AttributeBuilder(x)).ToList()
                            }
                        ).ToList(),
                Fields = instance.Properties
                        .Where(p => p.TypeName.FixObservableCollectionTypeName(newCollectionTypeName).StartsWith("System.Collections.ObjectModel.ObservableCollection<"))
                        .Select
                        (
                            p => new ClassFieldBuilder
                            {
                                Name = "_" + p.Name.ToPascalCase(),
                                TypeName = p.TypeName,
                                Visibility = Visibility.Private
                            }
                        ).ToList(),
                Constructors = new[]
                {
                    new ClassConstructorBuilder
                    {
                        CodeStatements = instance.Properties
                            .Where(p => p.TypeName.FixObservableCollectionTypeName(newCollectionTypeName).StartsWith("System.Collections.ObjectModel.ObservableCollection<"))
                            .Select(p => new LiteralCodeStatementBuilder { Statement = $"this.{p.Name} = Utilities.Extensions.InitializeObservableCollection(default({p.TypeName.FixObservableCollectionTypeName(newCollectionTypeName).GetCsharpFriendlyTypeName()}));" })
                            .Cast<ICodeStatementBuilder>()
                            .ToList()
                    }
                }.ToList()
            };

        public static IClass ToImmutableBuilderClass(this ITypeBase instance, ImmutableBuilderClassSettings settings)
            => instance.ToImmutableBuilderClassBuilder(settings).Build();

        public static ClassBuilder ToImmutableBuilderClassBuilder(this ITypeBase instance, ImmutableBuilderClassSettings settings)
        {
            if (!instance.Properties.Any())
            {
                throw new InvalidOperationException("To create an immutable builder class, there must be at least one property");
            }

            return new ClassBuilder
            {
                Name = instance.Name + "Builder",
                Namespace = instance.Namespace,
                Fields =
                    instance
                        .Properties
                        .Select
                        (
                            p => new ClassFieldBuilder
                            {
                                Name = "_" + p.Name.ToPascalCase(),
                                TypeName = p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, p.TypeName, o => string.Format(o?.ToString() ?? string.Empty, p.TypeName, p.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName(settings.NewCollectionTypeName),
                                Visibility = Visibility.Private
                            }
                        ).ToList(),
                Constructors = GetImmutableBuilderClassConstructors(instance, settings).ToList(),
                Methods = GetImmutableBuilderClassMethods(instance, settings).ToList(),
                Properties = GetImmutableBuilderClassProperties(instance, settings).ToList()
            };
        }

        public static string GetGenericTypeArgumentsString(this ITypeBase instance)
            => instance.GenericTypeArguments != null && instance.GenericTypeArguments.Count > 0
                ? $"<{string.Join(", ", instance.GenericTypeArguments)}>"
                : string.Empty;

        private static IEnumerable<ClassConstructorBuilder> GetImmutableBuilderClassConstructors(ITypeBase instance,
                                                                                                 ImmutableBuilderClassSettings settings)
        {
            yield return new ClassConstructorBuilder
            {
                CodeStatements = instance.Properties
                    .Where(p => p.TypeName.IsCollectionTypeName())
                    .Select(p => new LiteralCodeStatementBuilder { Statement = $"_{p.Name.ToPascalCase()} = new {p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, p.TypeName, o => string.Format(o?.ToString() ?? string.Empty, p.TypeName, p.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName(settings.NewCollectionTypeName).GetCsharpFriendlyTypeName()}();" })
                    .Cast<ICodeStatementBuilder>()
                    .ToList()
            };

            yield return new ClassConstructorBuilder
            {
                Parameters = new[]
                {
                    new ParameterBuilder
                    {
                        Name = "source",
                        TypeName = FormatInstanceName(instance, false, settings.FormatInstanceTypeNameDelegate)
                    }
                }.ToList(),
                CodeStatements = instance.Properties
                    .Where(p => p.TypeName.IsCollectionTypeName())
                    .Select(p => new LiteralCodeStatementBuilder { Statement = $"_{p.Name.ToPascalCase()} = new {p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, p.TypeName, o => string.Format(o?.ToString() ?? string.Empty, p.TypeName, p.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName(settings.NewCollectionTypeName).GetCsharpFriendlyTypeName()}();" } )
                    .Concat(instance.Properties.Select(p => new LiteralCodeStatementBuilder { Statement = p.CreateImmutableBuilderInitializationCode(settings.AddNullChecks) }))
                    .Cast<ICodeStatementBuilder>()
                    .ToList()
            };

            if (settings.AddCopyConstructor)
            {
                var cls = instance as IClass;
                var ctors = cls?.Constructors ?? new ValueCollection<IClassConstructor>();
                var properties = settings.Poco
                    ? instance.Properties
                    : ctors.First(x => x.Parameters.Count > 0).Parameters
                        .Select(x => instance.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
                        .Where(x => x != null);

                yield return new ClassConstructorBuilder
                {
                    Parameters = properties.Select(p => new ParameterBuilder
                    {
                        Name = p.Name.ToPascalCase(),
                        TypeName = p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)).GetMetadataStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixImmutableCollectionTypeName(settings.NewCollectionTypeName), o => string.Format(o?.ToString() ?? string.Empty, p.Name.ToPascalCase().GetCsharpFriendlyName(), p.TypeName.GetGenericArguments()))
                    }).ToList(),
                    CodeStatements = properties.Where(p => p.TypeName.IsCollectionTypeName())
                        .Select(p => new LiteralCodeStatementBuilder { Statement = $"_{p.Name.ToPascalCase()} = new {p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, p.TypeName, o => string.Format(o?.ToString() ?? string.Empty, p.TypeName, p.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName(settings.NewCollectionTypeName).GetCsharpFriendlyTypeName()}();" })
                        .Concat
                        (
                            instance.Properties.Select
                            (
                                p => p.TypeName.IsCollectionTypeName()
                                    ? new LiteralCodeStatementBuilder { Statement = CreateCtorStatementForCollection(p, settings) }
                                    : new LiteralCodeStatementBuilder { Statement = $"_{p.Name.ToPascalCase()} = {p.Name.ToPascalCase()};" }
                            )
                        ).Cast<ICodeStatementBuilder>().ToList()
                };
            }
        }

        private static string CreateCtorStatementForCollection(IClassProperty p, ImmutableBuilderClassSettings settings)
            => settings.AddNullChecks
                ? $"if ({p.Name.ToPascalCase()} != null) foreach (var x in {p.Name.ToPascalCase()}) _{p.Name.ToPascalCase()}.Add(x);"
                : $"foreach (var x in {p.Name.ToPascalCase()}) _{p.Name.ToPascalCase()}.Add(x);";

        private static IEnumerable<ClassMethodBuilder> GetImmutableBuilderClassMethods(ITypeBase instance,
                                                                                       ImmutableBuilderClassSettings settings)
        {
            var openSign = GetImmutableBuilderPocoOpenSign(settings.Poco);
            var closeSign = GetImmutableBuilderPocoCloseSign(settings.Poco);
            yield return new ClassMethodBuilder
            {
                Name = "Build",
                TypeName = FormatInstanceName(instance, false, settings.FormatInstanceTypeNameDelegate),
                CodeStatements = new[]
                {
                    new LiteralCodeStatementBuilder { Statement = $"return new {FormatInstanceName(instance, true, settings.FormatInstanceTypeNameDelegate)}{openSign}{GetBuildMethodParameters(instance, settings.Poco)}{closeSign};" }
                }.Cast<ICodeStatementBuilder>().ToList()
            };
            yield return new ClassMethodBuilder
            {
                Name = "Clear",
                TypeName = $"{instance.Name}Builder",
                CodeStatements =
                    instance.Properties
                        .Select(p => new LiteralCodeStatementBuilder { Statement = p.CreateImmutableBuilderClearCode() })
                        .Concat(new[] { "return this;" }.ToLiteralCodeStatementBuilders())
                        .ToList()
            };

            if (settings.AddCopyConstructor)
            {
                yield return new ClassMethodBuilder
                {
                    Name = "Update",
                    TypeName = $"{instance.Name}Builder",
                    Parameters = new[]
                    {
                        new ParameterBuilder
                        {
                            Name = "source",
                            TypeName = FormatInstanceName(instance, false, settings.FormatInstanceTypeNameDelegate)
                        }
                    }.ToList(),
                    CodeStatements = instance.Properties
                        .Where(p => p.TypeName.IsCollectionTypeName())
                        .Select
                        (
                            p => $"_{p.Name.ToPascalCase()} = new {p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, p.TypeName, o => string.Format(o?.ToString() ?? string.Empty, p.TypeName, p.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName(settings.NewCollectionTypeName).GetCsharpFriendlyTypeName()}();"
                        )
                        .Concat(instance.Properties.Select(p => p.CreateImmutableBuilderInitializationCode(settings.AddNullChecks)))
                        .Concat(new[]
                        {
                            "return this;"
                        })
                        .ToLiteralCodeStatementBuilders()
                        .ToList()
                };
            }
            foreach (var property in instance.Properties)
            {
                var overloads = property.Metadata.GetMetadataStringValues(MetadataNames.CustomImmutableBuilderWithOverloadArgumentType)
                    .Zip(property.Metadata.GetMetadataStringValues(MetadataNames.CustomImmutableBuilderWithOverloadInitializeExpression),
                        (typeName, expression) => new { TypeName = typeName, Expression = expression });
                if (property.TypeName.IsCollectionTypeName())
                {
                    // collection
                    yield return new ClassMethodBuilder
                    {
                        Name = $"Clear{property.Name}",
                        TypeName = $"{instance.Name}Builder",
                        CodeStatements = new[]
                        {
                            $"_{property.Name.ToPascalCase()}.Clear();",
                            "return this;"
                        }.ToLiteralCodeStatementBuilders().ToList()
                    };
                    yield return new ClassMethodBuilder
                    {
                        Name = $"Add{property.Name}",
                        TypeName = $"{instance.Name}Builder",
                        Parameters = new[]
                        {
                            new ParameterBuilder
                            {
                                Name = property.Name.ToPascalCase(),
                                TypeName = property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, property.TypeName, o => string.Format(o?.ToString() ?? string.Empty, property.TypeName, property.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName("System.Collections.Generic.IEnumerable")
                            }
                        }.ToList(),
                        CodeStatements = new[]
                        {
                            $"return Add{property.Name}({property.Name.ToPascalCase()}.ToArray());"
                        }.ToLiteralCodeStatementBuilders().ToList()
                    };
                    yield return new ClassMethodBuilder
                    {
                        Name = $"Add{property.Name}",
                        TypeName = $"{instance.Name}Builder",
                        Parameters = new[]
                        {
                            new ParameterBuilder
                            {
                                Name = property.Name.ToPascalCase(),
                                TypeName = "params " + property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, property.TypeName, o => string.Format(o?.ToString() ?? string.Empty, property.TypeName, property.TypeName.GetGenericArguments())).FixTypeName().ConvertTypeNameToArray()
                            }
                        }.ToList(),
                        CodeStatements = new[]
                        {
                            $"if ({property.Name.ToPascalCase()} != null)",
                             "{",
                            $"    foreach(var itemToAdd in {property.Name.ToPascalCase()})",
                             "    {",
                             property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderAddExpression, $"        _{property.Name.ToPascalCase()}.Add(itemToAdd);", o => string.Format(o?.ToString() ?? string.Empty, property.Name.ToPascalCase(), property.TypeName, property.TypeName.GetGenericArguments())),
                             "    }",
                             "}",
                             "return this;"
                        }.ToLiteralCodeStatementBuilders().ToList()
                    };
                    foreach (var overload in overloads)
                    {
                        yield return new ClassMethodBuilder
                        {
                            Name = $"Add{property.Name}",
                            TypeName = $"{instance.Name}Builder",
                            Parameters = new[]
                            {
                                new ParameterBuilder
                                {
                                    Name = property.Name.ToPascalCase(),
                                    TypeName = string.Format(overload.TypeName, property.TypeName, property.TypeName.GetGenericArguments()).FixBuilderCollectionTypeName("System.Collections.Generic.IEnumerable")
                                }
                            }.ToList(),
                            CodeStatements = new[]
                            {
                                $"return Add{property.Name}({property.Name.ToPascalCase()}.ToArray());"
                            }.ToLiteralCodeStatementBuilders().ToList()
                        };
                        yield return new ClassMethodBuilder
                        {
                            Name = $"Add{property.Name}",
                            TypeName = $"{instance.Name}Builder",
                            Parameters = new[]
                            {
                                new ParameterBuilder
                                {
                                    Name = property.Name.ToPascalCase(),
                                    TypeName = "params " + string.Format(overload.TypeName, property.TypeName, property.TypeName.GetGenericArguments()).ConvertTypeNameToArray()
                                }
                            }.ToList(),
                            CodeStatements = new[]
                            {
                                $"if ({property.Name.ToPascalCase()} != null)",
                                 "{",
                                $"    foreach(var itemToAdd in {property.Name.ToPascalCase()})",
                                 "    {",
                                 string.Format(overload.Expression, property.Name.ToPascalCase(), property.TypeName.FixTypeName(), property.TypeName.GetGenericArguments()),
                                 "    }",
                                 "}",
                                 "return this;"
                            }.ToLiteralCodeStatementBuilders().ToList()
                        };
                    }
                }
                else
                {
                    // single
                    yield return new ClassMethodBuilder
                    {
                        Name = string.Format(settings.SetMethodNameFormatString, property.Name),
                        TypeName = $"{instance.Name}Builder",
                        Parameters = new[]
                        {
                            new ParameterBuilder
                            {
                                Name = property.Name.ToPascalCase(),
                                TypeName = property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, property.TypeName, o => string.Format(o?.ToString() ?? string.Empty, property.TypeName, property.TypeName.GetGenericArguments()))
                            }
                        }.ToList(),
                        CodeStatements = new[]
                        {
                            property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderWithExpression, $"_{property.Name.ToPascalCase()} = {property.Name.ToPascalCase().GetCsharpFriendlyName()};", o => string.Format(o?.ToString() ?? string.Empty, property.Name.ToPascalCase(), property.Name.ToPascalCase().GetCsharpFriendlyName(), property.TypeName, property.TypeName.GetGenericArguments())),
                            "return this;"
                        }.ToLiteralCodeStatementBuilders().ToList()
                    };
                    foreach (var overload in overloads)
                    {
                        yield return new ClassMethodBuilder
                        {
                            Name = string.Format(settings.SetMethodNameFormatString, property.Name),
                            TypeName = $"{instance.Name}Builder",
                            Parameters = new[]
                            {
                                new ParameterBuilder
                                {
                                    Name = property.Name.ToPascalCase(),
                                    TypeName = string.Format(overload.TypeName, property.TypeName)
                                }
                            }.ToList(),
                            CodeStatements = new[]
                            {
                                string.Format(overload.Expression, property.Name.ToPascalCase(), property.TypeName.FixTypeName().GetCsharpFriendlyTypeName()),
                                "return this;"
                            }.ToLiteralCodeStatementBuilders().ToList()
                        };
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

        private static string FormatInstanceName(ITypeBase instance,
                                                 bool forCreate,
                                                 Func<ITypeBase, bool, string> formatInstanceTypeNameDelegate)
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

        private static IEnumerable<ClassPropertyBuilder> GetImmutableBuilderClassProperties(ITypeBase instance,
                                                                                            ImmutableBuilderClassSettings settings)
        {
            if (!settings.AddProperties)
            {
                yield break;
            }

            foreach (var property in instance.Properties)
            {
                yield return new ClassPropertyBuilder
                {
                    Name = property.Name,
                    TypeName = property.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableBuilderArgumentType, property.TypeName, o => string.Format(o?.ToString() ?? string.Empty, property.TypeName, property.TypeName.GetGenericArguments())).FixBuilderCollectionTypeName(settings.NewCollectionTypeName),
                }
                .AddGetterCodeStatements(new LiteralCodeStatementBuilder().WithStatement($"return _{property.Name.ToPascalCase()};"))
                .AddSetterCodeStatements(new LiteralCodeStatementBuilder().WithStatement($"_{property.Name.ToPascalCase()} = value;"));
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

        private static IClassProperty ChangeProperty(IClassProperty property,
                                                     IDictionary<string, string> applyGenericTypes,
                                                     bool changePropertiesToReadOnly)
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
                                    property.IsNullable,
                                    property.Visibility,
                                    property.GetterVisibility,
                                    property.SetterVisibility,
                                    property.InitializerVisibility,
                                    property.ExplicitInterfaceName,
                                    property.Metadata,
                                    property.Attributes,
                                    property.GetterCodeStatements,
                                    property.SetterCodeStatements,
                                    null);

        private static IClassMethod ChangeArgumentsAndReturnType(IClassMethod method, IDictionary<string, string> applyGenericTypes)
        {
            if (applyGenericTypes == null || applyGenericTypes.Count == 0)
            {
                return method;
            }

            return new ClassMethod
            (
                method.Name,
                ReplaceGenericType(method.TypeName, applyGenericTypes),
                method.Visibility,
                method.Static,
                method.Virtual,
                method.Abstract,
                method.Protected,
                method.Partial,
                method.Override,
                method.ExtensionMethod,
                method.Operator,
                method.IsNullable,
                method.ExplicitInterfaceName,
                method.Parameters.Select(p => ChangeParameter(p, applyGenericTypes)),
                method.Attributes,
                method.CodeStatements,
                method.Metadata
            );
        }

        private static IParameter ChangeParameter(IParameter property, IDictionary<string, string> applyGenericTypes)
        {
            if (applyGenericTypes == null || applyGenericTypes.Count == 0)
            {
                return property;
            }

            return new Parameter
            (
                property.Name,
                ReplaceGenericType(property.TypeName, applyGenericTypes),
                property.DefaultValue,
                property.IsNullable,
                property.Attributes,
                property.Metadata
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

        private static List<string> GenerateGenericTypeArguments(IDictionary<string, string> applyGenericTypes)
            => applyGenericTypes != null && applyGenericTypes.Count > 0
                ? applyGenericTypes.Values.ToList()
                : new List<string>();

        private static IEnumerable<ClassMethodBuilder> GetImmutableClassMethods(ITypeBase instance,
                                                                                string newCollectionTypeName,
                                                                                bool createWithMethod,
                                                                                bool implementIEquatable,
                                                                                bool extensionMethod)
        {
            if (createWithMethod)
            {
                yield return
                    new ClassMethodBuilder
                    {
                        Name = "With",
                        TypeName = instance.Name,
                        Static = extensionMethod,
                        CodeStatements =
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
                            .ToLiteralCodeStatementBuilders()
                            .ToList(),
                        Parameters =
                            (extensionMethod
                                ? new[] { new ParameterBuilder { Name = "instance", TypeName = instance.Name } }
                                : Enumerable.Empty<ParameterBuilder>())
                            .Concat
                            (
                                instance
                                    .Properties
                                    .Select
                                    (
                                        p => new ParameterBuilder
                                        {
                                            Name = p.Name.ToPascalCase().GetCsharpFriendlyName(),
                                            TypeName =p.Metadata
                                                .Concat(p.GetImmutableCollectionMetadata(newCollectionTypeName))
                                                .GetMetadataStringValue
                                                (
                                                    MetadataNames.CustomImmutableArgumentType,
                                                    p.TypeName.FixImmutableCollectionTypeName(newCollectionTypeName),
                                                    o => string.Format(o?.ToString() ?? string.Empty, p.TypeName.FixImmutableCollectionTypeName(newCollectionTypeName))
                                                ).GetCsharpFriendlyTypeName(),
                                            DefaultValue = new Literal($"default({p.Metadata.GetMetadataStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixImmutableCollectionTypeName(newCollectionTypeName)).GetCsharpFriendlyTypeName()})")
                                        }
                                    )
                            ).ToList(),
                        ExtensionMethod = extensionMethod
                    };
            }

            if (implementIEquatable)
            {
                yield return new ClassMethodBuilder
                {
                    Name = "Equals",
                    TypeName = typeof(bool).FullName,
                    Override = true,
                    Parameters = new[]
                    {
                        new ParameterBuilder
                        {
                            Name = "obj",
                            TypeName = typeof(object).FullName
                        }
                    }.ToList(),
                    CodeStatements = new[]
                    {
                        new LiteralCodeStatementBuilder { Statement = $"return Equals(obj as {instance.Name});" }
                    }.Cast<ICodeStatementBuilder>().ToList()
                };
                yield return new ClassMethodBuilder
                {
                    Name = $"IEquatable<{instance.Name}>.Equals",
                    TypeName = typeof(bool).FullName,
                    Parameters = new[]
                    {
                        new ParameterBuilder
                        {
                            Name = "other",
                            TypeName = instance.Name
                        }
                    }.ToList(),
                    CodeStatements = new[]
                    {
                        new LiteralCodeStatementBuilder
                        {
                            Statement = $"return other != null &&{Environment.NewLine}       {GetEqualsProperties(instance)};" 
                        }
                    }.Cast<ICodeStatementBuilder>().ToList()
                };
                yield return new ClassMethodBuilder
                {
                    Name = "GetHashCode",
                    TypeName = typeof(int).FullName,
                    Override = true,
                    CodeStatements = new[] { "int hashCode = 235838129;" }
                       .Concat(instance.Properties.Select(p => Type.GetType(p.TypeName.FixTypeName())?.IsValueType == true
                            ? $"hashCode = hashCode * -1521134295 + {p.Name}.GetHashCode();"
                            : $"hashCode = hashCode * -1521134295 + EqualityComparer<{p.TypeName.FixTypeName()}>.Default.GetHashCode({p.Name});"))
                        .Concat(new[] { "return hashCode;" })
                        .Select(x => new LiteralCodeStatementBuilder { Statement = x })
                        .Cast<ICodeStatementBuilder>()
                        .ToList()
                };
                yield return new ClassMethodBuilder
                {
                    Name = "==",
                    TypeName = typeof(bool).FullName,
                    Static = true,
                    Operator = true,
                    Parameters = new[]
                    {
                        new ParameterBuilder { Name = "left", TypeName = instance.Name },
                        new ParameterBuilder { Name = "right", TypeName = instance.Name }
                    }.ToList(),
                    CodeStatements = new[]
                    {
                        new LiteralCodeStatementBuilder { Statement = $"return EqualityComparer<{instance.Name}>.Default.Equals(left, right);" }
                    }.Cast<ICodeStatementBuilder>().ToList()
                };
                yield return new ClassMethodBuilder
                {
                    Name = "!=",
                    TypeName = typeof(bool).FullName,
                    Static = true,
                    Operator = true,
                    Parameters = new[]
                    {
                        new ParameterBuilder { Name = "left", TypeName = instance.Name },
                        new ParameterBuilder { Name = "right", TypeName = instance.Name }
                    }.ToList(),
                    CodeStatements = new[]
                    {
                        new LiteralCodeStatementBuilder { Statement = "return !(left == right);" }
                    }.Cast<ICodeStatementBuilder>().ToList()
                };
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
