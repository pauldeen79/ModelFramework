using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.CodeStatements;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Default;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Objects.Tests
{
    [ExcludeFromCodeCoverage]
    public class CSharpClassGenerator_DefaultPropertyTemplateClassTests
    {
        [Fact]
        public void GeneratesCodeBodyFromCodeStatementsCorrectly()
        {
            // Arrange
            var model = new ClassProperty("Name", "string"
                , getterCodeStatements: new ICodeStatement[] { new LiteralCodeStatement("return null;") }
                , setterCodeStatements: new ICodeStatement[] { new LiteralCodeStatement("// ignore value") });
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        public string Name
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
            actual.Should().Be(@"        public string MyProperty
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
                .WithTypeName(typeof(string).FullName)
                .WithVisibility(Visibility.Internal)
                .Build();
            var sut = TemplateRenderHelper.CreateNestedTemplate<CSharpClassGenerator, CSharpClassGenerator_DefaultPropertyTemplate>(model);

            // Act
            var actual = TemplateRenderHelper.GetTemplateOutput(sut, model);

            // Assert
            actual.Should().Be(@"        internal string MyProperty
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
            actual.Should().Be(@"        public string MyProperty
        {
            get;
            init;
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
            actual.Should().Be(@"        public int Property
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
            actual.Should().Be(@"        public int? Property
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
            actual.Should().Be(@"        public System.Nullable<int> Property
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
            actual.Should().Be(@"        public string Property
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
            actual.Should().Be(@"        public string? Property
        {
            get;
            set;
        }
");
        }
    }
}
