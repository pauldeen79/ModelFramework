namespace ModelFramework.Objects.Tests.Builders;

public class ClassFieldBuilderTests
{
    [Fact]
    public void ToString_Gives_Right_Result_With_ParentTypeFullName_Empty()
    {
        // Arrange
        var sut = new ClassFieldBuilder().WithName("Name").WithType(typeof(int));

        // Act
        var result = sut.ToString();

        // Assert
        result.Should().Be("System.Int32 Name");
    }

    [Fact]
    public void ToString_Gives_Right_Result_With_ParentTypeFullName_Filled()
    {
        // Arrange
        var sut = new ClassFieldBuilder().WithName("Name").WithType(typeof(int)).WithParentTypeFullName("MyParent");

        // Act
        var result = sut.ToString();

        // Assert
        result.Should().Be("System.Int32 MyParent.Name");
    }
}
