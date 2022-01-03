using System;
using System.Collections.Generic;
using System.Linq;
using CrossCutting.Common;
using CrossCutting.Common.Extensions;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Settings;

namespace ModelFramework.Objects.Extensions
{
    public static partial class TypeBaseEtensions
    {
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
    }
}
