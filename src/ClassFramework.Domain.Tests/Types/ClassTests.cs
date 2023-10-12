namespace ClassFramework.Domain.Tests.Types;

public class ClassTests
{
    public class HasPublicParameterlessConstructor
    {
        [Fact]
        public void Returns_True_When_No_Constructors_Are_Defined()
        {
            // Arrange
            var sut = new Class(Enumerable.Empty<ClassField>(), default, default, default, Enumerable.Empty<Class>(), Enumerable.Empty<ClassConstructor>(), Enumerable.Empty<Enumeration>(), default, default, string.Empty, default, Enumerable.Empty<string>(), Enumerable.Empty<ClassProperty>(), Enumerable.Empty<ClassMethod>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<Metadata>(), default, "MyClass", Enumerable.Empty<Attribute>());

            // Act
            var result = sut.HasPublicParameterlessConstructor();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_True_When_A_Public_Constructor_Is_Defined_Without_Any_Parameters()
        {
            // Arrange
            var sut = new Class(Enumerable.Empty<ClassField>(), default, default, default, Enumerable.Empty<Class>(), new[] { new ClassConstructor(default, Enumerable.Empty<Metadata>(), default, default, default, default, default, Domains.Visibility.Public, Enumerable.Empty<Attribute>(), Enumerable.Empty<CodeStatementBase>(), Enumerable.Empty<Parameter>()) }, Enumerable.Empty<Enumeration>(), default, default, string.Empty, default, Enumerable.Empty<string>(), Enumerable.Empty<ClassProperty>(), Enumerable.Empty<ClassMethod>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<Metadata>(), default, "MyClass", Enumerable.Empty<Attribute>());

            // Act
            var result = sut.HasPublicParameterlessConstructor();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_False_When_A_Private_Constructor_Is_Defined_Without_Any_Parameters()
        {
            // Arrange
            var sut = new Class(Enumerable.Empty<ClassField>(), default, default, default, Enumerable.Empty<Class>(), new[] { new ClassConstructor(default, Enumerable.Empty<Metadata>(), default, default, default, default, default, Domains.Visibility.Private, Enumerable.Empty<Attribute>(), Enumerable.Empty<CodeStatementBase>(), Enumerable.Empty<Parameter>()) }, Enumerable.Empty<Enumeration>(), default, default, string.Empty, default, Enumerable.Empty<string>(), Enumerable.Empty<ClassProperty>(), Enumerable.Empty<ClassMethod>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<Metadata>(), default, "MyClass", Enumerable.Empty<Attribute>());

            // Act
            var result = sut.HasPublicParameterlessConstructor();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_When_A_Public_Constructor_Is_Defined_With_Parameter()
        {
            // Arrange
            var sut = new Class(Enumerable.Empty<ClassField>(), default, default, default, Enumerable.Empty<Class>(), new[] { new ClassConstructor(default, Enumerable.Empty<Metadata>(), default, default, default, default, default, Domains.Visibility.Public, Enumerable.Empty<Attribute>(), Enumerable.Empty<CodeStatementBase>(), new[] { new Parameter(default, default, default, "System.String", default, default, Enumerable.Empty<Attribute>(), Enumerable.Empty<Metadata>(), "arg", default) }) }, Enumerable.Empty<Enumeration>(), default, default, string.Empty, default, Enumerable.Empty<string>(), Enumerable.Empty<ClassProperty>(), Enumerable.Empty<ClassMethod>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<string>(), Enumerable.Empty<Metadata>(), default, "MyClass", Enumerable.Empty<Attribute>());

            // Act
            var result = sut.HasPublicParameterlessConstructor();

            // Assert
            result.Should().BeFalse();
        }
    }
}
