namespace ClassFramework.Domain.Tests.Extensions;

public class MetadataContainerBuilderExtensionsTests
{
    public class AddMetadata
    {
        [Fact]
        public void Throws_On_Null_Name()
        {
            // Arrange
            var sut = new ClassBuilder();

            // Act & Assert
            sut.Invoking(x => x.AddMetadata(name: null!, value: null))
               .Should().Throw<ArgumentNullException>().WithParameterName("name");
        }

        [Fact]
        public void Adds_Metadata_Correctly()
        {
            // Arrange
            var sut = new ClassBuilder();

            // Act
            var result = sut.AddMetadata(name: "Name", value: "Value");

            // Assert
            result.Metadata.Should().BeEquivalentTo(new[] { new Metadata(name: "Name", value: "Value") });
        }
    }
}
