namespace ClassFramework.Pipelines.Tests.Extensions;

public class ClassPropertyExtensionsTests
{
    public class GetDefaultValue
    {
        [Fact]
        public void Gets_Value_From_TypeName_When_Metadata_Is_Not_Found()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();

            // Act
            var result = sut.GetDefaultValue(false);

            // Assert
            result.Should().Be("default(System.String)");
        }

        [Fact]
        public void Gets_Value_From_TypeName_When_Metadata_Is_Found_But_Value_Is_Null()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().AddMetadata(MetadataNames.CustomBuilderDefaultValue, null).Build();

            // Act
            var result = sut.GetDefaultValue(false);

            // Assert
            result.Should().Be("default(System.String)");
        }

        [Fact]
        public void Gets_Value_From_MetadataValue_Literal_When_Found()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().AddMetadata(MetadataNames.CustomBuilderDefaultValue, new Literal("custom value")).Build();

            // Act
            var result = sut.GetDefaultValue(false);

            // Assert
            result.Should().Be("custom value");
        }

        [Fact]
        public void Gets_Value_From_MetadataValue_Non_Literal_When_Found()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().AddMetadata(MetadataNames.CustomBuilderDefaultValue, "custom value").Build();

            // Act
            var result = sut.GetDefaultValue(false);

            // Assert
            result.Should().Be("@\"custom value\"");
        }
    }
}
