namespace ModelFramework.Objects.Tests;

public class SystemTests
{
    [Fact]
    public void Can_Determine_Nullability_Of_Generic_Argument()
    {
        // Arrange
        var type = typeof(Func<object?>);

        // Act
        var innerNullability = NullableHelper.IsNullable(type.GetGenericArguments()[0], type.GetGenericArguments()[0], type.GetGenericArguments()[0].CustomAttributes, 1);

        // Assert
        innerNullability.Should().BeTrue();
    }
}
