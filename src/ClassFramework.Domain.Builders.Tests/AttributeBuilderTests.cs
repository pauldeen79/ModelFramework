namespace ClassFramework.Domain.Builders.Tests;

public class AttributeBuilderTests
{
    [Fact]
    public void AddNameAndParameter_Sets_Name_And_Parameter_Correctly()
    {
        // Arrange
        var sut = new AttributeBuilder();

        // Act
        var actual = sut.AddNameAndParameter("System.ComponentModel.ReadOnly", true);

        // Assert
        actual.Name.ToString().Should().Be("System.ComponentModel.ReadOnly");
        actual.Parameters.Should().ContainSingle();
        actual.Parameters[0].Value.Should().Be(true);
    }

    [Fact]
    public void ForCodeGenerator_Adds_Parameters_Correctly()
    {
        // Arrange
        var sut = new AttributeBuilder();

        // Act
        var actual = sut.ForCodeGenerator("MyGenerator", "1.0.0.0");

        // Assert
        actual.Name.Should().Be(typeof(GeneratedCodeAttribute).FullName);
        actual.Parameters.Should().BeEquivalentTo(new[]
        {
            new AttributeParameterBuilder().WithValue("MyGenerator"),
            new AttributeParameterBuilder().WithValue("1.0.0.0")
        });
    }
}
