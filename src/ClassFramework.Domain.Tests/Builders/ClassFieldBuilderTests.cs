namespace ClassFramework.Domain.Builders.Tests;

public class ClassFieldBuilderTests
{
    public class Constructor
    {
        [Fact]
        public void Sets_Visibilty_To_Private()
        {
            // Act
            var sut = new FieldBuilder();

            // Assert
            sut.Visibility.Should().Be(Domains.Visibility.Private);
        }
    }
}
