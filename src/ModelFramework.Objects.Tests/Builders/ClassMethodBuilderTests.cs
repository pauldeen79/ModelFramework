namespace ModelFramework.Objects.Tests.Builders;

public class ClassMethodBuilderTests
{
    [Fact]
    public void AddNotImplementedException_Adds_CodeStatement_That_Throws_NotImplementedException()
    {
        // Arrange
        var sut = new ClassMethodBuilder().WithName("DoSomething");

        // Act
        sut.AddNotImplementedException();

        // Assert
        sut.CodeStatements.Should().ContainSingle();
        sut.CodeStatements.First().Build().ToString().Should().Be("throw new System.NotImplementedException();");
    }
}
