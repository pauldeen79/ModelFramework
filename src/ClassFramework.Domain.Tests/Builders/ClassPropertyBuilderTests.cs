namespace ClassFramework.Domain.Builders.Tests;

public class ClassPropertyBuilderTests
{
    public class Constructor
    {
        [Fact]
        public void Sets_HasGetter_To_True()
        {
            // Act
            var sut = new PropertyBuilder();

            // Assert
            sut.HasGetter.Should().BeTrue();
        }

        [Fact]
        public void Sets_HasSetter_To_True()
        {
            // Act
            var sut = new PropertyBuilder();

            // Assert
            sut.HasSetter.Should().BeTrue();
        }
    }
}
