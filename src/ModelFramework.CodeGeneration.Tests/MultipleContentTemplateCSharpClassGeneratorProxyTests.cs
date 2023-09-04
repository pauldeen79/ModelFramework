namespace ModelFramework.CodeGeneration.Tests;

public class MultipleContentTemplateCSharpClassGeneratorProxyTests
{
    [Fact]
    public void Constructor_Sets_Instance_When_Provided()
    {
        // Arrange
        var sut = new MultipleContentTemplateCSharpClassGeneratorProxy();

        // Act
        sut.Instance.Should().BeOfType<CSharpClassGenerator>();
    }
}
