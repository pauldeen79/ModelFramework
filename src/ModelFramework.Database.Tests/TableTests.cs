﻿namespace ModelFramework.Database.Tests;

public class TableTests
{
    [Fact]
    public void Ctor_Throws_On_Empty_Name()
    {
        // Arrange
        var action = new Action(() => _ = new TableBuilder().Build());

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
    }

    [Fact]
    public void ToString_Returns_Name()
    {
        // Arrange
        var sut = new TableBuilder().WithName("test").Build();

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(sut.Name);
    }
}
