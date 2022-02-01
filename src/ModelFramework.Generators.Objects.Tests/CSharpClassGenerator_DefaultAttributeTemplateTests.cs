namespace ModelFramework.Generators.Objects.Tests;

public class CSharpClassGenerator_DefaultAttributeTemplateTests
{
    [Fact]
    public void GeneratesAttributeCode()
    {
        // Arrange
        var model = new AttributeBuilder().WithName("Attribute1").Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultAttributeTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        [Attribute1]
");
    }

    [Fact]
    public void GeneratesCodeForAttributeWithParameterWithNameAndValue()
    {
        // Arrange
        var model = new AttributeBuilder()
            .WithName("Attribute1")
            .AddParameters(new AttributeParameterBuilder().WithValue("Value").WithName("Name"))
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultAttributeTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        [Attribute1(Name = @""Value"")]
");
    }

    [Fact]
    public void GeneratesCodeForAttributeWithParameterWithValueOnly()
    {
        // Arrange
        var model = new AttributeBuilder().WithName("Attribute1")
            .AddParameters(new AttributeParameterBuilder().WithValue("Value"))
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultAttributeTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        [Attribute1(@""Value"")]
");
    }

    //IndentFourSpacesOnClass
    //IndentEightSpacesOnChildItem
}
