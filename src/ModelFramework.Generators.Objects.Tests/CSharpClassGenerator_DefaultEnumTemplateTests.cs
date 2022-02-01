namespace ModelFramework.Generators.Objects.Tests;

public class CSharpClassGenerator_DefaultEnumTemplateTests
{
    [Fact]
    public void GeneratesCodeBody()
    {
        // Arrange
        var model = new EnumBuilder()
            .WithName("MyEnum")
            .AddMembers
            (
                new EnumMemberBuilder().WithName("Member1"),
                new EnumMemberBuilder().WithName("Member2"),
                new EnumMemberBuilder().WithName("Member3")
            ).Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultEnumTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be(@"        public enum MyEnum
        {
            Member1,
            Member2,
            Member3
        }
");
    }

    [Fact]
    public void GeneratesAttributes()
    {
        // Arrange
        var model = new EnumBuilder().WithName("MyEnum").AddMembers
        (
            new EnumMemberBuilder().WithName("Member1"),
            new EnumMemberBuilder().WithName("Member2"),
            new EnumMemberBuilder().WithName("Member3")
        ).AddAttributes
        (
            new AttributeBuilder().WithName("Attribute1"),
            new AttributeBuilder().WithName("Attribute2"),
            new AttributeBuilder().WithName("Attribute3"))
        .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultEnumTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be(@"        [Attribute1]
        [Attribute2]
        [Attribute3]
        public enum MyEnum
        {
            Member1,
            Member2,
            Member3
        }
");
    }

    [Fact]
    public void GeneratesInternalEnum()
    {
        // Arrange
        var model = new EnumBuilder()
            .WithName("MyEnum")
            .AddMembers
            (
                new EnumMemberBuilder().WithName("Member1"),
                new EnumMemberBuilder().WithName("Member2"),
                new EnumMemberBuilder().WithName("Member3")
            ).WithVisibility(Visibility.Internal).Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultEnumTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be(@"        internal enum MyEnum
        {
            Member1,
            Member2,
            Member3
        }
");
    }
}
