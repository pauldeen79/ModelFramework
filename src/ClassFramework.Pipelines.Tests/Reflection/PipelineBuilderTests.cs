namespace ClassFramework.Pipelines.Tests.Reflection;

public class PipelineBuilderTests : IntegrationTestBase<IPipelineBuilder<TypeBaseBuilder, ReflectionContext>>
{
    public class IntegrationTests : PipelineBuilderTests
    {
        [Fact]
        public void Creates_Class_With_NamespaceMapping()
        {
            // Arrange
            var model = new ClassBuilder();
            var sourceModel = typeof(MyClass);
            var namespaceMappings = CreateNamespaceMappings("ClassFramework.Pipelines.Tests.Reflection");
            var settings = CreateReflectionSettings(namespaceMappings: namespaceMappings, copyAttributes: true, copyInterfaces: true);
            var context = new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture);

            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(model, context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            result.Value.Should().NotBeNull();

            result.Value!.Attributes.Should().ContainSingle();
            result.Value.Attributes.Single().Name.Should().Be("System.ComponentModel.DisplayNameAttribute");

            result.Value.Interfaces.Should().BeEquivalentTo("MyNamespace.IMyInterface");

            result.Value.Name.Should().Be(nameof(MyClass));
            result.Value.Namespace.Should().Be("ClassFramework.Pipelines.Tests.Reflection");
        }

        [Fact]
        public void Creates_Interface_With_NamespaceMapping()
        {
            // Arrange
            var model = new InterfaceBuilder();
            var sourceModel = typeof(IMyInterface);
            var namespaceMappings = CreateNamespaceMappings("ClassFramework.Pipelines.Tests.Reflection");
            var settings = CreateReflectionSettings(namespaceMappings: namespaceMappings, copyAttributes: true);
            var context = new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture);

            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(model, context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            result.Value.Should().NotBeNull();

            result.Value!.Attributes.Should().BeEmpty();
            result.Value.Interfaces.Should().BeEmpty();

            result.Value.Name.Should().Be(nameof(IMyInterface));
            result.Value.Namespace.Should().Be("ClassFramework.Pipelines.Tests.Reflection");
        }
    }
}

[DisplayName("Test")]
public class MyClass : IMyInterface
{
    public string? MyProperty { get; set; }
    public void DoSomething(int myParameter)
    {
        // Method intentionally left empty.
    }
}

public interface IMyInterface
{
    string? MyProperty { get; set; }
    void DoSomething(int myParameter);
}
