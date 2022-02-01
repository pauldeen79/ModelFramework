namespace ModelFramework.Common.Tests.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("", "")]
    [InlineData("something", "something")]
    [InlineData("Something", "something")]
    [InlineData("a", "a")]
    [InlineData("A", "a")]
    public void ToPascalCase_Returns_Correct_Result(string input, string expectedResult)
    {
        // Act
        var actual = input.ToPascalCase();

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("", "''")]
    [InlineData(" ", "' '")]
    [InlineData("something", "'something'")]
    [InlineData("something 'quoted'", "'something ''quoted'''")]
    [InlineData("something 'quoted' bla", "'something ''quoted'' bla'")]
    public void SqlEncode_Returns_Correct_Result(string input, string expectedResult)
    {
        // Act
        var actual = input.SqlEncode();

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("System.Nullable`1[[System.Boolean, System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]", "System.Nullable<System.Boolean>")]
    [InlineData("MyNamespace.MyClass+MySubClass", "MyNamespace.MyClass.MySubClass")]
    public void FixTypeName_Returns_Correct_Result(string input, string expectedResult)
    {
        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Fact]
    public void FixTypeName_Returns_AnonymousType_When_Type_Is_Anonymous()
    {
        // Arrange
        var typeName = new { Name = "Test" }.GetType().FullName;

        // Act
        var actual = typeName.FixTypeName();

        // Assert
        actual.Should().Be("AnonymousType");
    }

    [Fact]
    public void FixTypeName_Returns_AnonymousType_When_Type_Is_Anonymous_Array()
    {
        // Arrange
        var typeName = new[] { new { Name = "Test" } }.GetType().FullName;

        // Act
        var actual = typeName.FixTypeName();

        // Assert
        actual.Should().Be("AnonymousType[]");
    }

    [Theory]
    [InlineData("ModelFramework.Common.Tests.TestFixtures.MyEnumThing, ModelFramework.Common.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", true)]
    [InlineData("System.Nullable`1[[ModelFramework.Common.Tests.TestFixtures.MyEnumThing, ModelFramework.Common.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", false)]
    [InlineData("", false)]
    [InlineData("System.String", false)]
    [InlineData("System.Int32", false)]
    [InlineData("SomeUnknownType", false)]
    public void IsRequiredEnum_Returns_Correct_Result(string input, bool expectedResult)
    {
        // Act
        var actual = input.IsRequiredEnum();

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("ModelFramework.Common.Tests.TestFixtures.MyEnumThing, ModelFramework.Common.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", false)]
    [InlineData("System.Nullable`1[[ModelFramework.Common.Tests.TestFixtures.MyEnumThing, ModelFramework.Common.Tests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null]], System.Private.CoreLib, Version=5.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e", true)]
    [InlineData("", false)]
    [InlineData("System.String", false)]
    [InlineData("System.Int32", false)]
    [InlineData("SomeUnknownType", false)]
    public void IsOptionalEnum_Returns_Correct_Result(string input, bool expectedResult)
    {
        // Act
        var actual = input.IsOptionalEnum();

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("MyClass", "MyClass")]
    [InlineData("MyNamespace.MyClass", "MyClass")]
    [InlineData("A.B.C.D", "D")]
    public void GetClassName_Returns_Correct_Result(string input, string expectedResult)
    {
        // Act
        var actual = input.GetClassName();

        // Assert
        actual.Should().Be(expectedResult);
    }


    [Theory]
    [InlineData("", "")]
    [InlineData("MyClass", "")]
    [InlineData("MyNamespace.MyClass", "MyNamespace")]
    [InlineData("A.B.C.D", "A.B.C")]
    public void GetNamespaceWithDefault_Returns_Correct_Result(string input, string expectedResult)
    {
        // Act
        var actual = input.GetNamespaceWithDefault();

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("", "MyClass")]
    [InlineData("Generic", "MyClass<Generic>")]
    [InlineData("MyNamespace.Generic", "MyClass<MyNamespace.Generic>")]
    public void MakeGenericTypeName_Returns_Correct_Result(string input, string expectedResult)
    {
        // Act
        var actual = "MyClass".MakeGenericTypeName(input);

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("", "A", "B", "")]
    [InlineData("ASomething", "A", "B", "ASomething")]
    [InlineData("ASomeAthing", "A", "B", "ASomeAthing")]
    [InlineData("SomethingA", "A", "B", "SomethingB")]
    public void ReplaceSuffix_Returns_Correct_Result(string input, string find, string replace, string expectedResult)
    {
        // Act
        var actual = input.ReplaceSuffix(find, replace, StringComparison.InvariantCulture);

        // Assert
        actual.Should().Be(expectedResult);
    }
}
