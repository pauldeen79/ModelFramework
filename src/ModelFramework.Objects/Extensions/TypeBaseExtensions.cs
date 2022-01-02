using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using CrossCutting.Common.Extensions;
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
            => instance is IClass cls
                ? GetInheritedClassesForClass(cls)
                : GetInheritedClassesForTypeBase(instance);

        private static string GetInheritedClassesForClass(IClass cls)
        {
            var lst = new List<string>();
            if (!string.IsNullOrEmpty(cls.BaseClass))
            {
                lst.Add(cls.BaseClass);
            }

            lst.AddRange(cls.Interfaces);

            return lst.Count == 0
                ? string.Empty
                : $" : {string.Join(", ", lst.Select(x => x.GetCsharpFriendlyTypeName()))}";
        }

        private static string GetInheritedClassesForTypeBase(ITypeBase instance)
            => !instance.Interfaces.Any()
                ? string.Empty
                : $" : {string.Join(", ", instance.Interfaces.Select(x => x.GetCsharpFriendlyTypeName()))}";

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

        public static IInterface ToInterfaceClass(this ITypeBase instance, InterfaceSettings settings)
            => instance.ToInterfaceBuilder(settings).Build();

        public static InterfaceBuilder ToInterfaceBuilder(this ITypeBase instance, InterfaceSettings settings)
            => new InterfaceBuilder()
                .WithName("I" + instance.Name)
                .WithNamespace(instance.Namespace)
                .WithVisibility(instance.Visibility)
                .WithPartial(instance.Partial)
                .AddInterfaces(instance.Interfaces.Where(i => i != "I" + instance.Name && i != instance.Namespace + ".I" + instance.Name))
                .AddProperties
                (
                    settings.PropertyFilter == null
                        ? instance.Properties.Select(p => ChangeProperty(p, settings.ApplyGenericTypes, settings.ChangePropertiesToReadOnly))
                                             .Select(x => new ClassPropertyBuilder(x))
                        : instance.Properties.Where(settings.PropertyFilter)
                                             .Select(p => ChangeProperty(p, settings.ApplyGenericTypes, settings.ChangePropertiesToReadOnly))
                                             .Select(x => new ClassPropertyBuilder(x))
                )
                .AddMethods
                (
                    settings.MethodFilter == null
                        ? instance.Methods
                            .Where(m => !m.Static
                                && !m.Override
                                && !m.Operator
                                && !m.IsInterfaceMethod()
                                && string.IsNullOrEmpty(m.ExplicitInterfaceName)
                                && m.Visibility == Visibility.Public)
                            .Select(m => ChangeArgumentsAndReturnType(m, settings.ApplyGenericTypes))
                            .Select(x => new ClassMethodBuilder(x))
                        : instance.Methods.Where(settings.MethodFilter)
                                          .Select(m => ChangeArgumentsAndReturnType(m, settings.ApplyGenericTypes))
                                          .Select(x => new ClassMethodBuilder(x))
                )
                .AddMetadata
                (
                    settings.MetadataFilter == null
                        ? instance.Metadata.Select(x => new MetadataBuilder(x))
                        : instance.Metadata.Where(settings.MetadataFilter)
                                           .Select(x => new MetadataBuilder(x))
                )
                .AddAttributes
                (
                    settings.AttributeFilter == null
                        ? instance.Attributes.Select(x => new AttributeBuilder(x))
                        : instance.Attributes.Where(settings.AttributeFilter)
                                             .Select(x => new AttributeBuilder(x))
                )
                .AddGenericTypeArguments(GenerateGenericTypeArguments(settings.ApplyGenericTypes));

        public static IClass ToImmutableClass(this ITypeBase instance, ImmutableClassSettings settings)
            => instance.ToImmutableClassBuilder(settings).Build();

        public static ClassBuilder ToImmutableClassBuilder(this ITypeBase instance, ImmutableClassSettings settings)
        {
            if (!instance.Properties.Any())
            {
                throw new InvalidOperationException("To create an immutable class, there must be at least one property");
            }

            return new ClassBuilder()
                .WithName(instance.Name)
                .WithNamespace(instance.Namespace)
                .AddProperties
                (
                    instance
                        .Properties
                        .Select
                        (
                            p => new ClassPropertyBuilder()
                                .WithName(p.Name)
                                .WithTypeName(p.TypeName.FixImmutableCollectionTypeName(settings.NewCollectionTypeName))
                                .WithStatic(p.Static)
                                .WithVirtual(p.Virtual)
                                .WithAbstract(p.Abstract)
                                .WithProtected(p.Protected)
                                .WithOverride(p.Override)
                                .WithHasGetter(p.HasGetter)
                                .WithHasInitializer(p.HasInitializer)
                                .AsReadOnly()
                                .WithIsNullable(p.IsNullable)
                                .WithVisibility(p.Visibility)
                                .WithGetterVisibility(p.GetterVisibility)
                                .WithSetterVisibility(p.SetterVisibility)
                                .WithInitializerVisibility(p.InitializerVisibility)
                                .WithExplicitInterfaceName(p.ExplicitInterfaceName)
                                .AddMetadata
                                (
                                    p.Metadata
                                        .Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName))
                                        .Select(x => new MetadataBuilder(x))
                                )
                                .AddAttributes(p.Attributes.Select(x => new AttributeBuilder(x)))
                                .AddGetterCodeStatements(p.GetterCodeStatements.Select(x => x.CreateBuilder()))
                                .AddSetterCodeStatements(p.SetterCodeStatements.Select(x => x.CreateBuilder()))
                                .AddInitializerCodeStatements(p.InitializerCodeStatements.Select(x => x.CreateBuilder()))
                        )
                )
                .AddConstructors
                (
                    new ClassConstructorBuilder()
                        .AddParameters
                        (
                            instance.Properties.Select(p => new ParameterBuilder()
                                .WithName(p.Name.ToPascalCase())
                                .WithTypeName(string.Format
                                (
                                    p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName))
                                                       .GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixImmutableCollectionTypeName(settings.NewCollectionTypeName)),
                                    p.Name.ToPascalCase().GetCsharpFriendlyName(),
                                    p.TypeName.GetGenericArguments()
                                )
                        )))
                        .AddCodeStatements
                        (
                            instance.Properties.Select
                            (
                                p => new LiteralCodeStatementBuilder()
                                    .WithStatement
                                    (
                                        string.Format
                                        (
                                            $"this.{p.Name.GetCsharpFriendlyName()} = {p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)).GetStringValue(MetadataNames.CustomImmutableDefaultValue, p.Name.ToPascalCase().GetCsharpFriendlyName())};",
                                            p.Name.ToPascalCase().GetCsharpFriendlyName(),
                                            p.TypeName.GetGenericArguments()
                                        )
                                     )
                            )
                        )
                        .AddCodeStatements
                        (
                            settings.ValidateArgumentsInConstructor
                                ? new[]
                                    {
                                        new LiteralCodeStatementBuilder()
                                            .WithStatement("System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new ValidationContext(this, null, null), true);")
                                    }
                                : Enumerable.Empty<LiteralCodeStatementBuilder>()
                        )
                )
                .AddMethods
                (
                    GetImmutableClassMethods(instance,
                                             settings.NewCollectionTypeName,
                                             settings.CreateWithMethod,
                                             settings.ImplementIEquatable)
                )
                .AddInterfaces
                (
                    settings.ImplementIEquatable
                        ? new[] { $"IEquatable<{instance.Name}>" }
                        : Enumerable.Empty<string>()
                )
                .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)));
        }

        public static IClass ToPocoClass(this ITypeBase instance,
                                         string newCollectionTypeName = "System.Collections.Generic.ICollection")
            => instance.ToPocoClassBuilder(newCollectionTypeName).Build();

        public static ClassBuilder ToPocoClassBuilder(this ITypeBase instance,
                                                      string newCollectionTypeName = "System.Collections.Generic.ICollection")
            => new ClassBuilder()
                .WithName(instance.Name)
                .WithNamespace(instance.Namespace)
                .AddProperties
                (
                    instance
                        .Properties
                        .Select
                        (
                            p => new ClassPropertyBuilder()
                                .WithName(p.Name)
                                .WithTypeName(p.TypeName.FixBuilderCollectionTypeName(newCollectionTypeName))
                                .WithStatic(p.Static)
                                .WithVirtual(p.Virtual)
                                .WithAbstract(p.Abstract)
                                .WithProtected(p.Protected)
                                .WithOverride(p.Override)
                                .WithHasGetter(p.HasGetter)
                                .WithHasSetter()
                                .WithHasInitializer(false)
                                .WithIsNullable(p.IsNullable)
                                .WithVisibility(p.Visibility)
                                .WithGetterVisibility(p.GetterVisibility)
                                .WithSetterVisibility(p.SetterVisibility)
                                .WithInitializerVisibility(p.InitializerVisibility)
                                .WithExplicitInterfaceName(p.ExplicitInterfaceName)
                                .AddMetadata
                                (
                                    p.Metadata
                                        .Concat(p.GetBuilderCollectionMetadata(newCollectionTypeName))
                                        .Select(x => new MetadataBuilder(x))
                                )
                                .AddAttributes(p.Attributes.Select(x => new AttributeBuilder(x)))
                                .AddGetterCodeStatements(p.GetterCodeStatements.Select(x => x.CreateBuilder()))
                                .AddSetterCodeStatements(p.SetterCodeStatements.Select(x => x.CreateBuilder()))
                        )
                )
                .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)));

        public static IClass ToObservableClass(this ITypeBase instance,
                                               string newCollectionTypeName = "System.Collections.ObjectModel.ObservableCollection")
            => instance.ToObservableClassBuilder(newCollectionTypeName).Build();

        public static ClassBuilder ToObservableClassBuilder(this ITypeBase instance,
                                                            string newCollectionTypeName = "System.Collections.ObjectModel.ObservableCollection")
            => new ClassBuilder()
                .WithName(instance.Name)
                .WithNamespace(instance.Namespace)
                .AddProperties
                (
                    instance
                        .Properties
                        .Select
                        (
                            p => new ClassPropertyBuilder()
                                .WithName(p.Name)
                                .WithTypeName(p.TypeName.FixObservableCollectionTypeName(newCollectionTypeName))
                                .WithStatic(p.Static)
                                .WithVirtual(p.Virtual)
                                .WithAbstract(p.Abstract)
                                .WithProtected(p.Protected)
                                .WithOverride(p.Override)
                                .WithHasGetter(p.HasGetter)
                                .WithHasSetter()
                                .WithHasInitializer(false)
                                .WithIsNullable(p.IsNullable)
                                .WithVisibility(p.Visibility)
                                .WithGetterVisibility(p.GetterVisibility)
                                .WithSetterVisibility(p.SetterVisibility)
                                .WithInitializerVisibility(p.InitializerVisibility)
                                .AddGetterCodeStatements(p.FixObservablePropertyGetterBody(newCollectionTypeName).Select(x => x.CreateBuilder()))
                                .AddSetterCodeStatements(p.FixObservablePropertySetterBody(newCollectionTypeName).Select(x => x.CreateBuilder()))
                                .WithExplicitInterfaceName(p.ExplicitInterfaceName)
                                .AddMetadata(p.Metadata.Concat(p.GetObservableCollectionMetadata(newCollectionTypeName))
                                                       .Select(x => new MetadataBuilder(x)))
                                .AddAttributes(p.Attributes.Select(x => new AttributeBuilder(x)))
                        )
                )
                .AddFields
                (
                    ((instance as IClass)?.Fields?.Select(x => new ClassFieldBuilder(x)) ?? Enumerable.Empty<ClassFieldBuilder>())
                    .Concat
                    (
                        instance.Properties
                            .Where(p => !p.GetterCodeStatements.Any() && !p.SetterCodeStatements.Any())
                            .Select
                            (
                                p => new ClassFieldBuilder()
                                    .WithName("_" + p.Name.ToPascalCase())
                                    .WithTypeName(p.TypeName)
                                    .WithIsNullable(p.IsNullable)
                            )
                    )
                )
                .AddConstructors
                (
                    new ClassConstructorBuilder()
                        .AddCodeStatements
                        (
                            instance.Properties
                                .Where(p => p.TypeName.FixObservableCollectionTypeName(newCollectionTypeName).StartsWith("System.Collections.ObjectModel.ObservableCollection<"))
                                .Select(p => new LiteralCodeStatementBuilder()
                                    .WithStatement($"this.{p.Name} = new {p.TypeName.FixObservableCollectionTypeName(newCollectionTypeName).GetCsharpFriendlyTypeName()}();"))
                        )
                )
                .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)));

        public static IClass ToImmutableBuilderClass(this ITypeBase instance, ImmutableBuilderClassSettings settings)
            => instance.ToImmutableBuilderClassBuilder(settings).Build();

        public static ClassBuilder ToImmutableBuilderClassBuilder(this ITypeBase instance, ImmutableBuilderClassSettings settings)
        {
            if (!instance.Properties.Any())
            {
                throw new InvalidOperationException("To create an immutable builder class, there must be at least one property");
            }

            return new ClassBuilder()
                .WithName(instance.Name + "Builder")
                .WithNamespace(instance.Namespace)
                .WithPartial(instance.Partial)
                .AddConstructors(GetImmutableBuilderClassConstructors(instance, settings))
                .AddMethods(GetImmutableBuilderClassMethods(instance, settings))
                .AddProperties(GetImmutableBuilderClassProperties(instance, settings))
                .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)))
                .AddFields(((instance as IClass)?.Fields)?.Select(x => new ClassFieldBuilder(x))?.ToList() ?? new List<ClassFieldBuilder>());
        }

        public static string GetGenericTypeArgumentsString(this ITypeBase instance)
            => instance.GenericTypeArguments != null && instance.GenericTypeArguments.Count > 0
                ? $"<{string.Join(", ", instance.GenericTypeArguments)}>"
                : string.Empty;

        private static IEnumerable<ClassConstructorBuilder> GetImmutableBuilderClassConstructors(ITypeBase instance,
                                                                                                 ImmutableBuilderClassSettings settings)
        {
            yield return new ClassConstructorBuilder()
                .AddCodeStatements
                (
                    instance.Properties
                        .Where(p => p.TypeName.IsCollectionTypeName())
                        .Select(p => new LiteralCodeStatementBuilder()
                            .WithStatement($"{p.Name} = new {GetImmutableBuilderClassConstructorInitializer(settings, p)}();"))
                )
                .AddCodeStatements
                (
                    instance.Properties
                        .Where(p => !settings.AddNullChecks && !p.TypeName.IsCollectionTypeName() && !p.IsNullable)
                        .Select(p => new LiteralCodeStatementBuilder().WithStatement($"{p.Name} = {p.GetDefaultValue()};"))
                );

            if (settings.ConstructorSettings.AddCopyConstructor)
            {
                yield return new ClassConstructorBuilder()
                    .AddParameters
                    (
                        new ParameterBuilder()
                            .WithName("source")
                            .WithTypeName(FormatInstanceName(instance, false, settings.FormatInstanceTypeNameDelegate))
                    )
                    .AddCodeStatements
                    (
                        instance.Properties
                            .Where(p => p.TypeName.IsCollectionTypeName())
                            .Select(p => new LiteralCodeStatementBuilder().WithStatement($"{p.Name} = new {GetCopyConstructorInitializeExpression(settings, p)}();"))
                            .Concat(instance.Properties.Select(p => new LiteralCodeStatementBuilder().WithStatement(p.CreateImmutableBuilderInitializationCode(settings.AddNullChecks))))
                    );
            }

            if (settings.ConstructorSettings.AddConstructorWithAllProperties)
            {
                var cls = instance as IClass;
                var ctors = cls?.Constructors ?? new ValueCollection<IClassConstructor>();
                var properties = settings.Poco
                    ? instance.Properties
                    : ctors.First(x => x.Parameters.Count > 0).Parameters
                        .Select(x => instance.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
                        .Where(x => x != null);

                yield return new ClassConstructorBuilder()
                    .AddParameters(properties.Select(p => new ParameterBuilder()
                        .WithName(p.Name.ToPascalCase())
                        .WithTypeName(string.Format
                        (
                            p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)).GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixImmutableCollectionTypeName(settings.NewCollectionTypeName)),
                            p.Name.ToPascalCase().GetCsharpFriendlyName(),
                            p.TypeName.GetGenericArguments()
                        ))
                        .WithIsNullable(p.IsNullable)
                    ))
                    .AddCodeStatements
                    (
                        properties
                            .Where(p => p.TypeName.IsCollectionTypeName())
                            .Select(p => new LiteralCodeStatementBuilder().WithStatement($"{p.Name} = new {GetConstructorInitializeExpressionForCollection(settings, p)}();"))
                            .Concat
                            (
                                instance.Properties.Select
                                (
                                    p => p.TypeName.IsCollectionTypeName()
                                        ? new LiteralCodeStatementBuilder().WithStatement(CreateConstructorStatementForCollection(p, settings))
                                        : new LiteralCodeStatementBuilder().WithStatement($"{p.Name} = {p.Name.ToPascalCase().GetCsharpFriendlyName()};")
                                )
                            )
                    );
            }
        }

        private static string GetConstructorInitializeExpressionForCollection(ImmutableBuilderClassSettings settings, IClassProperty p)
            => string.Format(p.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, p.TypeName),
                             p.TypeName,
                             p.TypeName.GetGenericArguments()
                            ).FixBuilderCollectionTypeName(settings.NewCollectionTypeName)
                             .GetCsharpFriendlyTypeName();

        private static string GetCopyConstructorInitializeExpression(ImmutableBuilderClassSettings settings, IClassProperty p)
            => string.Format(p.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, p.TypeName),
                             p.TypeName,
                             p.TypeName.GetGenericArguments()
                            ).FixBuilderCollectionTypeName(settings.NewCollectionTypeName)
                             .GetCsharpFriendlyTypeName();

        private static string GetImmutableBuilderClassConstructorInitializer(ImmutableBuilderClassSettings settings, IClassProperty p)
            => string.Format
            (
                p.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, p.TypeName),
                p.TypeName,
                p.TypeName.GetGenericArguments()
            ).FixBuilderCollectionTypeName(settings.NewCollectionTypeName)
             .GetCsharpFriendlyTypeName();

        private static string CreateConstructorStatementForCollection(IClassProperty p, ImmutableBuilderClassSettings settings)
            => settings.AddNullChecks
                ? $"if ({p.Name.ToPascalCase()} != null) {p.Name}.AddRange({p.Name.ToPascalCase()});"
                : $"{p.Name}.AddRange({p.Name.ToPascalCase()});";

        private static IEnumerable<ClassMethodBuilder> GetImmutableBuilderClassMethods(ITypeBase instance,
                                                                                       ImmutableBuilderClassSettings settings)
        {
            var openSign = GetImmutableBuilderPocoOpenSign(settings.Poco);
            var closeSign = GetImmutableBuilderPocoCloseSign(settings.Poco);
            yield return new ClassMethodBuilder()
                .WithName("Build")
                .WithTypeName(FormatInstanceName(instance, false, settings.FormatInstanceTypeNameDelegate))
                .AddCodeStatements
                (
                    new LiteralCodeStatementBuilder()
                        .WithStatement($"return new {FormatInstanceName(instance, true, settings.FormatInstanceTypeNameDelegate)}{openSign}{GetBuildMethodParameters(instance, settings.Poco)}{closeSign};")
                );

            foreach (var property in instance.Properties)
            {
                var overloads = GetOverloads(property);
                if (property.TypeName.IsCollectionTypeName())
                {
                    // collection
                    yield return new ClassMethodBuilder()
                        .WithName($"Add{property.Name}")
                        .WithTypeName($"{instance.Name}Builder")
                        .AddParameters
                        (
                            new ParameterBuilder()
                                .WithName(property.Name.ToPascalCase())
                                .WithTypeName
                                (
                                    string.Format
                                    (
                                        property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                                        property.TypeName,
                                        property.TypeName.GetGenericArguments()
                                    ).FixBuilderCollectionTypeName("System.Collections.Generic.IEnumerable")
                                )
                                .WithIsNullable(property.IsNullable)
                        )
                        .AddCodeStatements
                        (
                            new LiteralCodeStatementBuilder()
                                .WithStatement($"return Add{property.Name}({property.Name.ToPascalCase()}.ToArray());")
                        );
                    yield return new ClassMethodBuilder()
                        .WithName($"Add{property.Name}")
                        .WithTypeName($"{instance.Name}Builder")
                        .AddParameters
                        (
                            new ParameterBuilder()
                                .WithName(property.Name.ToPascalCase())
                                .WithTypeName
                                (
                                    "params " + string.Format
                                    (
                                        property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                                        property.TypeName,
                                        property.TypeName.GetGenericArguments()
                                    ).FixTypeName()
                                     .ConvertTypeNameToArray()
                                )
                                .WithIsNullable(property.IsNullable)
                        )
                        .AddCodeStatements(GetImmutableBuilderAddMethodStatements(settings, property));

                    foreach (var overload in overloads)
                    {
                        yield return new ClassMethodBuilder()
                            .WithName($"Add{property.Name}")
                            .WithTypeName($"{instance.Name}Builder")
                            .AddParameters
                            (
                                new ParameterBuilder()
                                    .WithName(property.Name.ToPascalCase())
                                    .WithTypeName(string.Format(overload.ArgumentType,
                                                                property.TypeName,
                                                                property.TypeName.GetGenericArguments()).FixBuilderCollectionTypeName("System.Collections.Generic.IEnumerable"))
                                    .WithIsNullable(property.IsNullable)
                            )
                            .AddCodeStatements
                            (
                                new LiteralCodeStatementBuilder()
                                    .WithStatement($"return Add{property.Name}({property.Name.ToPascalCase()}.ToArray());")
                            );
                        yield return new ClassMethodBuilder()
                            .WithName($"Add{property.Name}")
                            .WithTypeName($"{instance.Name}Builder")
                            .AddParameters
                            (
                                new ParameterBuilder()
                                    .WithName(property.Name.ToPascalCase())
                                    .WithTypeName("params " + string.Format(overload.ArgumentType,
                                                                            property.TypeName,
                                                                            property.TypeName.GetGenericArguments()).ConvertTypeNameToArray())
                                    .WithIsNullable(property.IsNullable)
                            )
                            .AddCodeStatements(GetImmutableBuilderAddOverloadMethodStatements(settings, property, overload.InitializeExpression));
                    }
                }
                else
                {
                    // single
                    yield return new ClassMethodBuilder()
                    .WithName(string.Format(settings.SetMethodNameFormatString, property.Name))
                    .WithTypeName($"{instance.Name}Builder")
                    .AddParameters
                    (
                        new ParameterBuilder()
                            .WithName(property.Name.ToPascalCase())
                            .WithTypeName
                            (
                                string.Format
                                (
                                    property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                                    property.TypeName,
                                    property.TypeName.GetGenericArguments()
                                )
                            )
                            .WithIsNullable(property.IsNullable)
                            .WithDefaultValue(property.Metadata.GetValue<object?>(MetadataNames.CustomBuilderWithDefaultPropertyValue, () => null))
                    )
                    .AddCodeStatements
                    (
                        new LiteralCodeStatementBuilder().WithStatement(string.Format
                        (
                            property.Metadata.GetStringValue(MetadataNames.CustomBuilderWithExpression, $"{property.Name} = {property.Name.ToPascalCase().GetCsharpFriendlyName()};"),
                            property.Name,
                            property.Name.ToPascalCase(),
                            property.Name.ToPascalCase().GetCsharpFriendlyName(),
                            property.TypeName,
                            property.TypeName.GetGenericArguments(),
                            "{",
                            "}"
                        )),
                        new LiteralCodeStatementBuilder().WithStatement("return this;")
                    );
                    foreach (var overload in overloads)
                    {
                        yield return new ClassMethodBuilder()
                            .WithName(string.Format(overload.MethodName.WhenNullOrEmpty(settings.SetMethodNameFormatString), property.Name))
                            .WithTypeName($"{instance.Name}Builder")
                            .AddParameters
                            (
                                new ParameterBuilder()
                                    .WithName(overload.ArgumentName.WhenNullOrEmpty(() => property.Name.ToPascalCase()))
                                    .WithTypeName(string.Format(overload.ArgumentType, property.TypeName))
                                    .WithIsNullable(property.IsNullable)
                            )
                            .AddCodeStatements
                            (
                                new LiteralCodeStatementBuilder().WithStatement(string.Format(overload.InitializeExpression,
                                                                                              property.Name.ToPascalCase(),
                                                                                              property.TypeName.FixTypeName().GetCsharpFriendlyTypeName(),
                                                                                              property.Name)),
                                new LiteralCodeStatementBuilder().WithStatement("return this;")
                            );
                    }
                }
            }
        }

        private static IEnumerable<Overload> GetOverloads(IClassProperty property)
        {
            var argumentTypes = property.Metadata.GetStringValues(MetadataNames.CustomBuilderWithOverloadArgumentType).ToArray();
            var initializeExpressions = property.Metadata.GetStringValues(MetadataNames.CustomBuilderWithOverloadInitializeExpression).ToArray();
            var methodNames = property.Metadata.GetStringValues(MetadataNames.CustomBuilderWithOverloadMethodName).ToArray();
            var argumentNames = property.Metadata.GetStringValues(MetadataNames.CustomBuilderWithOverloadArgumentName).ToArray();

            if (argumentTypes.Length > 0 && methodNames.Length == 0)
            {
                methodNames = argumentTypes.Select(_ => "{0}").ToArray();
            }

            if (argumentTypes.Length > 0 && argumentNames.Length == 0)
            {
                argumentNames = argumentTypes.Select(_ => "{0}").ToArray();
            }

            if (argumentTypes.Length != initializeExpressions.Length
                || argumentTypes.Length != methodNames.Length
                || argumentTypes.Length != argumentNames.Length)
            {
                throw new InvalidOperationException("Metadata for immutable builder overload method is incorrect. Metadata needs to be available in the same amount for all metadata types");
            }

            return
                from argumentType in argumentTypes
                from initializeExpression in initializeExpressions
                from methodName in methodNames
                from argumentName in argumentNames
                select new Overload(argumentType, initializeExpression, methodName, argumentName);
        }

        private static List<ICodeStatementBuilder> GetImmutableBuilderAddMethodStatements(ImmutableBuilderClassSettings settings, IClassProperty property)
            => settings.AddNullChecks
                ? new[]
                    {
                        $"if ({property.Name.ToPascalCase()} != null)",
                        "{",
                        string.Format
                        (
                            property.Metadata.GetStringValue(MetadataNames.CustomBuilderAddExpression, $"    {property.Name}.AddRange({property.Name.ToPascalCase()});"),
                            property.Name.ToPascalCase(),
                            property.TypeName,
                            property.TypeName.GetGenericArguments()
                        ),
                        "}",
                        "return this;"
                    }.ToLiteralCodeStatementBuilders().ToList()
                : new[]
                    {
                        string.Format
                        (
                            property.Metadata.GetStringValue(MetadataNames.CustomBuilderAddExpression, $"{property.Name}.AddRange({property.Name.ToPascalCase()});"),
                            property.Name.ToPascalCase(),
                            property.TypeName,
                            property.TypeName.GetGenericArguments()
                        ),
                        "return this;"
                    }.ToLiteralCodeStatementBuilders().ToList();

        private static List<ICodeStatementBuilder> GetImmutableBuilderAddOverloadMethodStatements(ImmutableBuilderClassSettings settings,
                                                                                                  IClassProperty property,
                                                                                                  string overloadExpression)
            => settings.AddNullChecks
                ? (new[]
                {
                    $"if ({property.Name.ToPascalCase()} != null)",
                        "{",
                        string.Format(overloadExpression,
                                      property.Name.ToPascalCase(),
                                      property.TypeName.FixTypeName(),
                                      property.TypeName.GetGenericArguments(),
                                      CreateIndentForImmuableBuilderAddOverloadMethodStatement(settings),
                                      property.Name),
                        "    }",
                        "}",
                        "return this;"
                }).ToLiteralCodeStatementBuilders().ToList()
                : (new[]
                {
                        string.Format(overloadExpression,
                                      property.Name.ToPascalCase(),
                                      property.TypeName.FixTypeName(),
                                      property.TypeName.GetGenericArguments(),
                                      CreateIndentForImmuableBuilderAddOverloadMethodStatement(settings),
                                      property.Name),
                        "return this;"
                }).ToLiteralCodeStatementBuilders().ToList();

        private static string CreateIndentForImmuableBuilderAddOverloadMethodStatement(ImmutableBuilderClassSettings settings)
            => settings.AddNullChecks
                ? "        "
                : "    ";

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
                                                 Func<ITypeBase, bool, string>? formatInstanceTypeNameDelegate)
        {
            if (formatInstanceTypeNameDelegate != null)
            {
                var retVal = formatInstanceTypeNameDelegate(instance, forCreate);
                if (!string.IsNullOrEmpty(retVal))
                {
                    return retVal;
                }
            }

            var ns = string.IsNullOrEmpty(instance.Namespace)
                ? string.Empty
                : instance.Namespace + ".";

            return (ns + instance.Name).GetCsharpFriendlyTypeName();
        }

        private static IEnumerable<ClassPropertyBuilder> GetImmutableBuilderClassProperties(ITypeBase instance,
                                                                                            ImmutableBuilderClassSettings settings)
            => instance.Properties.Select(property => new ClassPropertyBuilder()
                .WithName(property.Name)
                .WithTypeName
                (
                    string.Format
                    (
                        property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                        property.TypeName,
                        property.TypeName.GetGenericArguments()
                    ).FixBuilderCollectionTypeName(settings.NewCollectionTypeName)
                )
                .WithIsNullable(property.IsNullable)
                .AddAttributes(property.Attributes.Select(x => new AttributeBuilder(x)))
                .AddMetadata(property.Metadata.Select(x => new MetadataBuilder(x)))
                .AddGetterCodeStatements(property.GetterCodeStatements.Select(x => x.CreateBuilder()))
                .AddSetterCodeStatements(property.SetterCodeStatements.Select(x => x.CreateBuilder()))
            );

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
                ? new Func<IClassProperty, string>(p => $"{p.Name} = {p.Name}")
                : new Func<IClassProperty, string>(p => $"{p.Name}");

            return string.Join(", ", properties.Select(p => string.Format(p.Metadata.GetStringValue(MetadataNames.CustomBuilderMethodParameterExpression, defaultValueDelegate(p)),
                                                                          p.Name,
                                                                          p.Name.ToPascalCase())));
        }

        private static IClassProperty ChangeProperty(IClassProperty property,
                                                     IDictionary<string, string>? applyGenericTypes,
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
                                    Enumerable.Empty<ICodeStatement>());

        private static IClassMethod ChangeArgumentsAndReturnType(IClassMethod method, IDictionary<string, string>? applyGenericTypes)
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

        private static IParameter ChangeParameter(IParameter parameter, IDictionary<string, string>? applyGenericTypes)
        {
            if (applyGenericTypes == null || applyGenericTypes.Count == 0)
            {
                return parameter;
            }

            return new Parameter
            (
                parameter.Name,
                ReplaceGenericType(parameter.TypeName, applyGenericTypes),
                parameter.DefaultValue,
                parameter.IsNullable,
                parameter.IsParamArray,
                parameter.Attributes,
                parameter.Metadata
            );
        }

        private static string ReplaceGenericType(string typeName, IDictionary<string, string>? applyGenericTypes)
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

        private static List<string> GenerateGenericTypeArguments(IDictionary<string, string>? applyGenericTypes)
            => applyGenericTypes != null && applyGenericTypes.Count > 0
                ? applyGenericTypes.Values.ToList()
                : new List<string>();

        private static IEnumerable<ClassMethodBuilder> GetImmutableClassMethods(ITypeBase instance,
                                                                                string newCollectionTypeName,
                                                                                bool createWithMethod,
                                                                                bool implementIEquatable)
        {
            if (createWithMethod)
            {
                yield return
                    new ClassMethodBuilder()
                        .WithName("With")
                        .WithTypeName(instance.Name)
                        .AddCodeStatements
                        (
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
                                    .Select(p => $"    {p.Name.ToPascalCase()} == default({string.Format(p.Metadata.GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName), p.TypeName).GetCsharpFriendlyTypeName()}) ? this.{p.Name} : {string.Format(p.OriginalMetadata.GetStringValue(MetadataNames.CustomImmutableDefaultValue, p.Name.ToPascalCase()), p.Name.ToPascalCase())}{p.Suffix}")
                            )
                            .Concat
                            (
                                new[] { ");" }
                            )
                            .ToLiteralCodeStatementBuilders()
                        )
                        .AddParameters
                        (
                            instance
                                .Properties
                                .Select
                                (
                                    p => new ParameterBuilder()
                                        .WithName(p.Name.ToPascalCase().GetCsharpFriendlyName())
                                        .WithTypeName(string.Format
                                        (
                                            p.Metadata.Concat(p.GetImmutableCollectionMetadata(newCollectionTypeName)).GetStringValue
                                            (
                                                MetadataNames.CustomImmutableArgumentType,
                                                p.TypeName.FixImmutableCollectionTypeName(newCollectionTypeName)
                                            ), p.TypeName.FixImmutableCollectionTypeName(newCollectionTypeName)
                                        ).GetCsharpFriendlyTypeName())
                                        .WithDefaultValue(new Literal($"default({p.Metadata.GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixImmutableCollectionTypeName(newCollectionTypeName)).GetCsharpFriendlyTypeName()})"))
                                )
                        );
            }

            if (implementIEquatable)
            {
                yield return new ClassMethodBuilder()
                    .WithName("Equals")
                    .WithType(typeof(bool))
                    .WithOverride()
                    .AddParameters
                    (
                        new ParameterBuilder().WithName("obj").WithType(typeof(object))
                    )
                    .AddCodeStatements
                    (
                        new LiteralCodeStatementBuilder().WithStatement($"return Equals(obj as {instance.Name});")
                    );
                yield return new ClassMethodBuilder()
                    .WithName($"IEquatable<{instance.Name}>.Equals")
                    .WithType(typeof(bool))
                    .AddParameters
                    (
                        new ParameterBuilder().WithName("other").WithTypeName(instance.Name)
                    )
                    .AddCodeStatements
                    (
                        new LiteralCodeStatementBuilder()
                            .WithStatement($"return other != null &&{Environment.NewLine}       {GetEqualsProperties(instance)};")
                    );
                yield return new ClassMethodBuilder()
                    .WithName("GetHashCode")
                    .WithType(typeof(int))
                    .WithOverride()
                    .AddCodeStatements
                    (
                        new[] { "int hashCode = 235838129;" }
                        .Concat(instance.Properties.Select(p => Type.GetType(p.TypeName.FixTypeName())?.IsValueType == true
                            ? $"hashCode = hashCode * -1521134295 + {p.Name}.GetHashCode();"
                            : $"hashCode = hashCode * -1521134295 + EqualityComparer<{p.TypeName.FixTypeName()}>.Default.GetHashCode({p.Name});"))
                        .Concat(new[] { "return hashCode;" })
                        .Select(x => new LiteralCodeStatementBuilder { Statement = x })
                        .Cast<ICodeStatementBuilder>()
                    );
                yield return new ClassMethodBuilder()
                    .WithName("==")
                    .WithType(typeof(bool))
                    .WithStatic()
                    .WithOperator()
                    .AddParameters
                    (
                        new ParameterBuilder { Name = "left", TypeName = instance.Name },
                        new ParameterBuilder { Name = "right", TypeName = instance.Name }
                    )
                    .AddCodeStatements
                    (
                        new LiteralCodeStatementBuilder().WithStatement($"return EqualityComparer<{instance.Name}>.Default.Equals(left, right);")
                    );
                yield return new ClassMethodBuilder()
                    .WithName("!=")
                    .WithType(typeof(bool))
                    .WithStatic()
                    .WithOperator()
                    .AddParameters
                    (
                        new ParameterBuilder().WithName("left").WithTypeName(instance.Name),
                        new ParameterBuilder().WithName("right").WithTypeName(instance.Name)
                    )
                    .AddCodeStatements
                    (
                        new LiteralCodeStatementBuilder().WithStatement("return !(left == right);")
                    );
            }
        }

        private static string GetEqualsProperties(ITypeBase instance)
            => string.Join(" &&" + Environment.NewLine + "       ", instance.Properties.Select(p => $"{p.Name} == other.{p.Name}"));

        private sealed class Overload
        {
            public string ArgumentType { get; set; }
            public string InitializeExpression { get; set; }
            public string MethodName { get; set; }
            public string ArgumentName { get; set; }
            public Overload(string argumentType, string initializeExpression, string methodName, string argumentName)
            {
                ArgumentType = argumentType;
                InitializeExpression = initializeExpression;
                MethodName = methodName;
                ArgumentName = argumentName;
            }
        }
    }
}
