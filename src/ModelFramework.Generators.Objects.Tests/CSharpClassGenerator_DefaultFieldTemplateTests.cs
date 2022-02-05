namespace ModelFramework.Generators.Objects.Tests;

public class CSharpClassGenerator_DefaultFieldTemplateTests
{
    [Fact]
    public void GeneratesCodeBodyWithoutDefaultValueWhenNotSupplied()
    {
        // Arrange
        var model = new ClassFieldBuilder()
            .WithName("PropertyChanged")
            .WithTypeName("PropertyChangedEventHandler")
            .WithEvent()
            .WithVisibility(Visibility.Public)
            .Build();
        var rootModel = new ClassBuilder().WithName("Test").Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultFieldTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be(@"        public event PropertyChangedEventHandler PropertyChanged;
");
    }

    [Fact]
    public void GeneratesCodeBodyForEvent()
    {
        // Arrange
        var model = new ClassFieldBuilder()
            .WithName("Name")
            .WithType(typeof(string))
            .Build();
        var rootModel = new ClassBuilder().WithName("Test").Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultFieldTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be(@"        private string Name;
");
    }

    [Fact]
    public void GeneratesCodeBodyWithDefaultValueWhenSupplied()
    {
        // Arrange
        var model = new ClassFieldBuilder()
            .WithName("Name")
            .WithType(typeof(string))
            .WithDefaultValue("Hello world")
            .Build();
        var rootModel = new ClassBuilder().WithName("Test").Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultFieldTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be(@"        private string Name = @""Hello world"";
");
    }

    [Fact]
    public void GeneratesAttributes()
    {
        // Arrange
        var model = new ClassFieldBuilder()
            .WithName("Name")
            .WithType(typeof(string))
            .AddAttributes
            (
                new AttributeBuilder().WithName("Attribute1"),
                new AttributeBuilder().WithName("Attribute2"),
                new AttributeBuilder().WithName("Attribute3")
            ).Build();
        var rootModel = new ClassBuilder().WithName("Test").Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultFieldTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be(@"        [Attribute1]
        [Attribute2]
        [Attribute3]
        private string Name;
");
    }

    [Fact]
    public void GeneratesNullableField()
    {
        // Arrange
        var model = new ClassFieldBuilder()
            .WithName("Test")
            .WithType(typeof(string))
            .WithIsNullable()
            .Build();
        var rootModel = new ClassBuilder().WithName("Test").Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultFieldTemplate>(model, rootModel, rootAdditionalParameters: new { EnableNullableContext = true });

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.Should().Be(@"        private string? Test;
");
    }
}
