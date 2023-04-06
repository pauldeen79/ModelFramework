namespace ModelFramework.Generators.Objects.Tests.POC;

public class SharedValidationTests
{
    [Fact]
    public void Can_Validate_Builder_With_Shared_Domain_Logic()
    {
        // Arrange
        var builder = new MySharedValidationDomainEntityBuilder();

        // Act
        var results = builder.Validate(new ValidationContext(builder));

        // Assert
        results.Select(x => x.ErrorMessage).Should().BeEquivalentTo("The Name field is required.", "The field Age must be between 1 and 100.");
    }
}
