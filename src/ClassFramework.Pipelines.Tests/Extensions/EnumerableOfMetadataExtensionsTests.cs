namespace ClassFramework.Pipelines.Tests.Extensions;

public class EnumerableOfMetadataExtensionsTests : TestBase
{
    public class WithMappingMetadata : EnumerableOfMetadataExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_TypeName()
        {
            // Arrange
            var metadata = new[] { new MetadataBuilder().WithName("MyName").Build() };
            var pipelineBuilderTypeSettings = Fixture.Freeze<IPipelineBuilderTypeSettings>();

            // Act & Assert
            metadata.Invoking(x => x.WithMappingMetadata(typeName: null!, pipelineBuilderTypeSettings))
                    .Should().Throw<ArgumentNullException>().WithParameterName("typeName");
        }

        [Fact]
        public void Throws_On_Null_PipelineBuilderTypeSettings()
        {
            // Arrange
            var metadata = new[] { new MetadataBuilder().WithName("MyName").Build() };
            var typeName = "MyNamespace.MyClass";

            // Act & Assert
            metadata.Invoking(x => x.WithMappingMetadata(typeName, pipelineBuilderTypeSettings: null!))
                    .Should().Throw<ArgumentNullException>().WithParameterName("pipelineBuilderTypeSettings");
        }

        [Fact]
        public void Returns_InputData_When_No_Mapping_Matches_Input_TypeName()
        {
            // Arrange
            var metadata = new[] { new MetadataBuilder().WithName("MyName").Build() };
            var typeName = "MyNamespace.MyClass";
            var pipelineBuilderTypeSettings = Fixture.Freeze<IPipelineBuilderTypeSettings>();

            // Act
            var result = metadata.WithMappingMetadata(typeName, pipelineBuilderTypeSettings);

            // Assert
            result.Should().BeEquivalentTo(metadata);
        }

        [Fact]
        public void Returns_InputData_With_Mapping_From_Namespace_When_Mapping_Matches_Namespace_From_Input_TypeName()
        {
            // Arrange
            var metadata = new[] { new MetadataBuilder().WithName("MyName").Build() };
            var additionalMetadata = new[] { new MetadataBuilder().WithName("MyName2").Build() };
            var typeName = "MyNamespace.MyClass";
            var pipelineBuilderTypeSettings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            pipelineBuilderTypeSettings.NamespaceMappings.Returns(new[] { new NamespaceMapping("MyNamespace", "IgnoredNamespace", additionalMetadata) }.ToList().AsReadOnly()); // this one gets ignored, as typename gets precedence
            pipelineBuilderTypeSettings.TypenameMappings.Returns(new[] { new TypenameMapping("MyNamespace.MyClass", "MappedNamespace.MappedClass", additionalMetadata) }.ToList().AsReadOnly());

            // Act
            var result = metadata.WithMappingMetadata(typeName, pipelineBuilderTypeSettings);

            // Assert
            result.Should().BeEquivalentTo(metadata.Concat(additionalMetadata));
        }

        [Fact]
        public void Returns_InputData_With_Mapping_From_Namespace_When_Mapping_Matches_TypeName_From_Input_TypeName()
        {
            // Arrange
            var metadata = new[] { new MetadataBuilder().WithName("MyName").Build() };
            var additionalMetadata = new[] { new MetadataBuilder().WithName("MyName2").Build() };
            var typeName = "MyNamespace.MyClass";
            var pipelineBuilderTypeSettings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            pipelineBuilderTypeSettings.TypenameMappings.Returns(new[] { new TypenameMapping("MyNamespace.MyClass", "MappedNamespace.MappedClass", additionalMetadata) }.ToList().AsReadOnly());

            // Act
            var result = metadata.WithMappingMetadata(typeName, pipelineBuilderTypeSettings);

            // Assert
            result.Should().BeEquivalentTo(metadata.Concat(additionalMetadata));
        }
    }
}
