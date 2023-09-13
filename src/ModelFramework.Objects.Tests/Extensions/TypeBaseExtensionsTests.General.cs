namespace ModelFramework.Objects.Tests.Extensions;

public partial class TypeBaseExtensionsTests
{
    [Fact]
    public void GetInheritedClasses_Returns_Empty_String_For_Interface_When_No_Interfaces_Are_Defined()
    {
        // Arrange
        var sut = new InterfaceBuilder().WithName("ITestInterface").Build();

        // Act
        var actual = sut.GetInheritedClasses();

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void GetInheritedClasses_Returns_Empty_String_For_Class_When_No_Interfaces_Are_Defined()
    {
        // Arrange
        var sut = new ClassBuilder().WithName("TestClass").Build();

        // Act
        var actual = sut.GetInheritedClasses();

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void GetInheritedClasses_Returns_Correct_Value_For_Interface_With_Two_Interfaces()
    {
        // Arrange
        var sut = new InterfaceBuilder().WithName("ITestInterface")
                                        .AddInterfaces("IFirst", "ISecond")
                                        .Build();

        // Act
        var actual = sut.GetInheritedClasses();

        // Assert
        actual.Should().Be(" : IFirst, ISecond");
    }

    [Fact]
    public void GetInheritedClasses_Returns_Correct_Value_For_Class_With_Two_Interfaces()
    {
        // Arrange
        var sut = new ClassBuilder().WithName("ITestInterface")
                                    .AddInterfaces("IFirst", "ISecond")
                                    .Build();

        // Act
        var actual = sut.GetInheritedClasses();

        // Assert
        actual.Should().Be(" : IFirst, ISecond");
    }

    [Fact]
    public void GetInheritedClasses_Returns_Correct_Value_For_Class_With_BaseClass_And_Two_Interfaces()
    {
        // Arrange
        var sut = new ClassBuilder().WithName("ITestInterface")
                                    .WithBaseClass("BaseClass")
                                    .AddInterfaces("IFirst", "ISecond")
                                    .Build();

        // Act
        var actual = sut.GetInheritedClasses();

        // Assert
        actual.Should().Be(" : BaseClass, IFirst, ISecond");
    }

    [Fact]
    public void GetInheritedClasses_Returns_Correct_Value_For_Class_With_BaseClass()
    {
        // Arrange
        var sut = new ClassBuilder().WithName("ITestInterface")
                                    .WithBaseClass("BaseClass")
                                    .Build();

        // Act
        var actual = sut.GetInheritedClasses();

        // Assert
        actual.Should().Be(" : BaseClass");
    }

    [Fact]
    public void GetContainerType_Returns_Correct_Value_For_Class()
    {
        // Arrange
        var sut = new ClassBuilder().WithName("Test").Build();

        // Act
        var actual = sut.GetContainerType();

        // Assert
        actual.Should().Be("class");
    }

    [Fact]
    public void GetContainerType_Returns_Correct_Value_For_Interface()
    {
        // Arrange
        var sut = new InterfaceBuilder().WithName("Test").Build();

        // Act
        var actual = sut.GetContainerType();

        // Assert
        actual.Should().Be("interface");
    }

    [Fact]
    public void GetContainerType_Throws_On_Unknown_Type()
    {
        // Arrange
        var sut = Substitute.For<ITypeBase>();
        var action = new Action(() => sut.GetContainerType());

        // Act & Assert
        action.Should().Throw<InvalidOperationException>().WithMessage("Unknown container type: [Castle.Proxies.ObjectProxy]");
    }
}
