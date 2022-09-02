namespace ModelFramework.Objects.Tests.Extensions;

public partial class TypeBaseExtensionsTests
{
    [Fact]
    public void Generating_ImmutableClass_From_Class_Without_Properties_Throws_Exception()
    {
        // Arrange
        var input = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();

        // Act & Assert
        input.Invoking(x => x.ToImmutableClass(new ImmutableClassSettings()))
             .Should().Throw<InvalidOperationException>()
             .WithMessage("To create an immutable class, there must be at least one property");
    }

    [Fact]
    public void Can_Build_ImmutableClass_Without_NullChecks()
    {
        // Arrange
        var input = new ClassBuilder()
            .WithName("MyClass")
            .WithNamespace("MyNamespace")
            .AddProperties(new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)))
            .AddProperties(new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int)))
            .Build();
        var settings = new ImmutableClassSettings(constructorSettings: new ImmutableClassConstructorSettings(addNullChecks: false));

        // Act
        var actual = input.ToImmutableClass(settings);

        // Assert
        actual.Constructors.Should().HaveCount(1);
        string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"this.Property1 = property1;
this.Property2 = property2;");
    }

    [Fact]
    public void Can_Build_ImmutableClass_With_NullChecks()
    {
        // Arrange
        var input = new ClassBuilder()
            .WithName("MyClass")
            .WithNamespace("MyNamespace")
            .AddProperties(new ClassPropertyBuilder().WithName("Property1").WithType(typeof(string)))
            .AddProperties(new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int)).WithIsNullable()) //TODO: Check if we can fill IsNullable automatically when calling WithType
            .Build();
        var settings = new ImmutableClassSettings(constructorSettings: new ImmutableClassConstructorSettings(addNullChecks: true));

        // Act
        var actual = input.ToImmutableClass(settings);

        // Assert
        actual.Constructors.Should().HaveCount(1);
        string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"if (property1 == null) throw new System.ArgumentNullException(""property1"");
this.Property1 = property1;
this.Property2 = property2;");
    }

    [Fact]
    public void Can_Build_ImmutableClass_With_Correct_NullChecks_On_Custom_Types()
    {
        // Arrange
        var input = new ClassBuilder()
            .WithName("MyClass")
            .WithNamespace("MyNamespace")
            .AddProperties(new ClassPropertyBuilder().WithName("Property1").WithTypeName("MyNullableType").WithConstructorNullCheck(false))
            .AddProperties(new ClassPropertyBuilder().WithName("Property2").WithTypeName("MyNonNullableType").WithConstructorNullCheck(true))
            .Build();
        var settings = new ImmutableClassSettings(constructorSettings: new ImmutableClassConstructorSettings(addNullChecks: true));

        // Act
        var actual = input.ToImmutableClass(settings);

        // Assert
        actual.Constructors.Should().HaveCount(1);
        string.Join(Environment.NewLine, actual.Constructors.First().CodeStatements.Select(x => x.ToString())).Should().Be(@"if (property2 == null) throw new System.ArgumentNullException(""property2"");
this.Property1 = property1;
this.Property2 = property2;");
    }
}
