using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.Default;
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
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassMethod("Name", "string", body: "throw new NotImplementedException();");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        public string Name()
        {
            throw new NotImplementedException();
        }
");
        }

        [Fact]
        public void GeneratesCodeBodyWithoutReturnValueWhenTypeIsNotSupplied()
        {
            // Arrange
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassMethod("Name", null, body: "throw new NotImplementedException();");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        public void Name()
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
            var model = new ClassMethod("Name", null, codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();"), new LiteralCodeStatement("throw new NotImplementedException();") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        public void Name()
        {
            throw new NotImplementedException();
            throw new NotImplementedException();
        }
");
        }

        [Fact]
        public void GeneratesCodeBodyWithoutCodeWhenBodyIsNullAndCodeStatementsAreEmpty()
        {
            // Arrange
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassMethod("Name", null);
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        public void Name()
        {
        }
");
        }

        [Fact]
        public void GeneratesNoCodeBodyOnInterface()
        {
            // Arrange
            var rootModel = new[] { new Interface("IMyInterface", "MyNamespace") }.Cast<ModelFramework.Objects.Contracts.ITypeBase>();
            var model = new ClassMethod("Name", "string", body: "throw new NotImplementedException();");
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel, iterationContextModel: rootModel.First());

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        string Name();
");
        }

        [Fact]
        public void GeneratesParameters()
        {
            // Arrange
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassMethod("Name", "string", body: "throw new NotImplementedException();", parameters: new[] { new Parameter("parameter1", "string"), new Parameter("parameter2", "int"), new Parameter("parameter3", "bool") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        public string Name(string parameter1, int parameter2, bool parameter3)
        {
            throw new NotImplementedException();
        }
");
        }

        [Fact]
        public void GeneratesExtensionMethod()
        {
            // Arrange
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassMethod("Name", "string", body: "throw new NotImplementedException();", parameters: new[] { new Parameter("parameter1", "string"), new Parameter("parameter2", "int"), new Parameter("parameter3", "bool") }, extensionMethod: true);
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        public string Name(this string parameter1, int parameter2, bool parameter3)
        {
            throw new NotImplementedException();
        }
");
        }

        [Fact]
        public void GeneratesExplicitInterfaceMethod()
        {
            // Arrange
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassMethod("Name", null, explicitInterfaceName: "IMyInterface", codeStatements: new[] { new LiteralCodeStatement("throw new NotImplementedException();"), new LiteralCodeStatement("throw new NotImplementedException();") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        void IMyInterface.Name()
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
            var rootModel = new Class("MyClass", "MyNamespace");
            var model = new ClassMethod("Name", null, body: "throw new NotImplementedException();", attributes: new[] { new Attribute("Attribute1"), new Attribute("Attribute2"), new Attribute("Attribute3") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultMethodTemplate>(model, rootModel);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        [Attribute1]
        [Attribute2]
        [Attribute3]
        public void Name()
        {
            throw new NotImplementedException();
        }
");
        }

        //Modifiers
    }
}
