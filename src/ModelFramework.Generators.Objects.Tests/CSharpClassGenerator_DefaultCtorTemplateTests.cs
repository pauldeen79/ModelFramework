using System.Diagnostics.CodeAnalysis;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class Ctor_DefaultClassTests
    {
        [Fact]
        public void GeneratesCodeBody()
        {
            // Arrange
            var model = new ClassConstructor(codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") });
            var rootModel = new Class("MyClass", "MyNamespace");
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
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassConstructor(codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();"), new LiteralCodeStatement("throw new NotImplementedException();") });
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
            var rootModel = new Interface("MyClass", "MyNamespace");
            var model = new ClassConstructor(codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") });
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
            var rootModel = new Interface("MyClass", "MyNamespace");
            var model = new ClassConstructor(codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") }, parameters: new[] { new Parameter("parameter1", "string"), new Parameter("parameter2", "int"), new Parameter("parameter3", "bool") });
            var sut = CreateSut(model, rootModel);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"        public MyClass(string parameter1, int parameter2, bool parameter3);
");
        }

        [Fact]
        public void GeneratesAttributes()
        {
            // Arrange
            var rootModel = new Interface("MyClass", "MyNamespace");
            var model = new ClassConstructor(codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") }, attributes: new[] { new Attribute("Attribute1"), new Attribute("Attribute2"), new Attribute("Attribute3") });
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
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassConstructor(Visibility.Internal, codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") });
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
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassConstructor(Visibility.Private, codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") });
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
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassConstructor(@static: true, codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") });
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
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassConstructor(@virtual: true, codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") });
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
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassConstructor(@abstract: true);
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
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassConstructor(Visibility.Private, @protected: true, codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();") });
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
}
