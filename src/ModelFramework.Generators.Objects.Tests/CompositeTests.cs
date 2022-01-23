using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CrossCutting.Common.Extensions;
using FluentAssertions;
using ModelFramework.Generators.Objects;
using ModelFramework.Generators.Shared;
using ModelFramework.Generators.Tests.POC;
using ModelFramework.Objects.Builders;
using TextTemplateTransformationFramework.Runtime;
using Xunit;

namespace ModelFramework.Generators.Tests
{
    [ExcludeFromCodeCoverage]
    public class CompositeTests
    {
        private const string BasePath = @"C:\Temp";
        private const bool GenerateMultipleFiles = true;

        /// <summary>
        /// This scenario is suited for calling sattelite/composite templates from within another template (that uses the Runtime T4PlusGeneratedTemplateBase class as base class)
        /// </summary>
        [Fact]
        public void Can_Generate_Multiple_Files_With_TemplateFileManager_On_Runtime_Template()
        {
            // Arrange
            var Session = new Dictionary<string, object> { { "GenerateMultipleFiles", GenerateMultipleFiles } };
            var model = new[] { new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build() };
            var template = new ModelFrameworkGeneratorBase();
            var templateFileManager = new TemplateFileManager(b => template.GenerationEnvironment = b, () => template.GenerationEnvironment, BasePath);

            // Act
            TemplateRenderHelper.RenderTemplate(new CSharpClassGenerator(), templateFileManager, Session, additionalParameters: new { Model = model });
            TemplateRenderHelper.RenderTemplate(new CSharpClassGenerator(), templateFileManager, Session, additionalParameters: new { Model = model });
            templateFileManager.Process(GenerateMultipleFiles);
            var actual = templateFileManager.MultipleContentBuilder.ToString();

            // Assert
            actual.Should().NotBeEmpty();
        }

        /// <summary>
        /// This scenario is suited for calling sattelite/composite templates from any .Net program, i.e. a console app, web API, unit test, or a template (that uses the Runtime T4PlusGeneratedTemplateBase as a base class)
        /// </summary>
        [Fact]
        public void Can_Generate_Multiple_Files_With_MultipleContentBuilder_On_Runtime_Template()
        {
            // Arrange
            var Session = new Dictionary<string, object> { { "GenerateMultipleFiles", GenerateMultipleFiles } };
            var model = new[] { new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").Build() };
            var multipleContentBuilder = new MultipleContentBuilder(BasePath);

            // Act
            TemplateRenderHelper.RenderTemplate(new CSharpClassGenerator(), multipleContentBuilder, Session, additionalParameters: new { Model = model });
            TemplateRenderHelper.RenderTemplate(new CSharpClassGenerator(), multipleContentBuilder, Session, additionalParameters: new { Model = model });
            var actual = multipleContentBuilder.ToString();

            // Assert
            actual.Should().NotBeEmpty();
        }

        /// <summary>
        /// This scenario is suited for calling sattelite/composite templates from within another template (that uses the Runtime T4PlusGeneratedTemplateBase class as base class)
        /// </summary>
        [Fact]
        public void Can_Generate_Multiple_Files_With_TemplateFileManager_On_Non_Runtime_Template()
        {
            // Arrange
            var Session = new Dictionary<string, object> { { "GenerateMultipleFiles", GenerateMultipleFiles } };
            const string Model = "Unit test";
            var template = new ModelFrameworkGeneratorBase();
            var templateFileManager = new TemplateFileManager(b => template.GenerationEnvironment = b, () => template.GenerationEnvironment, BasePath);

            // Act
            TemplateRenderHelper.RenderTemplate(new MultipleOutputExample(), templateFileManager, Session, additionalParameters: new { Model });
            TemplateRenderHelper.RenderTemplate(new MultipleOutputExample(), templateFileManager, Session, additionalParameters: new { Model });
            templateFileManager.Process(GenerateMultipleFiles);
            var actual = templateFileManager.MultipleContentBuilder.ToString() ?? string.Empty;

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">
  <BasePath>C:\Temp</BasePath>
  <Contents>
    <Contents>
      <FileName i:nil=""true"" />
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>Hello, Unit test</d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
    <Contents>
      <FileName i:nil=""true"" />
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>Hello, Unit test</d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
  </Contents>
</MultipleContents>");
        }

        /// <summary>
        /// This scenario is suited for calling sattelite/composite templates from any .Net program, i.e. a console app, web API, unit test, or a template (that uses the Runtime T4PlusGeneratedTemplateBase as a base class)
        /// </summary>
        [Fact]
        public void Can_Generate_Multiple_Files_With_MultipleContentBuilder_On_Non_Runtime_Template()
        {
            // Arrange
            var Session = new Dictionary<string, object> { { "GenerateMultipleFiles", GenerateMultipleFiles } };
            const string Model = "Unit test";
            var multipleContentBuilder = new MultipleContentBuilder(BasePath);

            // Act
            TemplateRenderHelper.RenderTemplate(new MultipleOutputExample(), multipleContentBuilder, Session, additionalParameters: new { Model });
            TemplateRenderHelper.RenderTemplate(new MultipleOutputExample(), multipleContentBuilder, Session, additionalParameters: new { Model });
            var actual = multipleContentBuilder.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">
  <BasePath>C:\Temp</BasePath>
  <Contents>
    <Contents>
      <FileName i:nil=""true"" />
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>Hello, Unit test</d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
    <Contents>
      <FileName i:nil=""true"" />
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>Hello, Unit test</d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
  </Contents>
</MultipleContents>");
        }
    }
}
