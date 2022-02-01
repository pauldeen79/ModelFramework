namespace ModelFramework.Database.Tests;

public class SchemaTests
{
    [Fact]
    public void Ctor_Throws_On_Empty_Name()
    {
        // Arrange
        var action = new Action(() => _ = new Schema(Enumerable.Empty<ITable>(),
                                                     Enumerable.Empty<IStoredProcedure>(),
                                                     Enumerable.Empty<IView>(),
                                                     string.Empty,
                                                     Enumerable.Empty<IMetadata>()));

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
    }

    [Fact]
    public void ToString_Returns_Name()
    {
        // Arrange
        var sut = new Schema(Enumerable.Empty<ITable>(),
                             Enumerable.Empty<IStoredProcedure>(),
                             Enumerable.Empty<IView>(),
                             "Name",
                             Enumerable.Empty<IMetadata>());

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(sut.Name);
    }
}
