﻿namespace ClassFramework.Domain.Tests.Extensions;

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

    public class IsRecord
    {
        [Fact]
        public void Returns_False_On_Class()
        {
            // Arrange
            var sut = typeof(MyClass);

            // Act
            var result = sut.IsRecord();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_On_Struct()
        {
            // Arrange
            var sut = typeof(MyStruct);

            // Act
            var result = sut.IsRecord();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_False_On_Interface()
        {
            // Arrange
            var sut = typeof(IMyInterface);

            // Act
            var result = sut.IsRecord();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void Returns_True_On_Record()
        {
            // Arrange
            var sut = typeof(MyRecord);

            // Act
            var result = sut.IsRecord();

            // Assert
            result.Should().BeTrue();
        }

#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable S2094 // Classes should not be empty
#pragma warning disable CA1812
        private sealed class MyClass { }
        private struct MyStruct { }
        private interface IMyInterface { }
        private sealed record MyRecord { }
#pragma warning restore CA1812
#pragma warning restore S2094 // Classes should not be empty
#pragma warning restore S1144 // Unused private types or members should be removed
    }
}
