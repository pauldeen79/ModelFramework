﻿namespace ModelFramework.Generators.Objects.Tests;

public class CSharpClassGenerator_DefaultEnumMemberTemplateTests
{
    [Fact]
    public void GeneratesCodeBodyWithoutComma()
    {
        // Arrange
        var model = new EnumMemberBuilder().WithName("Member").Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultEnumMemberTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be("            Member");
    }

    [Fact]
    public void GeneratesCodeBodyWithValueWhenPresent()
    {
        // Arrange
        var model = new EnumMemberBuilder().WithName("Member").WithValue(1).Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultEnumMemberTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be("            Member = 1");
    }
}
