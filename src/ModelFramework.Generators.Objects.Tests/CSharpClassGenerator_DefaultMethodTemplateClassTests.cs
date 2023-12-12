namespace ModelFramework.Generators.Objects.Tests;

public class Method_DefaultClassTests
{
    [Fact]
    public void GeneratesCodeBodyWithReturnValueWhenTypeIsSupplied()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassMethodBuilder()
            .WithName("Name")
            .WithType(typeof(string))
            .AddCodeStatements(new LiteralCodeStatementBuilder("throw new NotImplementedException();"))
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public string Name()
        {
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesCodeBodyWithoutReturnValueWhenTypeIsNotSupplied()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassMethodBuilder()
            .WithName("Name")
            .WithTypeName(string.Empty)
            .AddCodeStatements(new LiteralCodeStatementBuilder("throw new NotImplementedException();"))
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public void Name()
        {
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesCodeStatements()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassMethodBuilder()
            .WithName("Name")
            .AddCodeStatements
            (
                new LiteralCodeStatementBuilder("throw new NotImplementedException();"),
                new LiteralCodeStatementBuilder("throw new NotImplementedException();")
            ).Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public void Name()
        {
            throw new NotImplementedException();
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesCodeBodyWithoutCodeWhenCodeStatementsAreEmpty()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassMethodBuilder().WithName("Name").Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public void Name()
        {
        }
");
    }

    [Fact]
    public void GeneratesNoCodeBodyOnInterface()
    {
        // Arrange
        var rootModel = new[] { new InterfaceBuilder().WithName("IMyInterface").WithNamespace("MyNamespace").Build() };
        var model = new ClassMethodBuilder()
            .WithName("Name")
            .WithType(typeof(string))
            .AddCodeStatements(new LiteralCodeStatementBuilder("throw new NotImplementedException();"))
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel, iterationContextModel: rootModel[0]);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        string Name();
");
    }

    [Fact]
    public void GeneratesParameters()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassMethodBuilder()
            .WithName("Name")
            .WithType(typeof(string))
            .AddCodeStatements(new LiteralCodeStatementBuilder("throw new NotImplementedException();"))
            .AddParameters
            (
                new ParameterBuilder().WithName("parameter1").WithType(typeof(string)),
                new ParameterBuilder().WithName("parameter2").WithType(typeof(int)),
                new ParameterBuilder().WithName("parameter3").WithType(typeof(bool))
            ).Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public string Name(string parameter1, int parameter2, bool parameter3)
        {
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesExtensionMethod()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassMethodBuilder()
            .WithName("Name")
            .WithType(typeof(string))
            .AddCodeStatements(new LiteralCodeStatementBuilder("throw new NotImplementedException();"))
            .AddParameters
            (
                new ParameterBuilder().WithName("parameter1").WithTypeName("string"),
                new ParameterBuilder().WithName("parameter2").WithTypeName("int"),
                new ParameterBuilder().WithName("parameter3").WithTypeName("bool")
            ).WithExtensionMethod()
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public string Name(this string parameter1, int parameter2, bool parameter3)
        {
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesExplicitInterfaceMethod()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassMethodBuilder()
            .WithName("Name")
            .WithExplicitInterfaceName("IMyInterface")
            .AddCodeStatements
            (
                new LiteralCodeStatementBuilder("throw new NotImplementedException();"),
                new LiteralCodeStatementBuilder("throw new NotImplementedException();")
            ).Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        void IMyInterface.Name()
        {
            throw new NotImplementedException();
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesAttributes()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassMethodBuilder()
            .WithName("Name")
            .AddCodeStatements(new LiteralCodeStatementBuilder("throw new NotImplementedException();"))
            .AddAttributes
            (
                new AttributeBuilder().WithName("Attribute1"),
                new AttributeBuilder().WithName("Attribute2"),
                new AttributeBuilder().WithName("Attribute3")
            ).Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        [Attribute1]
        [Attribute2]
        [Attribute3]
        public void Name()
        {
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesPrivateMethod()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassMethodBuilder()
            .WithName("Name")
            .WithType(typeof(string))
            .AddCodeStatements(new LiteralCodeStatementBuilder("throw new NotImplementedException();"))
            .WithVisibility(Visibility.Private)
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        private string Name()
        {
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesParamArrayArgument()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassMethodBuilder()
            .WithName("Name")
            .WithType(typeof(string))
            .AddCodeStatements(new LiteralCodeStatementBuilder("throw new NotImplementedException();"))
            .AddParameters
            (
                new ParameterBuilder().WithName("parameters").WithType(typeof(string[])).WithIsParamArray()
            ).Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public string Name(params string[] parameters)
        {
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void Can_Generate_Method_With_Multiple_Generic_Type_Arguments_And_Constraints()
    {
        // Arrange
        var model = new ClassMethodBuilder()
            .WithName("Test")
            .AddGenericTypeArguments("T1", "T2")
            .AddGenericTypeArgumentConstraints("where T1 : class", "where T2 : new()")
            .Build();
        var rootModel = new ClassBuilder().WithName("Test").Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public void Test<T1, T2>()
            where T1 : class
            where T2 : new()
        {
        }
");
    }

    [Fact]
    public void Can_Generate_Method_With_Abbreviated_Namespace_In_ReturnType()
    {
        // Arrange
        var rootModel = new ClassBuilder()
            .WithName("MyClass")
            .WithNamespace("MyNamespace")
            .AddNamespacesToAbbreviate("MyNamespace")
            .Build();
        var model = new ClassMethodBuilder()
            .WithName("Name")
            .WithTypeName("MyNamespace.MyClass")
            .AddCodeStatements(new LiteralCodeStatementBuilder("throw new NotImplementedException();"))
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public MyClass Name()
        {
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void Can_Generate_Method_With_Custom_Template_On_CodeStatement()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassMethodBuilder()
            .WithName("Name")
            .WithType(typeof(string))
            .AddCodeStatements(new LiteralCodeStatementBuilder("throw new NotImplementedException();"))
            .Build();
        var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>
        (
            model,
            rootModel,
            rootAdditionalActionDelegate: rootTemplate => rootTemplate.RegisterChildTemplate("Test", () => new CustomStatementTemplate(), typeof(LiteralCodeStatement))
        );

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public string Name()
        {
            throw new NotImplementedException(); // added in template
        }
");
    }

    private sealed class CustomStatementTemplate : CSharpClassGeneratorBaseChild
    {
#pragma warning disable S3459 // Unassigned members should be removed
#pragma warning disable S1144 // Unused private types or members should be removed
        public LiteralCodeStatement? Model { get; set; }
#pragma warning restore S1144 // Unused private types or members should be removed
#pragma warning restore S3459 // Unassigned members should be removed

#pragma warning disable S1144 // Unused private types or members should be removed
#pragma warning disable S1172 // Unused method parameters should be removed
        public void Render(StringBuilder builder)
#pragma warning restore S1172 // Unused method parameters should be removed
        {
            if (Model == null)
            {
                Error("CustomStatementTemplate: Model is null! How did you do this?");
                return;
            }

            WriteLine(Model.Statement + " // added in template");
        }
#pragma warning restore S1144 // Unused private types or members should be removed
    }
}
