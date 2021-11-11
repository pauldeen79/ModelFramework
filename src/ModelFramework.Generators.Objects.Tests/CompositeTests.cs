using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using ModelFramework.Generators.Objects;
using ModelFramework.Generators.Shared;
using ModelFramework.Generators.Tests.POC;
using ModelFramework.Objects.Default;
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
            var model = new[] { new Class("MyClass", "MyNamespace") };
            var template = new ModelFrameworkGeneratorBase();
            var templateFileManager = new TemplateFileManager(b => template.GenerationEnvironment = b, () => template.GenerationEnvironment, BasePath);

            // Act
            TemplateRenderHelper.RenderTemplate(new CSharpClassGenerator(), templateFileManager, Session, additionalParameters: new { Model = model });
            TemplateRenderHelper.RenderTemplate(new CSharpClassGenerator(), templateFileManager, Session, additionalParameters: new { Model = model });
            templateFileManager.Process(GenerateMultipleFiles);
            var actual = templateFileManager.MultipleContentBuilder.ToString();

            // Assert
            actual.Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">
  <BasePath>C:\Temp</BasePath>
  <Contents>
    <Contents>
      <FileName i:nil=""true"" />
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>&lt;?xml version=""1.0"" encoding=""utf-16""?&gt;</d4p1:string>
        <d4p1:string>&lt;MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework""&gt;</d4p1:string>
        <d4p1:string>  &lt;BasePath i:nil=""true"" /&gt;</d4p1:string>
        <d4p1:string>  &lt;Contents&gt;</d4p1:string>
        <d4p1:string>    &lt;Contents&gt;</d4p1:string>
        <d4p1:string>      &lt;FileName&gt;MyClass.cs&lt;/FileName&gt;</d4p1:string>
        <d4p1:string>      &lt;Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays""&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System.Collections.Generic;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System.Linq;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System.Text;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;namespace MyNamespace&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;{&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;    public class MyClass&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;    {&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;    }&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;}&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>      &lt;/Lines&gt;</d4p1:string>
        <d4p1:string>      &lt;SkipWhenFileExists&gt;false&lt;/SkipWhenFileExists&gt;</d4p1:string>
        <d4p1:string>    &lt;/Contents&gt;</d4p1:string>
        <d4p1:string>  &lt;/Contents&gt;</d4p1:string>
        <d4p1:string>&lt;/MultipleContents&gt;</d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
    <Contents>
      <FileName i:nil=""true"" />
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>&lt;?xml version=""1.0"" encoding=""utf-16""?&gt;</d4p1:string>
        <d4p1:string>&lt;MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework""&gt;</d4p1:string>
        <d4p1:string>  &lt;BasePath i:nil=""true"" /&gt;</d4p1:string>
        <d4p1:string>  &lt;Contents&gt;</d4p1:string>
        <d4p1:string>    &lt;Contents&gt;</d4p1:string>
        <d4p1:string>      &lt;FileName&gt;MyClass.cs&lt;/FileName&gt;</d4p1:string>
        <d4p1:string>      &lt;Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays""&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System.Collections.Generic;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System.Linq;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System.Text;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;namespace MyNamespace&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;{&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;    public class MyClass&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;    {&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;    }&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;}&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>      &lt;/Lines&gt;</d4p1:string>
        <d4p1:string>      &lt;SkipWhenFileExists&gt;false&lt;/SkipWhenFileExists&gt;</d4p1:string>
        <d4p1:string>    &lt;/Contents&gt;</d4p1:string>
        <d4p1:string>  &lt;/Contents&gt;</d4p1:string>
        <d4p1:string>&lt;/MultipleContents&gt;</d4p1:string>
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
        public void Can_Generate_Multiple_Files_With_MultipleContentBuilder_On_Runtime_Template()
        {
            // Arrange
            var Session = new Dictionary<string, object> { { "GenerateMultipleFiles", GenerateMultipleFiles } };
            var model = new[] { new Class("MyClass", "MyNamespace") };
            var multipleContentBuilder = new MultipleContentBuilder(BasePath);

            // Act
            TemplateRenderHelper.RenderTemplate(new CSharpClassGenerator(), multipleContentBuilder, Session, additionalParameters: new { Model = model });
            TemplateRenderHelper.RenderTemplate(new CSharpClassGenerator(), multipleContentBuilder, Session, additionalParameters: new { Model = model });
            var actual = multipleContentBuilder.ToString();

            // Assert
            actual.Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
<MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework"">
  <BasePath>C:\Temp</BasePath>
  <Contents>
    <Contents>
      <FileName i:nil=""true"" />
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>&lt;?xml version=""1.0"" encoding=""utf-16""?&gt;</d4p1:string>
        <d4p1:string>&lt;MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework""&gt;</d4p1:string>
        <d4p1:string>  &lt;BasePath i:nil=""true"" /&gt;</d4p1:string>
        <d4p1:string>  &lt;Contents&gt;</d4p1:string>
        <d4p1:string>    &lt;Contents&gt;</d4p1:string>
        <d4p1:string>      &lt;FileName&gt;MyClass.cs&lt;/FileName&gt;</d4p1:string>
        <d4p1:string>      &lt;Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays""&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System.Collections.Generic;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System.Linq;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System.Text;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;namespace MyNamespace&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;{&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;    public class MyClass&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;    {&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;    }&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;}&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>      &lt;/Lines&gt;</d4p1:string>
        <d4p1:string>      &lt;SkipWhenFileExists&gt;false&lt;/SkipWhenFileExists&gt;</d4p1:string>
        <d4p1:string>    &lt;/Contents&gt;</d4p1:string>
        <d4p1:string>  &lt;/Contents&gt;</d4p1:string>
        <d4p1:string>&lt;/MultipleContents&gt;</d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
    <Contents>
      <FileName i:nil=""true"" />
      <Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays"">
        <d4p1:string>&lt;?xml version=""1.0"" encoding=""utf-16""?&gt;</d4p1:string>
        <d4p1:string>&lt;MultipleContents xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.datacontract.org/2004/07/TextTemplateTransformationFramework""&gt;</d4p1:string>
        <d4p1:string>  &lt;BasePath i:nil=""true"" /&gt;</d4p1:string>
        <d4p1:string>  &lt;Contents&gt;</d4p1:string>
        <d4p1:string>    &lt;Contents&gt;</d4p1:string>
        <d4p1:string>      &lt;FileName&gt;MyClass.cs&lt;/FileName&gt;</d4p1:string>
        <d4p1:string>      &lt;Lines xmlns:d4p1=""http://schemas.microsoft.com/2003/10/Serialization/Arrays""&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System.Collections.Generic;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System.Linq;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;using System.Text;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;namespace MyNamespace&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;{&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;    public class MyClass&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;    {&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;    }&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;}&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>        &lt;d4p1:string&gt;&lt;/d4p1:string&gt;</d4p1:string>
        <d4p1:string>      &lt;/Lines&gt;</d4p1:string>
        <d4p1:string>      &lt;SkipWhenFileExists&gt;false&lt;/SkipWhenFileExists&gt;</d4p1:string>
        <d4p1:string>    &lt;/Contents&gt;</d4p1:string>
        <d4p1:string>  &lt;/Contents&gt;</d4p1:string>
        <d4p1:string>&lt;/MultipleContents&gt;</d4p1:string>
      </Lines>
      <SkipWhenFileExists>false</SkipWhenFileExists>
    </Contents>
  </Contents>
</MultipleContents>");
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
            var actual = templateFileManager.MultipleContentBuilder.ToString();

            // Assert
            actual.Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
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
            actual.Should().Be(@"<?xml version=""1.0"" encoding=""utf-16""?>
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
