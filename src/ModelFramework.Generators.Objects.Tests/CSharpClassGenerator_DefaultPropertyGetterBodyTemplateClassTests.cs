namespace ModelFramework.Generators.Objects.Tests;

public class CSharpClassGenerator_DefaultPropertyGetterBodyTemplateClassTests
{
    [Fact]
    public void GeneratesNoCodeBodyWhenGetterBodyIsEmpty()
    {
        // Arrange
        var model = new ClassPropertyBuilder().WithName("Name").WithTypeName("string").Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyGetterBodyTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"get;
");
    }

    [Fact]
    public void GeneratesCodeBodyWhenGetterBodyIsFilled()
    {
        // Arrange
        var model = new ClassPropertyBuilder()
            .WithName("Name")
            .WithTypeName("string")
            .AddGetterLiteralCodeStatements("throw new NotImplementedException();")
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyGetterBodyTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"get
            {
                throw new NotImplementedException();
            }
");
    }
}
