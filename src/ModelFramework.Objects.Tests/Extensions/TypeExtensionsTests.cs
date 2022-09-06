using System.Diagnostics.CodeAnalysis;

namespace ModelFramework.Objects.Tests.Extensions;

public class TypeExtensionsTests
{
    [Fact]
    public void WithoutGenerics_Throws_When_Type_Does_Not_Have_FullName()
    {
        // Arrange
        var sut = typeof(Nullable<>).GetGenericArguments()[0];
        var action = new Action(() => _ = sut.WithoutGenerics());

        // Act
        action.Should().Throw<ArgumentException>().WithMessage("Can't get typename without generics when the FullName of this type is null. Could not determine typename.");
    }

    [Fact]
    public void WithoutGenerics_Returns_Same_TypeName_When_Type_Is_Not_GenericType()
    {
        // Arrange
        var sut = typeof(string);

        // Act
        var actual = sut.WithoutGenerics();

        // Assert
        actual.Should().Be(typeof(string).FullName);
    }

    [Fact]
    public void WithoutGenerics_Strips_Generics_When_Type_Is_GenericType()
    {
        // Arrange
        var sut = typeof(int?);

        // Act
        var actual = sut.WithoutGenerics();

        // Assert
        actual.Should().Be("System.Nullable");
    }

    [Fact]
    public void ToInterface_Maps_Property_Type_With_Nullable_Generic_Argument_Correctly()
    {
        // Act
        var actual = typeof(IMyInterface).ToInterface();

        // Assert
        actual.Properties.Should().ContainSingle();
        actual.Properties.Single().TypeName.FixTypeName().Should().Be("System.Func<System.Object?,System.Object?>");
        actual.Properties.Single().IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ToClass_Maps_Field_Type_With_Nullable_Generic_Argument_Correctly()
    {
        // Act
        var actual = typeof(MyClass).ToClass();

        // Assert
        actual.Fields.Should().ContainSingle();
        actual.Fields.Single().TypeName.FixTypeName().Should().Be("System.Func<System.Object?,System.Object?>");
        actual.Fields.Single().IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ToClass_Maps_Property_Type_Without_Generic_Arguments_Correctly()
    {
        // Act
        var actual = typeof(MyClass).ToClass();

        // Assert
        actual.Properties.Should().ContainSingle();
        actual.Properties.Single().TypeName.FixTypeName().Should().Be("System.String");
        actual.Properties.Single().IsNullable.Should().BeFalse();
    }

    [Fact]
    public void ToInterface_Maps_Method_Return_Type_With_Nullable_Generic_Argument_Correctly()
    {
        // Act
        var actual = typeof(IMyInterface).ToInterface();

        // Assert
        actual.Methods.Should().ContainSingle();
        actual.Methods.Single().TypeName.FixTypeName().Should().Be("System.Func<System.Object?,System.Object?>");
        actual.Methods.Single().IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ToInterface_Maps_Method_Parameter_Type_With_Nullable_Generic_Argument_Correctly()
    {
        // Act
        var actual = typeof(IMyInterface).ToInterface();

        // Assert
        actual.Methods.Should().ContainSingle();
        actual.Methods.Single().Parameters.Should().ContainSingle();
        actual.Methods.Single().Parameters.Single().TypeName.FixTypeName().Should().Be("System.Func<System.Object?,System.Object?>");
        actual.Methods.Single().Parameters.Single().IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ToClass_Can_Map_CustomAttributes_Correctly()
    {
        // Arrange
        var settings = new ClassSettings(attributeInitializeDelegate: new Func<System.Attribute, AttributeBuilder>(a =>
        {
            var result = new AttributeBuilder().WithName(a.GetType().FullName!);
            if (a is StringLengthAttribute sla)
            {
                result.AddParameters(new AttributeParameterBuilder().WithValue(sla.MaximumLength));
                if (sla.MinimumLength > 0)
                {
                    result.AddParameters(new AttributeParameterBuilder().WithName(nameof(sla.MinimumLength)).WithValue(sla.MaximumLength));
                }
            }

            return result;
        }));

        // Act
        var actual = typeof(MyClass).ToClass(settings);

        // Arrange
        actual.Properties.Should().ContainSingle();
        actual.Properties.Single().Attributes.Should().ContainSingle();
        actual.Properties.Single().Attributes.Single().Parameters.Should().HaveCount(2);
    }
}

public interface IMyInterface
{
    Func<object?, object?>? Property { get; }
    Func<object?, object?>? Method(Func<object?, object?>? arg);
}

[ExcludeFromCodeCoverage]
public class MyClass
{
    [StringLength(10, MinimumLength = 1)]
    public string MyProperty { get; } = "";
#pragma warning disable CA1051 // Do not declare visible instance fields
    public Func<object?, object?>? Field;
#pragma warning restore CA1051 // Do not declare visible instance fields
}
