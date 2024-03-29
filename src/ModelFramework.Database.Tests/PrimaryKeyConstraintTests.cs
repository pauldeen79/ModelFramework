﻿namespace ModelFramework.Database.Tests;

public class PrimaryKeyConstraintTests
{
    [Fact]
    public void Ctor_Throws_On_Empty_Name()
    {
        // Arrange
        var action = new Action(() => _ = new PrimaryKeyConstraint
        (
            new[] { new PrimaryKeyConstraintField(false, "name", Enumerable.Empty<IMetadata>()) },
            string.Empty,
            Enumerable.Empty<IMetadata>(),
            string.Empty
        ));

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("Name cannot be null or whitespace");
    }

    [Fact]
    public void Ctor_Throws_On_Empty_Fields()
    {
        // Arrange
        var action = new Action(() => _ = new PrimaryKeyConstraint
        (
            Enumerable.Empty<IPrimaryKeyConstraintField>(),
            "Name",
            Enumerable.Empty<IMetadata>(),
            string.Empty
        ));

        // Act & Assert
        action.Should().Throw<ValidationException>().WithMessage("Fields should contain at least 1 value");
    }

    [Fact]
    public void ToString_Returns_Name()
    {
        // Arrange
        var sut = new PrimaryKeyConstraint
        (
            new[] { new PrimaryKeyConstraintField(false, "name", Enumerable.Empty<IMetadata>()) },
            "Name",
            Enumerable.Empty<IMetadata>(),
            string.Empty
        );

        // Act
        var actual = sut.ToString();

        // Assert
        actual.Should().Be(sut.Name);
    }
}
