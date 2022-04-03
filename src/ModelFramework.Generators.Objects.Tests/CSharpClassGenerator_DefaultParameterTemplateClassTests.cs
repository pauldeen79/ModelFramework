namespace ModelFramework.Generators.Objects.Tests;

public class Parameter_DefaultClassTests
{
    [Fact]
    public void GeneratesOutputCorrectly()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ParameterBuilder().WithName("Name").WithTypeName("string").Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultParameterTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be("string Name");
    }

    [Fact]
    public void GeneratesOutputForRefParameterCorrectly()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ParameterBuilder().WithName("Name").WithTypeName("string").WithIsRef().Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultParameterTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be("ref string Name");
    }

    [Fact]
    public void GeneratesOutputForOutParameterCorrectly()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ParameterBuilder().WithName("Name").WithTypeName("string").WithIsOut().Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultParameterTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be("out string Name");
    }

    [Fact]
    public void GeneratesAttributes()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ParameterBuilder()
            .WithName("Name")
            .WithTypeName("string")
            .AddAttributes
            (
                new AttributeBuilder().WithName("Attribute1"),
                new AttributeBuilder().WithName("Attribute2"),
                new AttributeBuilder().WithName("Attribute3")
            ).Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultParameterTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be("[Attribute1][Attribute2][Attribute3]string Name");
    }
}
