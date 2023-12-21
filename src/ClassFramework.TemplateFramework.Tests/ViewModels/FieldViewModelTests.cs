namespace ClassFramework.TemplateFramework.Tests.ViewModels;

public class FieldViewModelTests : TestBase<FieldViewModel>
{
    public class TypeName : FieldViewModelTests
    {
        [Fact]
        public void Throws_When_Model_Is_Null()
        {
            // Arrange
            var sut = CreateSut();
            sut.Model = null!;

            // Act & Assert
            sut.Invoking(x => _ = x.TypeName)
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("Model");
        }

        [Fact]
        public void Gets_Csharp_Friendly_TypeName()
        {
            // Arrange
            var sut = CreateSut();
            sut.Model = new FieldBuilder()
                .WithName("MyMethod")
                .WithType(typeof(int))
                .Build();

            // Act
            var result = sut.TypeName;

            // Assert
            result.Should().Be("int");
        }

        [Fact]
        public void Appends_Nullable_Notation()
        {
            // Arrange
            var sut = CreateSut();
            sut.Model = new FieldBuilder()
                .WithName("MyMethod")
                .WithType(new ClassBuilder().WithName("MyType"))
                .WithIsNullable()
                .Build();

            // Act
            var result = sut.TypeName;

            // Assert
            result.Should().Be("MyType?");
        }

        [Fact]
        public void Abbreviates_Namespaces()
        {
            // Arrange
            var sut = CreateSut();
            sut.Model = new FieldBuilder()
                .WithName("MyMethod")
                .WithType(new ClassBuilder().WithName("MyType").WithNamespace("MyNamespace"))
                .AddMetadata(MetadataNames.NamespaceToAbbreviate, "MyNamespace")
                .Build();

            // Act
            var result = sut.TypeName;

            // Assert
            result.Should().Be("MyType");
        }
    }
}
