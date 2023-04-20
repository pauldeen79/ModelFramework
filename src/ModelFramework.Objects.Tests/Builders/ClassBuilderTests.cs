namespace ModelFramework.Objects.Tests.Builders;

public class ClassBuilderTests
{
    [Fact]
    public void Can_Specify_Baseclass_Using_Type()
    {
        // Arrange
        var sut = new ClassBuilder();

        // Act
        var actual = sut.WithBaseClass(GetType());

        // Assert
        actual.BaseClass.ToString().Should().Be(GetType().FullName);
    }

    [Fact]
    public void Can_Add_Usings_For_CodeGeneration()
    {
        // Arrange
        var sut = new ClassBuilder();

        // Act
        var actual = sut.AddUsings("ModelFramework.Objects.Contracts", "ModelFramework.Database.Contracts");

        // Assert
        actual.Metadata.Select(x => $"{x.Name} = {x.Value}").Should().BeEquivalentTo(
            $"{MetadataNames.CustomUsing} = ModelFramework.Objects.Contracts",
            $"{MetadataNames.CustomUsing} = ModelFramework.Database.Contracts"
        );
    }

    [Fact]
    public void Can_Add_Abbreviated_Namespaces_For_CodeGeneration()
    {
        // Arrange
        var sut = new ClassBuilder();

        // Act
        var actual = sut.AddNamespacesToAbbreviate("ModelFramework.Objects.Contracts", "ModelFramework.Database.Contracts");

        // Assert
        actual.Metadata.Select(x => $"{x.Name} = {x.Value}").Should().BeEquivalentTo(
            $"{MetadataNames.NamespaceToAbbreviate} = ModelFramework.Objects.Contracts",
            $"{MetadataNames.NamespaceToAbbreviate} = ModelFramework.Database.Contracts"
        );
    }

    [Fact]
    public void ToString_Gives_Right_Result_With_Namespace_Empty()
    {
        // Arrange
        var sut = new ClassBuilder().WithName("Name");

        // Act
        var result = sut.ToString();

        // Assert
        result.Should().Be("Name");
    }

    [Fact]
    public void ToString_Gives_Right_Result_With_Namespace_Filled()
    {
        // Arrange
        var sut = new ClassBuilder().WithName("Name").WithNamespace("MyNamespace");

        // Act
        var result = sut.ToString();

        // Assert
        result.Should().Be("MyNamespace.Name");
    }

    [Fact]
    public void Can_Get_FullName_Without_Namespace()
    {
        // Arrange
        var builder = new ClassBuilder().WithName("Test");

        // Act
        var fullName = builder.GetFullName();

        // Assert
        fullName.Should().Be("Test");
    }

    [Fact]
    public void Can_Get_FullName_With_Namespace()
    {
        // Arrange
        var builder = new ClassBuilder().WithNamespace("MyNamespace").WithName("Test");

        // Act
        var fullName = builder.GetFullName();

        // Assert
        fullName.Should().Be("MyNamespace.Test");
    }

    [Fact]
    public void Can_Make_Poco_Class_Readonly()
    {
        // Arrange
        var builder = new ClassBuilder().AddProperties(new ClassPropertyBuilder().WithName("Test").WithType(typeof(bool)).AsWritable());

        // Act
        builder = builder.AsReadOnly();

        // Assert
        builder.Properties.Select(x => x.HasSetter).Should().AllBeEquivalentTo(false);
    }

    [Fact]
    public void Can_Make_Immutable_Class_Writable()
    {
        // Arrange
        var builder = new ClassBuilder().AddProperties(new ClassPropertyBuilder().WithName("Test").WithType(typeof(bool)).AsReadOnly());

        // Act
        builder = builder.AsWritable();

        // Assert
        builder.Properties.Select(x => x.HasSetter).Should().AllBeEquivalentTo(true);
    }
}
