﻿namespace ClassFramework.Pipelines.Tests.Reflection;

public class PipelineBuilderTests : IntegrationTestBase<IPipelineBuilder<ClassBuilder, ReflectionContext>>
{
    public class IntegrationTests : PipelineBuilderTests
    {
        private ClassBuilder Model { get; } = new();

        [Fact]
        public void Creates_Class_With_NamespaceMapping()
        {
            // Arrange
            var model = typeof(MyClass);
            var namespaceMappings = CreateNamespaceMappings("ClassFramework.Pipelines.Tests.Reflection");
            var settings = CreateReflectionSettings(namespaceMappings: namespaceMappings, copyAttributes: true);
            var context = new ReflectionContext(model, settings, CultureInfo.InvariantCulture);

            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            result.Value.Should().NotBeNull();

            result.Value!.Attributes.Should().ContainSingle();
            result.Value.Attributes.Single().Name.Should().Be("System.ComponentModel.DisplayNameAttribute");
        }
    }
}

[DisplayName("Test")]
public class MyClass
{
    public string? MyProperty { get; set; }
    public void DoSomething(int myParameter)
    {
        // Method intentionally left empty.
    }
}
