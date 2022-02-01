namespace ModelFramework.Database.Tests;

public class StoredProcedureParameterTests
{
    [Fact]
    public void Ctor_Throws_On_Empty_Name()
    {
        // Arrange
        var action = new Action(() => _ = new StoredProcedureParameter("type", "defaultValue", "", Enumerable.Empty<IMetadata>()));

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
    }

    [Fact]
    public void ToString_Returns_Name_When_Type_Is_Empty()
    {
        // Arrange
        var sut = new StoredProcedureParameter(string.Empty, "defaultValue", "Name", Enumerable.Empty<IMetadata>());

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be("@Name");
    }

    [Fact]
    public void ToString_Returns_Name_And_Type_When_Type_Is_Not_Empty()
    {
        // Arrange
        var sut = new StoredProcedureParameter("INT", "defaultValue", "Name", Enumerable.Empty<IMetadata>());

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be("@Name INT");
    }
}
