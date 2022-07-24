namespace ModelFramework.CodeGeneration.Tests;

public class CodeGenerationTests
{
    private static readonly CodeGenerationSettings Settings = new CodeGenerationSettings
    (
        basePath: Path.Combine(Directory.GetCurrentDirectory(), @"../../../../"),
        generateMultipleFiles: true,
        dryRun: true
    );

    // Bootstrap test that generates c# code for the model used in code generation :)
    [Fact]
    public void Can_Generate_Model_For_Abstractions()
    {
        // Act & Assert
        ///Verify(GenerateCode.For<TestInterfacesModels>(Settings));
        Verify(GenerateCode.For<ObjectsInterfacesModels>(Settings));
    }

    [Fact]
    public void Can_Generate_All_Classes_For_ModelFramework()
    {
        // Arrange
        var multipleContentBuilder = new MultipleContentBuilder(Settings.BasePath);
        
        // Act
        GenerateCode.For<CommonBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<CommonRecords>(Settings, multipleContentBuilder);

        GenerateCode.For<DatabaseBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<DatabaseRecords>(Settings, multipleContentBuilder);

        GenerateCode.For<ObjectsBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsRecords>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsBaseBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsBaseRecords>(Settings, multipleContentBuilder);

        GenerateCode.For<ObjectsCodeStatements>(Settings, multipleContentBuilder);
        GenerateCode.For<DatabaseCodeStatements>(Settings, multipleContentBuilder);

        // Assert
        Verify(multipleContentBuilder);
    }

    [Fact]
    public void Can_Generate_Test_Classes_For_ModelFramework()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: Path.Combine(Directory.GetCurrentDirectory(), @"../../../../"),
            generateMultipleFiles: true,
            dryRun: true
        );
        var multipleContentBuilder = new MultipleContentBuilder(settings.BasePath);

        // Act
        GenerateCode.For<PlainBuilders>(settings, multipleContentBuilder);
        GenerateCode.For<CodeGenerationProviders.TestRecords>(settings, multipleContentBuilder);

        // Assert
        Verify(multipleContentBuilder);
    }

    // Example how to generate builder extensions
    [Fact]
    public void Can_Generate_Builder_Extensions_For_ModelFramework()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<CommonBuildersExtensions>(settings);
        var actual = generatedCode.GenerationEnvironment.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
    }

    [Fact]
    public void Can_Generate_Plain_Immutable_Class()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<PlainRecords>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
#nullable enable
    public partial record TestClass
    {
        public string TestProperty
        {
            get;
        }

        public TestClass(string testProperty)
        {
            this.TestProperty = testProperty;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}
");
    }

    [Fact]
    public void Can_Generate_Plain_Immutable_ClassBuilder()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<PlainBuilders>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Builders
{
#nullable enable
    public partial class TestClassBuilder
    {
        public string TestProperty
        {
            get
            {
                return _testPropertyDelegate.Value;
            }
            set
            {
                _testPropertyDelegate = new (() => value);
            }
        }

        public Test.TestClass Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new Test.TestClass(TestProperty);
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public TestClassBuilder WithTestProperty(System.Func<string> testPropertyDelegate)
        {
            _testPropertyDelegate = new (testPropertyDelegate);
            return this;
        }

        public TestClassBuilder WithTestProperty(string testProperty)
        {
            TestProperty = testProperty;
            return this;
        }

        public TestClassBuilder()
        {
            #pragma warning disable CS8603 // Possible null reference return.
            _testPropertyDelegate = new (() => string.Empty);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public TestClassBuilder(Test.TestClass source)
        {
            _testPropertyDelegate = new (() => source.TestProperty);
        }

        private System.Lazy<string> _testPropertyDelegate;
    }
#nullable restore
}
");
    }

    [Fact]
    public void Can_Generate_CustomProperties_Immutable_Class()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<CustomPropertiesRecords>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
