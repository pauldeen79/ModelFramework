﻿using System.Linq;
using ModelFramework.Common.Builders;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements.Builders;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    public static partial class TypeBaseExtensions
    {
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
    }
}
