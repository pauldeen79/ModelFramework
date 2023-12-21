namespace ClassFramework.Domain.Tests;

public class LiteralTests
{
    [Fact]
    public void Constucts_Correclty_With_OriginalValue()
    {
        // Act
        var sut = new Literal("value", "original value");

        // Assert
        sut.Value.Should().Be("value");
        sut.OriginalValue.Should().Be("original value");
    }

    [Fact]
    public void Constucts_Correclty_Without_OriginalValue()
    {
        // Act
        var sut = new Literal("value", null);

        // Assert
        sut.Value.Should().Be("value");
        sut.OriginalValue.Should().BeNull();
    }
}
