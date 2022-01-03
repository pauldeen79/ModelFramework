using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Extensions;
using ModelFramework.Objects.Settings;
using Xunit;

namespace ModelFramework.Objects.Tests.Extensions
{
    public partial class TypeBaseExtensionsTests
    {
        [Fact]
        public void Can_Build_ImmutableBuilderClass_From_Immutable_Class()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("TestClass")
                                        .AddProperties(new ClassPropertyBuilder().WithName("Property1")
                                                                                 .WithType(typeof(string))
                                                                                 .AsReadOnly())
                                        .AddProperties(new ClassPropertyBuilder().WithName("Property2")
                                                                                 .WithType(typeof(IReadOnlyCollection<string>))
                                                                                 .AsReadOnly())
                                        .AddConstructors(new ClassConstructorBuilder().AddParameter("property1", typeof(string))
                                                                                      .AddParameter("property2", typeof(IEnumerable<string>))
                                                                                      .AddLiteralCodeStatements("Property1 = property1;")
                                                                                      .AddLiteralCodeStatements("Property2 = new List<string>(property2);"))
                                        .Build();

            // Act
            var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings());

            // Assert
            actual.Name.Should().Be("TestClassBuilder");
            actual.Properties.Should().HaveCount(2);
            actual.Properties.First().HasSetter.Should().BeTrue();
            actual.Properties.Last().HasSetter.Should().BeTrue();
            
            // By default, only a public parameterless constructor should be defined
            actual.Constructors.Should().ContainSingle();
            actual.Constructors.First().Parameters.Should().BeEmpty();
            actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
            string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
Property1 = string.Empty;");
            
            // Build method
            var buildMethod = actual.Methods.SingleOrDefault(x => x.Name == "Build");
            buildMethod.Should().NotBeNull();
            if (buildMethod != null)
            {
                string.Join(Environment.NewLine, buildMethod.CodeStatements.Select(x => x.ToString())).Should().Be("return new TestClass(Property1, Property2);");
            }
        }

        [Fact]
        public void Can_Build_ImmutableBuilderClass_From_Immutable_Class_With_NullChecks()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("TestClass")
                                        .AddProperties(new ClassPropertyBuilder().WithName("Property1")
                                                                                 .WithType(typeof(string))
                                                                                 .AsReadOnly())
                                        .AddProperties(new ClassPropertyBuilder().WithName("Property2")
                                                                                 .WithType(typeof(IReadOnlyCollection<string>))
                                                                                 .AsReadOnly())
                                        .AddConstructors(new ClassConstructorBuilder().AddParameter("property1", typeof(string))
                                                                                      .AddParameter("property2", typeof(IEnumerable<string>))
                                                                                      .AddLiteralCodeStatements("Property1 = property1;")
                                                                                      .AddLiteralCodeStatements("Property2 = new List<string>(property2);"))
                                        .Build();

            // Act
            var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings(addNullChecks: true));

            // Assert
            // By default, only a public parameterless constructor should be defined
            actual.Constructors.Should().ContainSingle();
            actual.Constructors.First().Parameters.Should().BeEmpty();
            actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
            string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
Property1 = string.Empty;");

            // Build method
            var buildMethod = actual.Methods.SingleOrDefault(x => x.Name == "Build");
            buildMethod.Should().NotBeNull();
            if (buildMethod != null)
            {
                string.Join(Environment.NewLine, buildMethod.CodeStatements.Select(x => x.ToString())).Should().Be("return new TestClass(Property1, Property2);");
            }
        }

        [Fact]
        public void Can_Build_ImmutableBuilderClass_From_Poco_Class()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("TestClass")
                                        .AddProperties(new ClassPropertyBuilder().WithName("Property1")
                                                                                 .WithType(typeof(string)))
                                        .AddProperties(new ClassPropertyBuilder().WithName("Property2")
                                                                                 .WithType(typeof(List<string>)))
                                        .Build();

            // Act
            var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings());

            // Assert
            actual.Name.Should().Be("TestClassBuilder");
            actual.Properties.Should().HaveCount(2);
            actual.Properties.First().HasSetter.Should().BeTrue();
            actual.Properties.Last().HasSetter.Should().BeTrue();

            // By default, only a public parameterless constructor should be defined
            actual.Constructors.Should().ContainSingle();
            actual.Constructors.First().Parameters.Should().BeEmpty();
            actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
            string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
