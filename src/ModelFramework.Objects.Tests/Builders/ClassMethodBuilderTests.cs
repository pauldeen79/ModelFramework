namespace ModelFramework.Objects.Tests.Builders;

public class ClassMethodBuilderTests
{
    [Fact]
    public void AddNotImplementedException_Adds_CodeStatement_That_Throws_NotImplementedException()
    {
        // Arrange
        var sut = new ClassMethodBuilder().WithName("DoSomething");

        // Act
        sut.AddNotImplementedException();

        // Assert
        sut.CodeStatements.Should().ContainSingle();
        sut.CodeStatements.First().Build().ToString().Should().Be("throw new System.NotImplementedException();");
    }

    [Fact]
    public void ToString_Gives_Right_Result_With_ParentTypeFullName_Empty()
    {
        // Arrange
        var sut = new ClassMethodBuilder().WithName("Name").WithType(typeof(int));

        // Act
        var result = sut.ToString();

        // Assert
        result.Should().Be("System.Int32 Name()");
    }

    [Fact]
    public void ToString_Gives_Right_Result_With_ParentTypeFullName_Filled()
    {
        // Arrange
        var sut = new ClassMethodBuilder().WithName("Name").WithType(typeof(int)).WithParentTypeFullName("MyParent");

        // Act
        var result = sut.ToString();

        // Assert
        result.Should().Be("System.Int32 MyParent.Name()");
    }

    [Fact]
    public void ToString_Gives_Right_Result_With_ParentTypeFullName_And_Parameters_Filled()
    {
        // Arrange
        var sut = new ClassMethodBuilder()
            .WithName("Name")
            .WithType(typeof(int))
            .WithParentTypeFullName("MyParent")
            .AddParameter("arg1", typeof(int))
            .AddParameter("arg2", typeof(KeyValuePair<string, object?>).GetTypeName(GetType()))
            .AddParameter("arg3", typeof(int?));

        // Act
        var result = sut.ToString();

        // Assert
        result.Should().Be("System.Int32 MyParent.Name(System.Int32 arg1, System.Collections.Generic.KeyValuePair<System.String,System.Object?> arg2, System.Nullable<System.Int32> arg3)");
    }
}
