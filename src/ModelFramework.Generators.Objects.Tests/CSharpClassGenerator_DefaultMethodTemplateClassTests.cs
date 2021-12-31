using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements.Builders;
using ModelFramework.Objects.Contracts;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
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
                .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
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
                .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
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
                    new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"),
                    new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();")
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
            var rootModel = new[] { new InterfaceBuilder().WithName("IMyInterface").WithNamespace("MyNamespace").Build() }.Cast<ITypeBase>();
            var model = new ClassMethodBuilder()
                .WithName("Name")
                .WithType(typeof(string))
                .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
                .Build();
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel, iterationContextModel: rootModel.First());

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
                .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
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
                .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
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
                    new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"),
                    new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();")
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
                .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
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
                .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
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
                .AddCodeStatements(new LiteralCodeStatementBuilder().WithStatement("throw new NotImplementedException();"))
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
    }
}
