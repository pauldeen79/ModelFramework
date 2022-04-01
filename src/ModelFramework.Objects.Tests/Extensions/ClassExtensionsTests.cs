namespace ModelFramework.Objects.Tests.Extensions;

public class ClassExtensionsTests
{
    [Fact]
    public void IsPoco_Returns_True_On_Class_With_Public_Parameterless_Contructor_And_Writable_Properties()
    {
        // Assert
        var input = new ClassBuilder().WithName("Test")
                                      .AddProperties(new ClassPropertyBuilder().WithName("Name")
                                                                               .WithTypeName("System.String")
                                                                               .AsReadOnly())
                                      .AddConstructors(new ClassConstructorBuilder().AddParameter("name", typeof(string))
                                                                                    .AddLiteralCodeStatements("Name = name;"))
                                      .Build();

        // Act
        var actual = input.HasPublicParameterlessConstructor();

        // Assert
        actual.Should().BeFalse();
    }

    [Fact]
    public void IsPoco_Returns_False_On_Class_With_Public_Contructor_With_Parameters_And_No_Writable_Properties()
    {
        // Assert
        var input = new ClassBuilder().WithName("Test")
                                      .AddProperties(new ClassPropertyBuilder().WithName("Name")
                                                                               .WithTypeName("System.String"))
                                      .Build();

        // Act
        var actual = input.HasPublicParameterlessConstructor();

        // Assert
        actual.Should().BeTrue();
    }
}
