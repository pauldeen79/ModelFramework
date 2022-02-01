namespace ModelFramework.Generators.Objects.Tests;

public class CSharpClassGenerator_DefaultPropertyTemplateClassTests
{
    [Fact]
    public void GeneratesCodeBodyFromCodeStatementsCorrectly()
    {
        // Arrange
        var model = new ClassPropertyBuilder()
            .WithName("Name")
            .WithTypeName("string")
            .AddGetterLiteralCodeStatements("return null;")
            .AddSetterLiteralCodeStatements("// ignore value")
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public string Name
        {
            get
            {
                return null;
            }
            set
            {
                // ignore value
            }
        }
");
    }

    [Fact]
    public void Generates_Property_Correctly()
    {
        // Arrange
        var model = new ClassPropertyBuilder()
            .WithName("MyProperty")
            .WithType(typeof(string))
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public string MyProperty
        {
            get;
            set;
        }
");
    }

    [Fact]
    public void Generates_Internal_Property_Correctly()
    {
        // Arrange
        var model = new ClassPropertyBuilder()
            .WithName("MyProperty")
            .WithType(typeof(string))
            .WithVisibility(Visibility.Internal)
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        internal string MyProperty
        {
            get;
            set;
        }
");
    }

    [Fact]
    public void Generates_Init_Property_Correctly()
    {
        // Arrange
        var model = new ClassPropertyBuilder()
            .WithName("MyProperty")
            .WithType(typeof(string))
            .WithHasInitializer()
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public string MyProperty
        {
            get;
            init;
        }
");
    }

    [Fact]
    public void Generates_Property_With_Different_Setter_Visibility_Correcly()
    {
        // Arrange
        var model = new ClassPropertyBuilder()
            .WithName("MyProperty")
            .WithType(typeof(string))
            .WithSetterVisibility(Visibility.Private)
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public string MyProperty
        {
            get;
            private set;
        }
");
    }

    [Fact]
    public void Can_Generate_Int_Property()
    {
        // Arrange
        var model = new ClassPropertyBuilder()
            .WithName("Property")
            .WithType(typeof(int))
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model, rootAdditionalParameters: new { EnableNullableContext = true });

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public int Property
        {
            get;
            set;
        }
");
    }

    [Fact]
    public void Can_Generate_Nullable_Int_Property()
    {
        // Arrange
        var model = new ClassPropertyBuilder()
            .WithName("Property")
            .WithType(typeof(int))
            .WithIsNullable()
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model, rootAdditionalParameters: new { EnableNullableContext = true });

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public int? Property
        {
            get;
            set;
        }
");
    }

    [Fact]
    public void Can_Generate_Nullable_Int_Property_From_Nullable_Type()
    {
        // Arrange
        var model = new ClassPropertyBuilder()
            .WithName("Property")
            .WithType(typeof(int?))
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model, rootAdditionalParameters: new { EnableNullableContext = true });

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public System.Nullable<int> Property
        {
            get;
            set;
        }
");
    }

    [Fact]
    public void Can_Generate_Required_String_Property()
    {
        // Arrange
        var model = new ClassPropertyBuilder()
            .WithName("Property")
            .WithType(typeof(string))
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model, rootAdditionalParameters: new { EnableNullableContext = true });

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public string Property
        {
            get;
            set;
        }
");
    }

    [Fact]
    public void Can_Generate_Nullable_String_Property()
    {
        // Arrange
        var model = new ClassPropertyBuilder()
            .WithName("Property")
            .WithType(typeof(string))
            .WithIsNullable()
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model, rootAdditionalParameters: new { EnableNullableContext = true });

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public string? Property
        {
            get;
            set;
        }
");
    }
}