Property1 = string.Empty;");

            // Build method
            var buildMethod = actual.Methods.SingleOrDefault(x => x.Name == "Build");
            buildMethod.Should().NotBeNull();
            if (buildMethod != null)
            {
                string.Join(Environment.NewLine, buildMethod.CodeStatements.Select(x => x.ToString())).Should().Be("return new TestClass { Property1 = Property1, Property2 = Property2 };");
            }
        }

        [Fact]
        public void Generating_ImmutableBuilderClass_From_Class_Without_Properties_Throws_Exception()
        {
            // Arrange
            var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

            // Act & Assert
            input.Invoking(x => x.ToImmutableBuilderClass(new ImmutableBuilderClassSettings()))
                 .Should().Throw<InvalidOperationException>()
                 .WithMessage("To create an immutable builder class, there must be at least one property");
        }

        [Fact]
        public void Can_Build_ImmutableBuilderClass_From_Interface()
        {
            // Arrange
            var sut = new InterfaceBuilder().WithName("ITestClass")
                                            .AddProperties(new ClassPropertyBuilder().WithName("Property1")
                                                                                     .WithType(typeof(string))
                                                                                     .AsReadOnly())
                                            .AddProperties(new ClassPropertyBuilder().WithName("Property2")
                                                                                     .WithType(typeof(IReadOnlyCollection<string>))
                                                                                     .AsReadOnly())
                                            .Build();

            // Act
            var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings(formatInstanceTypeNameDelegate: (type, forCreate) =>
            {
                if (type.Name == "ITestClass" && forCreate)
                {
                    return "TestClass";
                }
                return string.Empty;
            }));

            // Assert
            actual.Name.Should().Be("ITestClassBuilder");
            actual.Properties.Should().HaveCount(2);
            actual.Properties.First().HasSetter.Should().BeTrue();
            actual.Properties.Last().HasSetter.Should().BeTrue();
            // By default, only a public parameterless constructor should be defined
            actual.Constructors.Should().ContainSingle();
            actual.Constructors.First().Parameters.Should().BeEmpty();
            actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
            string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
Property1 = string.Empty;");

            // Build method
            var buildMethod = actual.Methods.SingleOrDefault(x => x.Name == "Build");
            buildMethod.Should().NotBeNull();
            if (buildMethod != null)
            {
                string.Join(Environment.NewLine, buildMethod.CodeStatements.Select(x => x.ToString())).Should().Be("return new TestClass(Property1, Property2);");
            }
        }

        [Fact]
        public void Can_Add_CopyConstructor_To_ImmutableBuilderClass()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("TestClass")
                                        .AddProperties(new ClassPropertyBuilder().WithName("Property1")
                                                                                 .WithType(typeof(string))
                                                                                 .AsReadOnly())
                                        .AddProperties(new ClassPropertyBuilder().WithName("Property2")
                                                                                 .WithType(typeof(IReadOnlyCollection<string>))
                                                                                 .AsReadOnly())
                                        .AddConstructors(new ClassConstructorBuilder().AddParameter("property1", typeof(string))
                                                                                      .AddLiteralCodeStatements("Property1 = property1;"))
                                        .Build();

            // Act
            var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addCopyConstructor: true)));

            // Assert
            actual.Name.Should().Be("TestClassBuilder");
            actual.Properties.Should().HaveCount(2);
            actual.Properties.First().HasSetter.Should().BeTrue();
            actual.Properties.Last().HasSetter.Should().BeTrue();
            actual.Constructors.Should().HaveCount(2);

            actual.Constructors.First().Parameters.Should().BeEmpty();
            actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
            string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