#nullable enable
    public partial record TestClass
    {
        public TestClass TestProperty
        {
            get;
        }

        public TestClass(TestClass testProperty)
        {
            this.TestProperty = testProperty;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
}
");
    }

    [Fact]
    public void Can_Generate_CustomProperties_Immutable_ClassBuilder()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<CustomPropertiesBuilders>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Builders
{
#nullable enable
    public partial class TestClassBuilder
    {
        public TestClassBuilder TestProperty
        {
            get
            {
                return _testPropertyDelegate.Value;
            }
            set
            {
                _testPropertyDelegate = new (() => value);
            }
        }

        public Test.TestClass Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new Test.TestClass(TestProperty?.Build());
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public TestClassBuilder WithTestProperty(System.Func<TestClassBuilder> testPropertyDelegate)
        {
            _testPropertyDelegate = new (testPropertyDelegate);
            return this;
        }

        public TestClassBuilder WithTestProperty(TestClassBuilder testProperty)
        {
            TestProperty = testProperty;
            return this;
        }

        public TestClassBuilder()
        {
            #pragma warning disable CS8603 // Possible null reference return.
            _testPropertyDelegate = new (() => default);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public TestClassBuilder(Test.TestClass source)
        {
            TestProperty = new TestClassBuilder(source.TestProperty);
        }

        private System.Lazy<TestClassBuilder> _testPropertyDelegate;
    }
#nullable restore
}
");
    }

    [Fact]
    public void Can_Generate_Immutable_ClassBuilder_With_Additional_Constructor_Argument()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<AdditionalConstructorArgumentBuilders>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Builders
{
#nullable enable
    public partial class TestClassBuilder
    {
        public string TestProperty
        {
            get
            {
                return _testPropertyDelegate.Value;
            }
            set
            {
                _testPropertyDelegate = new (() => value);
            }
        }

        public Test.TestClass Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new Test.TestClass(TestProperty);
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public TestClassBuilder WithTestProperty(System.Func<string> testPropertyDelegate)
        {
            _testPropertyDelegate = new (testPropertyDelegate);
            return this;
        }

        public TestClassBuilder WithTestProperty(string testProperty)
        {
            TestProperty = testProperty;
            return this;
        }

        public TestClassBuilder()
        {
            #pragma warning disable CS8603 // Possible null reference return.
            _testPropertyDelegate = new (() => string.Empty);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public TestClassBuilder(Test.TestClass source, string additionalArgument)
        {
            _testPropertyDelegate = new (() => source.TestProperty);
        }

        private System.Lazy<string> _testPropertyDelegate;
    }
#nullable restore
}
");
    }

    private void Verify(GenerateCode generatedCode)
    {
        if (Settings.DryRun)
        {
            // Act
            var actual = generatedCode.GenerationEnvironment.ToString();

            // Assert
            actual.NormalizeLineEndings().Should().NotBeNullOrEmpty().And.NotStartWith("Error:");
        }
    }

    private void Verify(MultipleContentBuilder multipleContentBuilder)
    {
        var actual = multipleContentBuilder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().NotBeNullOrEmpty();
    }

    private abstract class PlainBase : CSharpClassBase
    {
        public override string Path => @"C:\Temp";
        public override string DefaultFileName => "GeneratedCode.cs";
        public override bool RecurseOnDeleteGeneratedFiles => false;

        protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
        protected override bool EnableNullableContext => true;
        protected override bool CreateCodeGenerationHeader => false;

        protected ITypeBase[] GetModels() => new[]
        {
            new ClassBuilder()
                .WithName("TestClass")
                .WithNamespace("Test")
                .AddProperties(new ClassPropertyBuilder().WithName("TestProperty").WithType(typeof(string)))
                .Build()
        };
    }

    private sealed class PlainRecords : PlainBase
    {
        public override object CreateModel() => GetImmutableClasses(GetModels(), "Test");
    }

    private sealed class PlainBuilders : PlainBase
    {
        public override object CreateModel() => GetImmutableBuilderClasses(GetModels(), "Test", "Test.Builders");
    }

    private sealed class AdditionalConstructorArgumentBuilders : PlainBase
    {
        public override object CreateModel() => GetImmutableBuilderClasses(GetModels(), "Test", "Test.Builders");

        protected override bool AddCopyConstructor => true;
        protected override void PostProcessImmutableBuilderClass(ClassBuilder classBuilder)
        {
            // Note that this method is fired after creating the builder class, so it's no use to add metadata for extra metadata. You have to manipulate the constructor yourself, here...
            classBuilder.Constructors[1].AddParameters(new ParameterBuilder().WithType(typeof(string)).WithName("additionalArgument"));
            base.PostProcessImmutableBuilderClass(classBuilder); //note that this is actually optional, as the base implementation does nothing
        }
    }

    private abstract class CustomPropertiesBase : CSharpClassBase
    {
        public override string Path => @"C:\Temp";
        public override string DefaultFileName => "GeneratedCode.cs";
        public override bool RecurseOnDeleteGeneratedFiles => false;

        protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
        protected override bool EnableNullableContext => true;
        protected override bool CreateCodeGenerationHeader => false;

        protected override void FixImmutableClassProperties(InterfaceBuilder interfaceBuilder) => FixInterface(interfaceBuilder, false);
        protected override void FixImmutableClassProperties(ClassBuilder classBuilder) => FixClass(classBuilder, false);
        protected override void FixImmutableBuilderProperties(InterfaceBuilder interfaceBuilder) => FixInterface(interfaceBuilder, true);
        protected override void FixImmutableBuilderProperties(ClassBuilder classBuilder) => FixClass(classBuilder, true);

        private static void FixInterface(InterfaceBuilder interfaceBuilder, bool forBuilder)
        {
            if (interfaceBuilder == null)
            {
                // Not possible, but needs to be added because TTTF.Runtime doesn't support nullable reference types
                return;
            }

            foreach (var property in interfaceBuilder.Properties)
            {
                FixImmutableBuilderProperty(property, forBuilder);
            }
        }

        private static void FixClass(ClassBuilder classBuilder, bool forBuilder)
        {
            if (classBuilder == null)
            {
                // Not possible, but needs to be added because TTTF.Runtime doesn't support nullable reference types
                return;
            }

            foreach (var property in classBuilder.Properties)
            {
                FixImmutableBuilderProperty(property, forBuilder);
            }
        }

        private static void FixImmutableBuilderProperty(ClassPropertyBuilder property, bool forBuilder)
        {
            var typeName = property.TypeName.FixTypeName();
            if (typeName.StartsWith("Test.Contracts.", StringComparison.InvariantCulture))
            {
                if (forBuilder)
                {
                    property.ConvertSinglePropertyToBuilderOnBuilder(typeName.Replace("Test.Contracts.", string.Empty) + "Builder");
                }
                else
                {
                    property.TypeName = typeName.Replace("Test.Contracts.", string.Empty);
                }
            }
            else if (typeName.Contains("Collection<Test.Contracts.", StringComparison.InvariantCulture))
            {
                if (forBuilder)
                {
                    property.ConvertCollectionPropertyToBuilderOnBuilder(addNullChecks: false, argumentType: typeName.Replace("Test.Contracts.", string.Empty, StringComparison.InvariantCulture).ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture));
                }
                else
                {
                    property.TypeName = typeName.Replace("Test.Contracts.", string.Empty, StringComparison.InvariantCulture);
                }
            }
        }

        protected ITypeBase[] GetModels() => new[]
        {
            new ClassBuilder()
                .WithName("TestClass")
                .WithNamespace("Test.Contracts")
                .AddProperties(new ClassPropertyBuilder().WithName("TestProperty").WithTypeName("Test.Contracts.TestClass"))
                .Build()
        };
    }

    private sealed class CustomPropertiesRecords : CustomPropertiesBase
    {
        public override object CreateModel() => GetImmutableClasses(GetModels(), "Test");
    }

    private sealed class CustomPropertiesBuilders : CustomPropertiesBase
    {
        public override object CreateModel() => GetImmutableBuilderClasses(GetModels(), "Test", "Test.Builders");
    }
}
