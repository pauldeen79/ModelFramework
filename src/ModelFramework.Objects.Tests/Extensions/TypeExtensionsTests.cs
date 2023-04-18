namespace ModelFramework.Objects.Tests.Extensions;

public class TypeExtensionsTests
{
    [Fact]
    public void WithoutGenerics_Returns_GenericArgumentName_When_FullName_Is_Null()
    {
        // Arrange
        var sut = typeof(Nullable<>).GetGenericArguments()[0];

        // Act
        var actual = sut.WithoutGenerics();

        // Assert
        actual.Should().Be("T");
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
        actual.Properties.Single().TypeName.Should().Be("System.Func<System.Object?,System.Object?>");
        actual.Properties.Single().IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ToInterface_Maps_Property_Type_With_Double_Generic_Argument_Correctly()
    {
        // Act
        var actual = typeof(IMyGenericInterface<>).ToInterface();

        // Assert
        actual.Properties.Should().ContainSingle();
        actual.Properties.Single().TypeName.Should().Be("System.Func<System.Object,T?>");
        actual.Properties.Single().IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ToInterface_Maps_Property_Type_With_Double_Generic_Argument_Correctly_Two()
    {
        // Act
        var actual = typeof(IMyOtherGenericInterface<>).ToInterface();

        // Assert
        actual.Properties.Should().ContainSingle();
        actual.Properties.Single().TypeName.Should().Be("System.Func<System.Object?,T?>");
        actual.Properties.Single().IsNullable.Should().BeFalse();
    }

    [Fact]
    public void ToInterface_Maps_Property_Type_With_Double_Generic_Argument_Correctly_Three()
    {
        // Act
        var actual = typeof(IMyOtherOtherGenericInterface<>).ToInterface();

        // Assert
        actual.Properties.Should().ContainSingle();
        actual.Properties.Single().TypeName.Should().Be("System.Func<System.Object?,T>");
        actual.Properties.Single().IsNullable.Should().BeFalse();
    }

    [Fact]
    public void ToInterface_Maps_Property_Type_With_Double_Generic_Argument_Correctly_Four()
    {
        // Act
        var actual = typeof(IMyOtherOtherOtherGenericInterface<>).ToInterface();

        // Assert
        actual.Properties.Should().ContainSingle();
        actual.Properties.Single().TypeName.Should().Be("System.Func<System.Object?,T>");
        actual.Properties.Single().IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ToClass_Maps_Field_Type_With_Nullable_Generic_Argument_Correctly()
    {
        // Act
        var actual = typeof(MyClass).ToClass();

        // Assert
        actual.Fields.Should().ContainSingle();
        actual.Fields.Single().TypeName.Should().Be("System.Func<System.Object?,System.Object?>");
        actual.Fields.Single().IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ToClass_Maps_Property_Type_With_NonNullable_Generic_Argument_Correctly()
    {
        // Act
        var actual = typeof(IMyNonNullableGenericPropertyInterface).ToClass();

        // Assert
        actual.Properties.Should().ContainSingle();
        actual.Properties.Single().TypeName.Should().Be("System.Func<System.Object,System.Object?>");
        actual.Properties.Single().IsNullable.Should().BeFalse();
    }

    [Fact]
    public void ToClass_Maps_Property_Type_Without_Generic_Arguments_Correctly()
    {
        // Act
        var actual = typeof(MyClass).ToClass();

        // Assert
        actual.Properties.Should().ContainSingle();
        actual.Properties.Single().TypeName.Should().Be("System.String");
        actual.Properties.Single().IsNullable.Should().BeFalse();
    }

    [Fact]
    public void ToClass_Maps_Property_Type_With_Double_Generic_Arguments_Correctly()
    {
        // Act
        var actual = typeof(MyDoubleGenericClass).ToClass();

        // Assert
        actual.Properties.Should().ContainSingle();
        actual.Properties.Single().TypeName.Should().Be("System.Func<System.String,System.Collections.Generic.KeyValuePair<System.String,System.Object?>>");
        actual.Properties.Single().IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ToInterface_Maps_Method_Return_Type_With_Nullable_Generic_Argument_Correctly()
    {
        // Act
        var actual = typeof(IMyInterface).ToInterface();

        // Assert
        actual.Methods.Should().ContainSingle();
        actual.Methods.Single().TypeName.Should().Be("System.Func<System.Object?,System.Object?>");
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
        actual.Methods.Single().Parameters.Single().TypeName.Should().Be("System.Func<System.Object?,System.Object?>");
        actual.Methods.Single().Parameters.Single().IsNullable.Should().BeTrue();
    }

    [Fact]
    public void ToInterface_Maps_Interface_Type_With_Nullable_Generic_Argument_Correctly()
    {
        // Act
        var actual = typeof(IMyInheritedInterface).ToInterface();

        // Assert
        actual.Interfaces.Should().BeEquivalentTo(
        "ModelFramework.Objects.Tests.Extensions.IMyGenericInterface<System.Object?>",
        "ModelFramework.Objects.Tests.Extensions.IMyGenericInterface<System.Collections.Generic.KeyValuePair<System.Int32,System.Object?>>",
        "ModelFramework.Objects.Tests.Extensions.IMyGenericInterface<System.Nullable<System.Collections.Generic.KeyValuePair<System.Int32,System.Object?>>?>"
        );
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
                    result.AddParameters(new AttributeParameterBuilder().WithName(nameof(sla.MinimumLength)).WithValue(sla.MinimumLength));
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

[ExcludeFromCodeCoverage]
public class MyDoubleGenericClass
{
    public Func<string, KeyValuePair<string, object?>>? Property { get; set; }
}

public interface IMyNonNullableGenericPropertyInterface
{
    Func<object, object?> Property { get; }
}

public interface IMyGenericInterface<T>
{
    Func<object, T?>? Property { get; }
}

public interface IMyOtherGenericInterface<T>
{
    Func<object?, T?> Property { get; }
}

public interface IMyOtherOtherGenericInterface<T>
{
    Func<object?, T> Property { get; }
}

public interface IMyOtherOtherOtherGenericInterface<T>
{
    Func<object?, T>? Property { get; }
}

public interface IMyInheritedInterface
    : IMyGenericInterface<object?>,
      IMyGenericInterface<KeyValuePair<int, object?>>,
      IMyGenericInterface<KeyValuePair<int, object>?>
{
}
