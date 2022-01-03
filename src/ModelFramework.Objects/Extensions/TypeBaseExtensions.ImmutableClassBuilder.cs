using System;
using System.Collections.Generic;
using System.Linq;
using ModelFramework.Common;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Settings;

namespace ModelFramework.Objects.Extensions
{
    public static partial class TypeBaseExtensions
    {
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
    }
}
