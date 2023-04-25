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

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_NonGeneric_Type()
    {
        // Arrange
        var input = typeof(int).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be("System.Int32");
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_Nullable_Type()
    {
        // Arrange
        var input = typeof(int?).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be("System.Nullable<System.Int32>");
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_Generic_Func()
    {
        // Arrange
        var input = typeof(Func<int>).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be("System.Func<System.Int32>");
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_Nullable_Generic_Func()
    {
        // Arrange
        var input = typeof(Func<int?>).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be("System.Func<System.Nullable<System.Int32>>");
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_Generic_Enumerable()
    {
        // Arrange
        var input = typeof(IEnumerable<int>).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be("System.Collections.Generic.IEnumerable<System.Int32>");
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_Nullable_Generic_Enumerable()
    {
        // Arrange
        var input = typeof(IEnumerable<int?>).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        actual.Should().Be("System.Collections.Generic.IEnumerable<System.Nullable<System.Int32>>");
    }

    [Fact]
    public void FixTypeName_Returns_Correct_Result_For_Generics_With_Multiple_Generic_Parameters()
    {
        // Arrange
        var input = typeof(Func<object?, IAsyncDisposable, object?>).FullName;

        // Act
        var actual = input.FixTypeName();

        // Assert
        //Note that nullable generic argument types are not recognized. I'm not sure how to fix this...
        actual.Should().Be("System.Func<System.Object,System.IAsyncDisposable,System.Object>");
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
    [InlineData(null, "")]
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

    [Theory]
    [InlineData("SomeUnknownType", "SomeUnknownType")]
    [InlineData("", "")]
    [InlineData(" ", " ")]
    [InlineData("    ", "    ")]
    [InlineData("System.Char", "char")]
    [InlineData("System.Char with some prefix", "System.Char with some prefix")]
    [InlineData("suffix System.Char", "suffix System.Char")]
    [InlineData("System.String", "string")]
    [InlineData("System.Boolean", "bool")]
    [InlineData("System.Object", "object")]
    [InlineData("System.Decimal", "decimal")]
    [InlineData("System.Double", "double")]
    [InlineData("System.Single", "float")]
    [InlineData("System.Byte", "byte")]
    [InlineData("System.SByte", "sbyte")]
    [InlineData("System.Int16", "short")]
    [InlineData("System.UInt16", "ushort")]
    [InlineData("System.Int32", "int")]
    [InlineData("System.UInt32", "uint")]
    [InlineData("System.Int64", "long")]
    [InlineData("System.UInt64", "ulong")]
    [InlineData("System.String[]", "string[]")]
    [InlineData("IEnumerable<System.String>", "IEnumerable<string>")]
    [InlineData("KeyValuePair<System.String, System.String>", "KeyValuePair<string, string>")]
    [InlineData("KeyValuePair<System.String,System.String>", "KeyValuePair<string,string>")]
    [InlineData("TripleGeneric<System.String, System.String, System.String>", "TripleGeneric<string, string, string>")]
    [InlineData("TripleGeneric<System.String,System.String,System.String>", "TripleGeneric<string,string,string>")]
    public void GetCsharpFriendlyTypeName_Returns_Correct_Result(string input, string expectedResult)
    {
        // Act
        var actual = input.GetCsharpFriendlyTypeName();

        // Assert
        actual.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("System.Object", "")]
    [InlineData("ExpressionFramework.Domain.Contracts.ITypedExpression<System.Object>", "System.Object")]
    [InlineData("ExpressionFramework.Domain.Contracts.ITypedExpression<System.Collections.Generic.IEnumerable<System.Object>>", "System.Collections.Generic.IEnumerable<System.Object>")]
    public void GetGenericArguments_Returns_Correct_Result(string input, string expected)
    {
        // Act
        var result = input.GetGenericArguments();

        // Assert
        result.Should().Be(expected);
    }
}