Property1 = string.Empty;");

            actual.Constructors.Last().Parameters.Should().HaveCount(1);
            actual.Constructors.Last().Parameters.First().Name.Should().Be("source");
            actual.Constructors.Last().Parameters.First().TypeName.Should().Be("TestClass");
            actual.Constructors.Last().Visibility.Should().Be(Visibility.Public);
            string.Join(Environment.NewLine, actual.Constructors.Last().CodeStatements.Select(x => x.ToString())).Should().Be(@"Property2 = new System.Collections.Generic.List<string>();
Property1 = source.Property1;
Property2.AddRange(source.Property2);");
        }

        [Fact]
        public void Can_Add_Constructor_With_All_Parameters_To_ImmutableBuilderClass()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("TestClass")
                                        .AddProperties(new ClassPropertyBuilder().WithName("Property1")
                                                                                 .WithType(typeof(string))
                                                                                 .AsReadOnly())
                                        .AddProperties(new ClassPropertyBuilder().WithName("Property2")
                                                                                 .WithType(typeof(IReadOnlyCollection<string>))
                                                                                 .AsReadOnly()
                                                                                 .ConvertCollectionOnBuilderToEnumerable())
                                        .AddConstructors(new ClassConstructorBuilder().AddParameter("property1", typeof(string))
                                                                                      .AddParameter("property2", typeof(IEnumerable<string>))
                                                                                      .AddLiteralCodeStatements("Property1 = property1;")
                                                                                      .AddLiteralCodeStatements("Property2 = new List<string>(property2);"))
                                        .Build();

            // Act
            var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings(constructorSettings: new ImmutableBuilderClassConstructorSettings(addConstructorWithAllProperties: true)));

            // Assert
            actual.Name.Should().Be("TestClassBuilder");
            actual.Properties.Should().HaveCount(2);
            actual.Properties.First().HasSetter.Should().BeTrue();
            actual.Properties.Last().HasSetter.Should().BeTrue();
            actual.Constructors.Should().HaveCount(2);

            actual.Constructors.First().Parameters.Should().BeEmpty();
            actual.Constructors.First().Visibility.Should().Be(Visibility.Public);

            actual.Constructors.Last().Parameters.Should().HaveCount(2);
            actual.Constructors.Last().Parameters.First().Name.Should().Be("property1");
            actual.Constructors.Last().Parameters.First().TypeName.Should().Be("System.String");
            actual.Constructors.Last().Parameters.Last().Name.Should().Be("property2");
            actual.Constructors.Last().Parameters.Last().TypeName.Should().Be("System.Collections.Generic.IEnumerable<System.String>");
            actual.Constructors.Last().Visibility.Should().Be(Visibility.Public);
        }

        [Fact]
        public void Can_Add_Overload_To_NonCollection_Property()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("TestClass")
                                        .AddProperties(new ClassPropertyBuilder().WithName("TypeName")
                                                                                 .WithType(typeof(string))
                                                                                 .AddBuilderOverload("WithType", typeof(Type), "type", "{2} = type.AssemblyQualifiedName;"))
                                        .Build();

            // Act
            var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings());

            // Assert
            actual.Name.Should().Be("TestClassBuilder");
            actual.Properties.Should().ContainSingle();
            actual.Properties.First().HasSetter.Should().BeTrue();

            // By default, only a public parameterless constructor should be defined
            actual.Constructors.Should().ContainSingle();
            actual.Constructors.First().Parameters.Should().BeEmpty();
            actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
            string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"TypeName = string.Empty;");

            // Build method
            var buildMethod = actual.Methods.SingleOrDefault(x => x.Name == "Build");
            buildMethod.Should().NotBeNull();
            if (buildMethod != null)
            {
                string.Join(Environment.NewLine, buildMethod.CodeStatements.Select(x => x.ToString())).Should().Be("return new TestClass { TypeName = TypeName };");
            }

            // Default 'with' method
            var withTypeNameMethod = actual.Methods.SingleOrDefault(x => x.Name == "WithTypeName");
            withTypeNameMethod.Should().NotBeNull();
            if (withTypeNameMethod != null)
            {
                string.Join(Environment.NewLine, withTypeNameMethod.CodeStatements.Select(x => x.ToString())).Should().Be(@"TypeName = typeName;
return this;");
            }

            // Added overload method
            var withTypeMethod = actual.Methods.SingleOrDefault(x => x.Name == "WithType");
            withTypeMethod.Should().NotBeNull();
            if (withTypeMethod != null)
            {
                string.Join(Environment.NewLine, withTypeMethod.CodeStatements.Select(x => x.ToString())).Should().Be(@"TypeName = type.AssemblyQualifiedName;
return this;");
            }
        }

        [Fact]
        public void Can_Add_Overload_To_Collection_Property()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("TestClass")
                                        .AddProperties(new ClassPropertyBuilder().WithName("TypeNames")
                                                                                 .WithType(typeof(IEnumerable<string>))
                                                                                 .AddBuilderOverload("AddTypes", typeof(IEnumerable<Type>), "types", "{4}.AddRange(types.Select(x => x.AssemblyQualifiedName));"))
                                        .Build();

            // Act
            var actual = sut.ToImmutableBuilderClass(new ImmutableBuilderClassSettings());

            // Assert
            actual.Name.Should().Be("TestClassBuilder");
            actual.Properties.Should().ContainSingle();
            actual.Properties.First().HasSetter.Should().BeTrue();

            // By default, only a public parameterless constructor should be defined
            actual.Constructors.Should().ContainSingle();
            actual.Constructors.First().Parameters.Should().BeEmpty();
            actual.Constructors.First().Visibility.Should().Be(Visibility.Public);
            string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"TypeNames = new System.Collections.Generic.List<string>();");

            // Build method
            var buildMethod = actual.Methods.SingleOrDefault(x => x.Name == "Build");
            buildMethod.Should().NotBeNull();
            if (buildMethod != null)
            {
                string.Join(Environment.NewLine, buildMethod.CodeStatements.Select(x => x.ToString())).Should().Be("return new TestClass { TypeNames = TypeNames };");
            }

            // Default 'with' method
            var withTypeNameMethod = actual.Methods.SingleOrDefault(x => x.Name == "AddTypeNames" && x.Parameters.First().TypeName == "System.String[]");
            withTypeNameMethod.Should().NotBeNull();
            if (withTypeNameMethod != null)
            {
                string.Join(Environment.NewLine, withTypeNameMethod.CodeStatements.Select(x => x.ToString())).Should().Be(@"TypeNames.AddRange(typeNames);
return this;");
            }

            // Added overload method
            var withTypeMethod = actual.Methods.SingleOrDefault(x => x.Name == "AddTypes" && x.Parameters.First().TypeName == "System.Type[]");
            withTypeMethod.Should().NotBeNull();
            if (withTypeMethod != null)
            {
                string.Join(Environment.NewLine, withTypeMethod.CodeStatements.Select(x => x.ToString())).Should().Be(@"TypeNames.AddRange(types.Select(x => x.AssemblyQualifiedName));
return this;");
            }
        }
    }
}
