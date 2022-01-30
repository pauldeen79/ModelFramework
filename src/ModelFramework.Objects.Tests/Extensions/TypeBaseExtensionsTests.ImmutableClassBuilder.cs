using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;
using Xunit;

namespace ModelFramework.Objects.Tests.Extensions
{
    public partial class TypeBaseExtensionsTests
    {
        [Fact]
        public void Generating_ImmutableClass_From_Class_Without_Properties_Throws_Exception()
        {
            // Arrange
            var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

            // Act & Assert
            input.Invoking(x => x.ToImmutableClass(new ImmutableClassSettings()))
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("To create an immutable class, there must be at least one property");
        }

        [Fact]
        public void Generating_ImmutableClassBuilder_From_Class_Without_Properties_Throws_Exception()
        {
            // Arrange
            var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

            // Act & Assert
            input.Invoking(x => x.ToImmutableClassBuilder(new ImmutableClassSettings()))
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("To create an immutable class, there must be at least one property");
        }

        [Fact]
        public void Generating_ImmutableClass_With_MethodTemplate_Returns_Class_With_Build_And_With_Methods()
        {
            // Arrange
            var properties = new[]
            {
                new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
                new ClassPropertyBuilder().WithName("Property2").WithType(typeof(ICollection<string>)).ConvertCollectionOnBuilderToEnumerable(true),
                new ClassPropertyBuilder().WithName("Property3").WithTypeName("MyCustomType").ConvertSinglePropertyToBuilderOnBuilder(),
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                new ClassPropertyBuilder().WithName("Property4").WithTypeName(typeof(ICollection<string>).FullName.Replace("System.String","MyCustomType")).ConvertCollectionPropertyToBuilderOnBuilder(true)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            };
            var settings = new ImmutableBuilderClassSettings();
            var cls = new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToImmutableClass(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"));

            // Act
            var actual = cls.ToImmutableBuilderClass(settings);

            // Assert
            actual.Methods.Select(x => x.Name).Should().BeEquivalentTo(new[]
            {
                "Build",
                "WithProperty1",
                "AddProperty2",
                "AddProperty2",
                "WithProperty3",
                "AddProperty4",
                "AddProperty4"
            });
        }

        [Fact]
        public void Generating_ImmutableClass_Without_MethodTemplate_Returns_Class_With_Build_And_With_Methods()
        {
            // Arrange
            var properties = new[]
            {
                new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)),
                new ClassPropertyBuilder().WithName("Property2").WithType(typeof(ICollection<string>)).ConvertCollectionOnBuilderToEnumerable(true),
                new ClassPropertyBuilder().WithName("Property3").WithTypeName("MyCustomType").ConvertSinglePropertyToBuilderOnBuilder(),
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                new ClassPropertyBuilder().WithName("Property4").WithTypeName(typeof(ICollection<string>).FullName.Replace("System.String","MyCustomType")).ConvertCollectionPropertyToBuilderOnBuilder(true)
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            };
            var settings = new ImmutableBuilderClassSettings(setMethodNameFormatString: string.Empty);
            var cls = new ClassBuilder()
                .WithName("MyRecord")
                .WithNamespace("MyNamespace")
                .AddProperties(properties)
                .Build()
                .ToImmutableClass(new ImmutableClassSettings("System.Collections.Generic.IReadOnlyCollection"));

            // Act
            var actual = cls.ToImmutableBuilderClass(settings);

            // Assert
            actual.Methods.Select(x => x.Name).Should().BeEquivalentTo(new[]
            {
                "Build"
            });
        }

        [Fact]
        public void ToImmutableExtensionClass_Throws_When_Properties_Are_Empty()
        {
            // Arrange
            var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

            // Act & Assert
            input.Invoking(x => x.ToImmutableExtensionClass(new ImmutableClassExtensionsSettings()))
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("To create an immutable extensions class, there must be at least one property");
        }

        [Fact]
        public void ToImmutableExtensionClassBuilder_Throws_When_Properties_Are_Empty()
        {
            // Arrange
            var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

            // Act & Assert
            input.Invoking(x => x.ToImmutableExtensionClassBuilder(new ImmutableClassExtensionsSettings()))
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("To create an immutable extensions class, there must be at least one property");
        }

        [Fact]
        public void ToBuilderExtensionsClass_Throws_When_Properties_Are_Empty()
        {
            // Arrange
            var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

            // Act & Assert
            input.Invoking(x => x.ToBuilderExtensionsClass(new ImmutableBuilderClassSettings()))
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("To create a builder extensions class, there must be at least one property");
        }

        [Fact]
        public void ToBuilderExtensionsClassBuilder_Throws_When_Properties_Are_Empty()
        {
            // Arrange
            var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

            // Act & Assert
            input.Invoking(x => x.ToBuilderExtensionsClassBuilder(new ImmutableBuilderClassSettings()))
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("To create a builder extensions class, there must be at least one property");
        }
    }
}
