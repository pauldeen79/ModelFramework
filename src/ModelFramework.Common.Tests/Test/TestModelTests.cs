namespace ModelFramework.Common.Tests.Test;

public class TestModelTests
{
    [Fact]
    public void Can_Build_Nested_Entity_With_StringBuilder_Property_On_ChildProperty()
    {
        // Arrange
        var sut = new ParentBuilder();
        sut.WithChild(new ChildBuilder()); //necessary because the property gets initialized with Child set to null

        // Act
        sut.AppendToParentProperty("Hello"); //needs to be within 1 and 10 positions ;-)
        sut.Child.AppendToChildProperty("Hello world!");
        var result = sut.Build();

        // Assert
        result.Child.ChildProperty.Should().Be("Hello world!");
    }

    [Fact]
    public void Can_Build_Entity_With_StringBuilder_Property()
    {
        // Arrange
        var sut = new ChildBuilder();

        // Act
        sut.AppendToChildProperty("Hello world!");
        var result = sut.Build();

        // Assert
        result.ChildProperty.Should().Be("Hello world!");
    }
}
