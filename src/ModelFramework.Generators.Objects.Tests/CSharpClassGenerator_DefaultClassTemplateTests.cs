namespace ModelFramework.Generators.Objects.Tests;

public class CSharpClassGenerator_DefaultClassTemplateTests
{
    [Fact]
    public void Can_Generate_NullableContext_Class()
    {
        // Arrange
        var typeBaseMock = CreateTypeBaseMock();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultClassTemplate>(typeBaseMock, rootAdditionalParameters: new { EnableNullableContext = true, CreateCodeGenerationHeader = true });

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"#nullable enable
    public class Test
    {
    }
#nullable restore
");
    }

    [Fact]
    public void Can_Generate_Class_With_Single_Generic_Type_Argument()
    {
        // Arrange
        var model = new ClassBuilder()
            .WithName("Test")
            .AddGenericTypeArguments("T")
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultClassTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"    public class Test<T>
    {
    }
");
    }

    [Fact]
    public void Can_Generate_Class_With_Single_Generic_Type_Argument_And_Constraint()
    {
        // Arrange
        var model = new ClassBuilder()
            .WithName("Test")
            .AddGenericTypeArguments("T")
            .AddGenericTypeArgumentConstraints("where T : class")
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultClassTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"    public class Test<T>
        where T : class
    {
    }
");
    }

    [Fact]
    public void Can_Generate_Class_With_Multiple_Generic_Type_Arguments_And_Constraints()
    {
        // Arrange
        var model = new ClassBuilder()
            .WithName("Test")
            .AddGenericTypeArguments("T1", "T2")
            .AddGenericTypeArgumentConstraints("where T1 : class", "where T2 : new()")
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultClassTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"    public class Test<T1, T2>
        where T1 : class
        where T2 : new()
    {
    }
");
    }

    [Fact]
    public void CanTestViewModel()
    {
        // Arrange
        var typeBaseMock = CreateTypeBaseMock();
        var sut = new CSharpClassGenerator_DefaultClassViewModel
        {
            Model = typeBaseMock,
            TemplateContext = TemplateRenderHelper.CreateNestedTemplateContext<CSharpClassGenerator, CSharpClassGenerator_DefaultClassTemplate, TemplateInstanceContext>(typeBaseMock, rootAdditionalParameters: new { EnableNullableContext = true, CreateCodeGenerationHeader = true })
        };

        // Act
        var actual = sut.EnableNullableContext;

        // Assert
        actual.Should().BeTrue();
    }

    private static IClass CreateTypeBaseMock()
    {
        var typeBaseMock = Substitute.For<IClass>();
        typeBaseMock.Name.Returns("Test");
        typeBaseMock.Methods.Returns(new ReadOnlyValueCollection<IClassMethod>());
        typeBaseMock.Properties.Returns(new ReadOnlyValueCollection<IClassProperty>());
        typeBaseMock.Constructors.Returns(new ReadOnlyValueCollection<IClassConstructor>());
        typeBaseMock.Fields.Returns(new ReadOnlyValueCollection<IClassField>());
        typeBaseMock.Enums.Returns(new ReadOnlyValueCollection<IEnum>());
        typeBaseMock.Attributes.Returns(new ReadOnlyValueCollection<IAttribute>());
        typeBaseMock.Interfaces.Returns(new ReadOnlyValueCollection<string>());
        typeBaseMock.Metadata.Returns(new ReadOnlyValueCollection<IMetadata>());
        typeBaseMock.GenericTypeArguments.Returns(new ReadOnlyValueCollection<string>());
        typeBaseMock.GenericTypeArgumentConstraints.Returns(new ReadOnlyValueCollection<string>());
        return typeBaseMock;
    }
}
