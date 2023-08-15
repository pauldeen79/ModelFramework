namespace ModelFramework.CodeGeneration.Tests;

public class SingleContentTemplateProxyTests
{
    [Fact]
    public void Constructor_Throws_On_Null_Instance()
    {
        // Act & Assert
        this.Invoking(_ => new SingleContentTemplateProxy(instance: null!))
            .Should().Throw<ArgumentNullException>().WithParameterName("instance");
    }

    [Fact]
    public void Constructor_Sets_Instance_When_Provided()
    {
        // Arrange
        var sut = new SingleContentTemplateProxy(this);

        // Act
        sut.Instance.Should().BeSameAs(this);
    }

    [Fact]
    public void Model_Property_Is_Persisted()
    {
        // Arrange
        var sut = new SingleContentTemplateProxy(new CSharpClassGenerator());
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
        var sut = new SingleContentTemplateProxy(this);

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
        var sut = new SingleContentTemplateProxy(new CSharpClassGenerator());

        // Act
        sut.Invoking(x => x.Render(builder: null!))
           .Should().Throw<ArgumentNullException>().WithParameterName("builder");
    }

    [Theory, InlineData(false), InlineData(true)]
    public void Render_Appends_Content_To_Builder_When_Provided(bool generateMultipleFiles)
    {
        // Arrange
        var sut = new SingleContentTemplateProxy(new CSharpClassGenerator());
        sut.Model = new[] { new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build() };
        sut.SetParameter(nameof(CSharpClassGenerator.CreateCodeGenerationHeader), false);
        sut.SetParameter(nameof(CSharpClassGenerator.GenerateMultipleFiles), generateMultipleFiles);
        var builder = new StringBuilder();

        // Act
        sut.Render(builder);

        // Assert
        builder.ToString().Should().NotBeEmpty();
    }

    public string? Myproperty { get; set; }
}
