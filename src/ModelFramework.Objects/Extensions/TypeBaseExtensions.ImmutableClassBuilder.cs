﻿namespace ModelFramework.Objects.Extensions;

public static partial class TypeBaseExtensions
{
    public const string NullCheckMetadataValue = "ImmutableClass.Constructor.NullCheck";

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
                            .WithTypeName(p.TypeName.FixCollectionTypeName(settings.NewCollectionTypeName))
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
                        instance.Properties.Select
                        (
                            p => new ParameterBuilder()
                                .WithName(p.Name.ToPascalCase())
                                .WithTypeName(string.Format
                                (
                                    p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName))
                                                       .GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixCollectionTypeName(settings.NewCollectionTypeName)),
                                    p.Name.ToPascalCase().GetCsharpFriendlyName(),
                                    p.TypeName.GetGenericArguments()
                                ))
                                .WithIsNullable(p.IsNullable)
                        )
                    )
                    .AddLiteralCodeStatements
                    (
                        instance.Properties.Where(p => settings.AddNullChecks && p.Metadata.GetValue(NullCheckMetadataValue, () => !p.IsNullable && Type.GetType(p.TypeName.FixTypeName())?.IsValueType != true)).Select
                        (
                            p => @$"if ({p.Name.ToPascalCase()} == null) throw new System.ArgumentNullException(""{p.Name.ToPascalCase()}"");"
                        )
                    )
                    .AddLiteralCodeStatements
                    (
                        instance.Properties.Select
                        (
                            p => string.Format
                            (
                                $"this.{p.Name.GetCsharpFriendlyName()} = {p.Metadata.Concat(p.GetImmutableCollectionMetadata(settings.NewCollectionTypeName)).GetStringValue(MetadataNames.CustomImmutableDefaultValue, p.Name.ToPascalCase().GetCsharpFriendlyName())};",
                                p.Name.ToPascalCase().GetCsharpFriendlyName(),
                                p.TypeName.GetGenericArguments()
                            )
                        )
                    )
                    .AddLiteralCodeStatements
                    (
                        settings.ValidateArgumentsInConstructor
                            ? new[]
                            {
                                "System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);"
                            }
                            : Enumerable.Empty<string>()
                    )
            )
            .AddMethods
            (
                GetImmutableClassMethods(instance,
                                         settings.NewCollectionTypeName,
                                         settings.CreateWithMethod,
                                         settings.ImplementIEquatable,
                                         false)
            )
            .AddInterfaces
            (
                settings.ImplementIEquatable
                    ? new[] { $"IEquatable<{instance.Name}>" }
                    : Enumerable.Empty<string>()
            )
            .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)));
    }

    public static IClass ToImmutableExtensionClass(this ITypeBase instance, ImmutableClassExtensionsSettings settings)
        => instance.ToImmutableExtensionClassBuilder(settings).Build();

    public static ClassBuilder ToImmutableExtensionClassBuilder(this ITypeBase instance, ImmutableClassExtensionsSettings settings)
    {
        if (!instance.Properties.Any())
        {
            throw new InvalidOperationException("To create an immutable extensions class, there must be at least one property");
        }

        return new ClassBuilder()
            .WithName(instance.Name + "Extensions")
            .WithNamespace(instance.Namespace)
            .AddMethods(GetImmutableClassMethods
            (
                instance,
                settings.NewCollectionTypeName,
                createWithMethod: true,
                implementIEquatable: false,
                extensionMethod: true
            ))
            .WithStatic();
    }

    private static IEnumerable<ClassMethodBuilder> GetImmutableClassMethods(ITypeBase instance,
                                                                            string newCollectionTypeName,
                                                                            bool createWithMethod,
                                                                            bool implementIEquatable,
                                                                            bool extensionMethod)
    {
        if (createWithMethod)
        {
            yield return
                new ClassMethodBuilder()
                    .WithName("With")
                    .WithTypeName(instance.Name)
                    .WithStatic(extensionMethod)
                    .WithExtensionMethod(extensionMethod)
                    .AddLiteralCodeStatements($"return new {instance.Name}",
                                               "(")
                    .AddLiteralCodeStatements
                    (
                        instance
                            .Properties
                            .Select
                            (p => new
                            {
                                p.Name,
                                TypeName = p.TypeName.FixCollectionTypeName(newCollectionTypeName),
                                OriginalMetadata = p.Metadata,
                                Metadata = p.Metadata.Concat(p.GetImmutableCollectionMetadata(newCollectionTypeName)),
                                Suffix = p.Name != instance.Properties.Last().Name
                                    ? ","
                                    : string.Empty
                            }
                            )
                            .Select(p => $"    {p.Name.ToPascalCase()} == default({string.Format(p.Metadata.GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName), p.TypeName).GetCsharpFriendlyTypeName()}) ? {GetInstanceName(extensionMethod)}.{p.Name} : {string.Format(p.OriginalMetadata.GetStringValue(MetadataNames.CustomImmutableBuilderDefaultValue, p.Name.ToPascalCase()), p.Name.ToPascalCase())}{p.Suffix}")
                    )
                    .AddLiteralCodeStatements(");")
                    .AddParameters
                    (
                        extensionMethod
                            ? new[] { new ParameterBuilder().WithName("instance").WithTypeName(instance.Name) }
                            : Enumerable.Empty<ParameterBuilder>()
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
                                            p.TypeName.FixCollectionTypeName(newCollectionTypeName)
                                        ), p.TypeName.FixCollectionTypeName(newCollectionTypeName)
                                    ).GetCsharpFriendlyTypeName())
                                    .WithDefaultValue(new Literal($"default({p.Metadata.GetStringValue(MetadataNames.CustomImmutableArgumentType, p.TypeName.FixCollectionTypeName(newCollectionTypeName)).GetCsharpFriendlyTypeName()})"))
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
                .AddLiteralCodeStatements
                (
                    $"return Equals(obj as {instance.Name});"
                );
            yield return new ClassMethodBuilder()
                .WithName($"IEquatable<{instance.Name}>.Equals")
                .WithType(typeof(bool))
                .AddParameters
                (
                    new ParameterBuilder().WithName("other").WithTypeName(instance.Name)
                )
                .AddLiteralCodeStatements
                (
                    $"return other != null &&{Environment.NewLine}       {GetEqualsProperties(instance)};"
                );
            yield return new ClassMethodBuilder()
                .WithName("GetHashCode")
                .WithType(typeof(int))
                .WithOverride()
                .AddLiteralCodeStatements("int hashCode = 235838129;")
                .AddLiteralCodeStatements
                (
                    instance.Properties.Select(p => Type.GetType(p.TypeName.FixTypeName())?.IsValueType == true
                        ? $"hashCode = hashCode * -1521134295 + {p.Name}.GetHashCode();"
                        : $"hashCode = hashCode * -1521134295 + EqualityComparer<{p.TypeName.FixTypeName()}>.Default.GetHashCode({p.Name});")
                )
                .AddLiteralCodeStatements("return hashCode;");
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
                .AddLiteralCodeStatements($"return EqualityComparer<{instance.Name}>.Default.Equals(left, right);");
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
                .AddLiteralCodeStatements("return !(left == right);");
        }
    }

    private static string GetInstanceName(bool extensionMethod)
        => extensionMethod
            ? "instance"
            : "this";

    private static string GetEqualsProperties(ITypeBase instance)
        => string.Join(" &&" + Environment.NewLine + "       ",
                       instance.Properties.Select(p => $"{p.Name} == other.{p.Name}"));
}
