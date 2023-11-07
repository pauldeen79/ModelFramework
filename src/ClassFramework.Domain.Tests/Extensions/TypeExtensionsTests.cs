namespace ClassFramework.Domain.Tests.Extensions;

public class TypeExtensionsTests
{
    public class WithoutGenerics
    {
        [Fact]
        public void Returns_GenericArgumentName_When_FullName_Is_Null()
        {
            // Arrange
            var sut = typeof(Nullable<>).GetGenericArguments()[0];

            // Act
            var actual = sut.WithoutGenerics();

            // Assert
            actual.Should().Be("T");
        }

        [Fact]
        public void Returns_Same_TypeName_When_Type_Is_Not_GenericType()
        {
            // Arrange
            var sut = typeof(string);

            // Act
            var actual = sut.WithoutGenerics();

            // Assert
            actual.Should().Be(typeof(string).FullName);
        }

        [Fact]
        public void Strips_Generics_When_Type_Is_GenericType()
        {
            // Arrange
            var sut = typeof(int?);

            // Act
            var actual = sut.WithoutGenerics();

            // Assert
            actual.Should().Be("System.Nullable");
        }
    }

    public class GetTypeName
    {
        [Fact]
        public void Returns_Correct_Result_On_IReadOnlyCollection()
        {
            // Arrange
            var prop = GetType().GetProperty(nameof(MyProperty));
            var type = prop!.PropertyType;

            // Act
            var result = type.GetTypeName(prop);

            // Assert
            result.Should().Be("System.Collections.Generic.IReadOnlyCollection<System.String>");
        }

        [Fact]
        public void Returns_Correct_Result_On_Nullable_Int()
        {
            // Arrange
            var prop = GetType().GetProperty(nameof(MyProperty2));
            var type = prop!.PropertyType;

            // Act
            var result = type.GetTypeName(prop);

            // Assert
            result.Should().Be("System.Nullable<System.Int32>");
        }

        [Fact]
        public void Returns_Correct_Result_On_Tuple_With_Nullable_And_NotNullable_ReferenceTypes()
        {
            // Arrange
            var prop = GetType().GetProperty(nameof(MyProperty3));
            var type = prop!.PropertyType;

            // Act
            var result = type.GetTypeName(prop);

            // Assert
            result.Should().Be("System.Tuple<ClassFramework.Domain.Tests.Extensions.TypeExtensionsTests,System.Lazy<ClassFramework.Domain.Tests.Extensions.TypeExtensionsTests>>");
        }

        public IReadOnlyCollection<string> MyProperty { get; } = new ReadOnlyCollection<string>(Array.Empty<string>());
        public int? MyProperty2 { get; }
        public Tuple<TypeExtensionsTests, Lazy<TypeExtensionsTests>> MyProperty3 { get; } = null!;
    }
}
