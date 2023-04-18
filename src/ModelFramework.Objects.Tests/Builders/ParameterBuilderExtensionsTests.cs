namespace ModelFramework.Objects.Tests.Builders;

public class ParameterBuilderExtensionsTests
{
    [Fact]
    public void ToString_Gives_Right_Result_With_Ref_Parameter()
    {
        // Arrange
        var parameter = new ParameterBuilder().WithName("Test").WithType(typeof(int)).WithIsRef();

        // Act
        var result = parameter.ToString();

        // Assert
        result.Should().Be("ref System.Int32 Test");
    }

    [Fact]
    public void ToString_Gives_Right_Result_With_Out_Parameter()
    {
        // Arrange
        var parameter = new ParameterBuilder().WithName("Test").WithType(typeof(int)).WithIsOut();

        // Act
        var result = parameter.ToString();

        // Assert
        result.Should().Be("out System.Int32 Test");
    }
}
