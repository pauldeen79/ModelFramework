﻿namespace ModelFramework.Objects.Extensions;

public static partial class TypeBaseExtensions
{
    public static IClass ToObservableClass(this ITypeBase instance,
                                           string newCollectionTypeName = "System.Collections.ObjectModel.ObservableCollection")
        => instance.ToObservableClassBuilder(newCollectionTypeName).BuildTyped();

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
                            .ForPocoClassBuilder(p, newCollectionTypeName)
                            .AddGetterCodeStatements(p.FixObservablePropertyGetterBody().Select(x => x.CreateBuilder()))
                            .AddSetterCodeStatements(p.FixObservablePropertySetterBody().Select(x => x.CreateBuilder()))
                            .WithExplicitInterfaceName(p.ExplicitInterfaceName)
                            .AddMetadata(p.Metadata.Concat(p.GetObservableCollectionMetadata(newCollectionTypeName))
                                                   .Select(x => new MetadataBuilder(x)))
                            .AddAttributes(p.Attributes.Select(x => new AttributeBuilder(x)))
                    )
            )
            .AddFields(instance.GetFields().Select(x => new ClassFieldBuilder(x)))
            .AddFields
            (
                instance.Properties
                    .Where(p => !p.GetterCodeStatements.Any() && !p.SetterCodeStatements.Any())
                    .Select
                    (
                        p => new ClassFieldBuilder()
                            .WithName("_" + p.Name.ToPascalCase())
                            .WithTypeName(p.TypeName)
                            .WithIsNullable(p.IsNullable)
                            .WithIsValueType(p.IsValueType)
                    )
            )
            .AddConstructors
            (
                new ClassConstructorBuilder()
                    .AddLiteralCodeStatements
                    (
                        instance.Properties
                            .Where(p => p.TypeName.FixCollectionTypeName(newCollectionTypeName).StartsWith("System.Collections.ObjectModel.ObservableCollection<"))
                            .Select(p => $"this.{p.Name} = new {p.TypeName.FixCollectionTypeName(newCollectionTypeName).GetCsharpFriendlyTypeName()}();")
                    )
            )
            .AddAttributes(instance.Attributes.Select(x => new AttributeBuilder(x)))
            .AddGenericTypeArguments(instance.GenericTypeArguments)
            .AddGenericTypeArgumentConstraints(instance.GenericTypeArgumentConstraints);
}
