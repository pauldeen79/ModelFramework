namespace DatabaseFramework.Domain.Tests;

public class TableTests
{
    [Fact]
    public void Cannot_Construct_Without_Any_Fields()
    {
        // Arrange
        var tableBuilder = new TableBuilder().WithName("MyTable");

        // Act & Assert
        tableBuilder.Invoking(x => x.Build()).Should().Throw<ValidationException>().WithMessage("The field Fields is invalid.");
    }
}
