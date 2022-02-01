namespace ModelFramework.Generators.Objects.Tests;

public class Ctor_DefaultClassTests
{
    [Fact]
    public void GeneratesCodeBody()
    {
        // Arrange
        var model = new ClassConstructorBuilder().AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();")).Build();
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var sut = CreateSut(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public MyClass()
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
        var model = new ClassConstructorBuilder()
            .AddCodeStatements
            (
                new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"),
                new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();")
            )
            .Build();
        var sut = CreateSut(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public MyClass()
        {
            throw new NotImplementedException();
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesNoCodeBodyOnInterface()
    {
        // Arrange
        var rootModel = new InterfaceBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassConstructorBuilder().AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();")).Build();
        var sut = CreateSut(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public MyClass();
");
    }

    [Fact]
    public void GeneratesParameters()
    {
        // Arrange
        var rootModel = new InterfaceBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassConstructorBuilder().AddCodeStatements
        (
            new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();")
        ).AddParameters
        (
            new ParameterBuilder().WithName("parameter1").WithType(typeof(string)),
            new ParameterBuilder().WithName("parameter2").WithType(typeof(int)),
            new ParameterBuilder().WithName("parameter3").WithType(typeof(bool))
        ).Build();
        var sut = CreateSut(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public MyClass(string parameter1, int parameter2, bool parameter3);
");
    }

    [Fact]
    public void GeneratesParamArrayParameter()
    {
        // Arrange
        var rootModel = new InterfaceBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassConstructorBuilder().AddCodeStatements
        (
            new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();")
        ).AddParameters
        (
            new ParameterBuilder().WithName("parameters").WithType(typeof(string[])).WithIsParamArray()
        ).Build();
        var sut = CreateSut(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public MyClass(params string[] parameters);
");
    }

    [Fact]
    public void GeneratesAttributes()
    {
        // Arrange
        var rootModel = new InterfaceBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassConstructorBuilder()
            .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
            .AddAttributes
            (
            new AttributeBuilder().WithName("Attribute1"),
            new AttributeBuilder().WithName("Attribute2"),
            new AttributeBuilder().WithName("Attribute3")
            ).Build();
        var sut = CreateSut(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        [Attribute1]
        [Attribute2]
        [Attribute3]
        public MyClass();
");
    }

    [Fact]
    public void GeneratesInternalCtor()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassConstructorBuilder()
            .WithVisibility(Visibility.Internal)
            .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
            .Build();
        var sut = CreateSut(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        internal MyClass()
        {
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesPrivateCtor()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassConstructorBuilder()
            .WithVisibility(Visibility.Private)
            .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
            .Build();
        var sut = CreateSut(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        private MyClass()
        {
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesStaticCtor()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassConstructorBuilder()
            .WithStatic()
            .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
            .Build();
        var sut = CreateSut(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public static MyClass()
        {
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesVirtualCtor()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassConstructorBuilder()
            .WithVirtual()
            .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
            .Build();
        var sut = CreateSut(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public virtual MyClass()
        {
            throw new NotImplementedException();
        }
");
    }

    [Fact]
    public void GeneratesAbstractCtor()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassConstructorBuilder().WithAbstract().Build();
        var sut = CreateSut(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        public abstract MyClass();
");
    }

    [Fact]
    public void GeneratesProtectedCtor()
    {
        // Arrange
        var rootModel = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build();
        var model = new ClassConstructorBuilder()
            .WithVisibility(Visibility.Private)
            .WithProtected()
            .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
            .Build();
        var sut = CreateSut(model, rootModel);

        // Act
        var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"        protected MyClass()
        {
            throw new NotImplementedException();
        }
");
    }

    private CSharpClassGenerator_DefaultCtorTemplate CreateSut(object model, object rootModel)
        => TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultCtorTemplate>(model, rootModel);
}
