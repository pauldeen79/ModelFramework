namespace ClassFramework.Domain.Tests.Builders;

public class PropertyBuilderTests : TestBase<PropertyBuilder>
{
    public class Constructor : PropertyBuilderTests
    {
        [Fact]
        public void Sets_HasGetter_To_True()
        {
            // Act
            var sut = CreateSut();

            // Assert
            sut.HasGetter.Should().BeTrue();
        }

        [Fact]
        public void Sets_HasSetter_To_True()
        {
            // Act
            var sut = CreateSut();

            // Assert
            sut.HasSetter.Should().BeTrue();
        }
    }
}
