namespace ClassFramework.Pipelines.Tests.Extensions;

public class StringExtensionsTests : TestBase
{
    public class MapTypeName : StringExtensionsTests
    {
        private const string TypeName = "MyNamespace.MyClass";

        [Fact]
        public void Throws_On_Null_PipelineBuilderTypeSettings()
        {
            // Act & Assert
            TypeName.Invoking(x => x.MapTypeName(pipelineBuilderTypeSettings: null!))
                    .Should().Throw<ArgumentNullException>().WithParameterName("pipelineBuilderTypeSettings");
        }

        [Fact]
        public void Maps_CollectionItemType_Correctly_Without_New_Collection_TypeName()
        {
            // Arrange
            var collectionTypeName = typeof(IEnumerable<>).ReplaceGenericTypeName(TypeName);
            var settings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            settings.NewCollectionTypeName.Returns(string.Empty);
            settings.NamespaceMappings.Returns(new[] { new NamespaceMapping("MyNamespace", "MappedNamespace", Enumerable.Empty<Metadata>()) }.ToList().AsReadOnly());

            // Act
            var result = collectionTypeName.MapTypeName(settings);

            // Assert
            result.Should().Be("System.Collections.Generic.IEnumerable<MappedNamespace.MyClass>");
        }

        [Fact]
        public void Maps_CollectionItemType_Correctly_With_New_Collection_TypeName()
        {
            // Arrange
            var collectionTypeName = typeof(IEnumerable<>).ReplaceGenericTypeName(TypeName);
            var settings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            settings.NewCollectionTypeName.Returns(typeof(List<>).WithoutGenerics());
            settings.NamespaceMappings.Returns(new[] { new NamespaceMapping("MyNamespace", "MappedNamespace", Enumerable.Empty<Metadata>()) }.ToList().AsReadOnly());

            // Act
            var result = collectionTypeName.MapTypeName(settings);

            // Assert
            result.Should().Be("System.Collections.Generic.List<MappedNamespace.MyClass>");
        }

        [Fact]
        public void Maps_SingleType_Correctly_Using_NamespaceMapping()
        {
            // Arrange
            var settings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            settings.NewCollectionTypeName.Returns(string.Empty);
            settings.NamespaceMappings.Returns(new[] { new NamespaceMapping("MyNamespace", "MappedNamespace", Enumerable.Empty<Metadata>()) }.ToList().AsReadOnly());

            // Act
            var result = TypeName.MapTypeName(settings);

            // Assert
            result.Should().Be("MappedNamespace.MyClass");
        }

        [Fact]
        public void Maps_SingleType_Correctly_Using_TypenameMapping()
        {
            // Arrange
            var settings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            settings.NewCollectionTypeName.Returns(string.Empty);
            settings.TypenameMappings.Returns(new[] { new TypenameMapping("MyNamespace.MyClass", "MappedNamespace.MappedClass", Enumerable.Empty<Metadata>()) }.ToList().AsReadOnly());

            // Act
            var result = TypeName.MapTypeName(settings);

            // Assert
            result.Should().Be("MappedNamespace.MappedClass");
        }

        [Fact]
        public void Returns_Input_Value_When_No_Mappings_Are_Present_Without_New_Collection_TypeName()
        {
            // Arrange
            var settings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            settings.NewCollectionTypeName.Returns(string.Empty);
            settings.NamespaceMappings.Returns(new[] { new NamespaceMapping("WrongNamespace", "MappedNamespace", Enumerable.Empty<Metadata>()) }.ToList().AsReadOnly());
            settings.TypenameMappings.Returns(new[] { new TypenameMapping("WrongNamespace.MyClass", "MappedNamespace.MappedClass", Enumerable.Empty<Metadata>()) }.ToList().AsReadOnly());

            // Act
            var result = TypeName.MapTypeName(settings);

            // Assert
            result.Should().Be(TypeName);
        }

        [Fact]
        public void Returns_Input_Value_When_No_Mappings_Are_Present_With_New_Collection_TypeName()
        {
            // Arrange
            var collectionTypeName = typeof(IEnumerable<>).ReplaceGenericTypeName(TypeName);
            var settings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            settings.NewCollectionTypeName.Returns(typeof(List<>).WithoutGenerics());
            settings.NamespaceMappings.Returns(new[] { new NamespaceMapping("WrongNamespace", "MappedNamespace", Enumerable.Empty<Metadata>()) }.ToList().AsReadOnly());
            settings.TypenameMappings.Returns(new[] { new TypenameMapping("WrongNamespace.MyClass", "MappedNamespace.MappedClass", Enumerable.Empty<Metadata>()) }.ToList().AsReadOnly());

            // Act
            var result = collectionTypeName.MapTypeName(settings);

            // Assert
            result.Should().Be(typeof(List<>).ReplaceGenericTypeName(TypeName));
        }
    }
}
