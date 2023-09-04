namespace ModelFramework.CodeGeneration.Tests;

public class SingleContentTemplateCSharpClassGeneratorProxyTests
{
    [Fact]
    public void Constructor_Sets_Instance_When_Provided()
    {
        // Arrange
        var sut = new SingleContentTemplateCSharpClassGeneratorProxy();

        // Act
        sut.Instance.Should().BeOfType<CSharpClassGenerator>();
    }
}
