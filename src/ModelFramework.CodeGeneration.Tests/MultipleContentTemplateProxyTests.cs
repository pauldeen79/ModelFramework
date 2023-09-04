namespace ModelFramework.CodeGeneration.Tests;

public class MultipleContentTemplateProxyTests
{
    [Fact]
    public void Constructor_Throws_On_Null_Instance()
    {
        // Act & Assert
        this.Invoking(_ => new MultipleContentTemplateProxy(instance: null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("instance");
    }

    [Fact]
    public void Constructor_Sets_Instance_When_Provided()
    {
        // Arrange
        var sut = new MultipleContentTemplateProxy(this);

        // Act
        sut.Instance.Should().BeSameAs(this);
    }

    [Fact]
    public void Model_Property_Is_Persisted()
    {
        // Arrange
        var sut = new MultipleContentTemplateProxy(new CSharpClassGenerator());
        var modelValue = new object();

        // Act
        sut.Model = modelValue;

        // Assert
        sut.Model.Should().BeSameAs(modelValue);
    }

    [Fact]
    public void GetParameters_Returns_All_Properties_From_Wraped_Instance()
    {
        // Arrange
        var sut = new MultipleContentTemplateProxy(this);

        // Act
        var parameters = sut.GetParameters();

        // Assert
        parameters.Should().ContainSingle();
        parameters.Single().Name.Should().Be(nameof(Myproperty));
        parameters.Single().Type.Should().Be(typeof(string));
    }

    [Fact]
    public void Render_Throws_When_Builder_Is_Null()
    {
        // Arrange
        var sut = new MultipleContentTemplateProxy(new CSharpClassGenerator());

        // Act
        sut.Invoking(x => x.Render(builder: null!))
           .Should().Throw<ArgumentNullException>().WithParameterName("builder");
    }

    [Theory,
        InlineData(false, true),
        InlineData(true, true),
        InlineData(false, false),
        InlineData(true, false)]
    public void Render_Appends_Content_To_Builder_When_Provided(bool generateMultipleFiles, bool filledModel)
    {
        // Arrange
        var sut = new MultipleContentTemplateProxy(new CSharpClassGenerator());
        sut.Model = new[] { new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build() }.Where(_ => filledModel);
        sut.SetParameter(nameof(CSharpClassGenerator.CreateCodeGenerationHeader), false);
        sut.SetParameter(nameof(CSharpClassGenerator.GenerateMultipleFiles), generateMultipleFiles);
        var builder = new MultipleContentBuilder();

        // Act
        sut.Render(builder);

        // Assert
        if (generateMultipleFiles && !filledModel)
        {
            builder.Build().Contents.Should().BeEmpty();
        }
        else
        {
            builder.Build().Contents.Should().NotBeEmpty();
        }
    }

    public string? Myproperty { get; set; }
}
