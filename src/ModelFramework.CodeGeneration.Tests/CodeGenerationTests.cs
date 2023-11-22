namespace ModelFramework.CodeGeneration.Tests;

public class CodeGenerationTests
{
    private static readonly CodeGenerationSettings Settings = new CodeGenerationSettings
    (
        basePath: Path.Combine(Directory.GetCurrentDirectory(), @"../../../../"),
        generateMultipleFiles: true,
        skipWhenFileExists: false,
        dryRun: true
    );

    // Bootstrap test that generates c# code for the model used in code generation :)
    [Fact]
    public void Can_Generate_Model_For_Abstractions()
    {
        // Act & Assert
        Verify(GenerateCode.For<TestInterfacesModels>(Settings));
        Verify(GenerateCode.For<ObjectsInterfacesModels>(Settings));
    }

    [Fact]
    public void Can_Generate_All_Classes_For_ModelFramework()
    {
        // Arrange
        var multipleContentBuilder = new MultipleContentBuilder(Settings.BasePath);

        // Act
        GenerateCode.For<CommonBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<CommonModels>(Settings, multipleContentBuilder);
        GenerateCode.For<CommonRecords>(Settings, multipleContentBuilder);

        GenerateCode.For<DatabaseBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<DatabaseModels>(Settings, multipleContentBuilder);
        GenerateCode.For<DatabaseRecords>(Settings, multipleContentBuilder);

        GenerateCode.For<ObjectsBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsModels>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsRecords>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsBaseBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsBaseModels>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsNonGenericBaseBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsNonGenericBaseModels>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsBaseRecords>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsOverrideBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsOverrideModels>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsOverrideRecords>(Settings, multipleContentBuilder);

        GenerateCode.For<ObjectsCodeStatementBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsCodeStatementModels>(Settings, multipleContentBuilder);
        GenerateCode.For<DatabaseCodeStatementBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<DatabaseCodeStatementModels>(Settings, multipleContentBuilder);

        // Assert
        Verify(multipleContentBuilder);
    }

    [Fact]
    public void Can_Generate_Test_Classes_For_ModelFramework_WithInheritance()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: Path.Combine(Directory.GetCurrentDirectory(), @"../../../../"),
            generateMultipleFiles: true,
            skipWhenFileExists: false,
            dryRun: true
        );
        var multipleContentBuilder = new MultipleContentBuilder(settings.BasePath);

        // Act
        GenerateCode.For<TestBuildersWithInheritance>(settings, multipleContentBuilder);
        GenerateCode.For<TestRecordsWithInheritance>(settings, multipleContentBuilder);

        // Assert
        Verify(multipleContentBuilder);
    }

    [Fact]
    public void Can_Generate_Test_Classes_For_ModelFramework_WithoutInheritance()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: Path.Combine(Directory.GetCurrentDirectory(), @"../../../../"),
            generateMultipleFiles: true,
            skipWhenFileExists: false,
            dryRun: true
        );
        var multipleContentBuilder = new MultipleContentBuilder(settings.BasePath);

        // Act
        GenerateCode.For<TestBuildersWithoutInheritance>(settings, multipleContentBuilder);
        GenerateCode.For<TestRecordsWithoutInheritance>(settings, multipleContentBuilder);

        // Assert
        Verify(multipleContentBuilder);
    }

    [Fact]
    public void Can_Generate_Test_Classes_For_ModelFramework_Without_PreGeneration_Of_Models()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: Path.Combine(Directory.GetCurrentDirectory(), @"../../../../"),
            generateMultipleFiles: true,
            skipWhenFileExists: false,
            dryRun: true
        );
        var multipleContentBuilder = new MultipleContentBuilder(settings.BasePath);

        // Act
        GenerateCode.For<TestBuildersWithoutInheritanceNoPregeneration>(settings, multipleContentBuilder);

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
            skipWhenFileExists: false,
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
            skipWhenFileExists: false,
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
    public partial record TestClass : TestClassBase
    {
        public TestClass(TestClass original) : base((TestClassBase)original)
        {
        }

        public TestClass(string testProperty) : base(testProperty)
        {
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }

    public partial record TestClassBase
    {
        public string TestProperty
        {
            get;
        }

        public TestClassBase(string testProperty)
        {
            this.TestProperty = testProperty;
        }
    }
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
            skipWhenFileExists: false,
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
    public partial class TestClassBuilder : System.ComponentModel.DataAnnotations.IValidatableObject
    {
        public System.Text.StringBuilder TestProperty
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
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new Test.TestClass(TestProperty?.ToString());
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public System.Collections.Generic.IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            var instance = new Test.TestClassBase(TestProperty?.ToString());
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
            var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();
            System.ComponentModel.DataAnnotations.Validator.TryValidateObject(instance, new System.ComponentModel.DataAnnotations.ValidationContext(instance), results, true);
            return results;
        }

        public TestClassBuilder WithTestProperty(System.Text.StringBuilder testProperty)
        {
            TestProperty = testProperty;
            return this;
        }

        public TestClassBuilder WithTestProperty(System.Func<System.Text.StringBuilder> testPropertyDelegate)
        {
            _testPropertyDelegate = new (testPropertyDelegate);
            return this;
        }

        public TestClassBuilder WithTestProperty(string value)
        {
            if (TestProperty == null)
                TestProperty = new System.Text.StringBuilder();
            TestProperty.Clear().Append(value);
            return this;
        }

        public TestClassBuilder AppendToTestProperty(string value)
        {
            if (TestProperty == null)
                TestProperty = new System.Text.StringBuilder();
            TestProperty.Append(value);
            return this;
        }

        public TestClassBuilder AppendLineToTestProperty(string value)
        {
            if (TestProperty == null)
                TestProperty = new System.Text.StringBuilder();
            TestProperty.AppendLine(value);
            return this;
        }

        public TestClassBuilder()
        {
            _testPropertyDelegate = new (() => new System.Text.StringBuilder());
        }

        public TestClassBuilder(Test.TestClass source)
        {
            _testPropertyDelegate = new (() => new System.Text.StringBuilder(source.TestProperty));
        }

        protected System.Lazy<System.Text.StringBuilder> _testPropertyDelegate;
    }
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
            skipWhenFileExists: false,
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
            skipWhenFileExists: false,
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
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new Test.TestClass(TestProperty?.Build()!);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public TestClassBuilder WithTestProperty(TestClassBuilder testProperty)
        {
            TestProperty = testProperty;
            return this;
        }

        public TestClassBuilder WithTestProperty(System.Func<TestClassBuilder> testPropertyDelegate)
        {
            _testPropertyDelegate = new (testPropertyDelegate);
            return this;
        }

        public TestClassBuilder()
        {
            _testPropertyDelegate = new (() => default(TestClassBuilder)!);
        }

        public TestClassBuilder(Test.TestClass source)
        {
            TestProperty = new TestClassBuilder(source.TestProperty);
        }

        protected System.Lazy<TestClassBuilder> _testPropertyDelegate;
    }
}
");
    }

    [Fact]
    public void Can_Generate_Generics_Immutable_Class()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<GenericsRecords>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    public partial record TestClass<T>
    {
        public T TestProperty
        {
            get;
        }

        public TestClass(T testProperty)
        {
            this.TestProperty = testProperty;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_Generics_Immutable_ClassBuilder()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<GenericsBuilders>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Builders
{
    public partial class TestClassBuilder<T>
    {
        public T TestProperty
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

        public Test.TestClass<T> Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new Test.TestClass<T>(TestProperty);
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public TestClassBuilder<T> WithTestProperty(T testProperty)
        {
            TestProperty = testProperty;
            return this;
        }

        public TestClassBuilder<T> WithTestProperty(System.Func<T> testPropertyDelegate)
        {
            _testPropertyDelegate = new (testPropertyDelegate);
            return this;
        }

        public TestClassBuilder()
        {
            _testPropertyDelegate = new (() => default(T)!);
        }

        public TestClassBuilder(Test.TestClass<T> source)
        {
            _testPropertyDelegate = new (() => source.TestProperty);
        }

        protected System.Lazy<T> _testPropertyDelegate;
    }
}
");
    }

    [Fact]
    public void Can_Generate_GenericArgument_Immutable_ClassBuilder()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<GenericArgumentBuilders>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test.Builders
{
    public partial class TestClassBuilder<T>
    {
        public ModelFramework.Builders.MyGenericTypeBuilder<T> TestProperty
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

        public Test.TestClass<T> Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new Test.TestClass<T>(TestProperty.BuildTyped());
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public TestClassBuilder<T> WithTestProperty(ModelFramework.Builders.MyGenericTypeBuilder<T> testProperty)
        {
            TestProperty = testProperty;
            return this;
        }

        public TestClassBuilder<T> WithTestProperty(System.Func<ModelFramework.Builders.MyGenericTypeBuilder<T>> testPropertyDelegate)
        {
            _testPropertyDelegate = new (testPropertyDelegate);
            return this;
        }

        public TestClassBuilder()
        {
            _testPropertyDelegate = new (() => new ModelFramework.Builders.Domain.MyGenericTypeBuilder<T>());
        }

        public TestClassBuilder(Test.TestClass<T> source)
        {
            _testPropertyDelegate = new (() => new ModelFramework.Builders.Domain.MyGenericTypeBuilder<T>(source.TestProperty));
        }

        protected System.Lazy<ModelFramework.Builders.MyGenericTypeBuilder<T>> _testPropertyDelegate;
    }
}
");
    }

    [Fact]
    public void NewCollectionTypeName_Is_Set_To_GenericList()
    {
        // Arrange
        var sut = new TestBuildersWithInheritance();

        // Act
        var actual = sut.GetNewCollectionTypeName();

        // Assert
        actual.Should().Be("System.Collections.Generic.List");
    }

    [Fact]
    public void Can_Generate_Core_Builder_Using_ModelTransformation_And_Automatic_Builder_Properties()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<TestCSharpClassBaseModelTransformationCoreBuilders>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace.Domain.Builders
{
    public partial class MyClassBuilder
    {
        public System.Collections.Generic.List<MyNamespace.Domain.Builders.MyClassBuilder> SubTypes
        {
            get;
            set;
        }

        public MyNamespace.Domain.Builders.MyClassBuilder? ParentType
        {
            get
            {
                return _parentTypeDelegate.Value;
            }
            set
            {
                _parentTypeDelegate = new (() => value);
            }
        }

        public MyNamespace.Domain.MyClass Build()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new MyNamespace.Domain.MyClass(SubTypes.Select(x => x.Build()), ParentType?.Build());
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public MyClassBuilder AddSubTypes(System.Collections.Generic.IEnumerable<MyNamespace.Domain.Builders.MyClassBuilder> subTypes)
        {
            return AddSubTypes(subTypes.ToArray());
        }

        public MyClassBuilder AddSubTypes(params MyNamespace.Domain.Builders.MyClassBuilder[] subTypes)
        {
            SubTypes.AddRange(subTypes);
            return this;
        }

        public MyClassBuilder WithParentType(MyNamespace.Domain.Builders.MyClassBuilder? parentType)
        {
            ParentType = parentType;
            return this;
        }

        public MyClassBuilder WithParentType(System.Func<MyNamespace.Domain.Builders.MyClassBuilder?> parentTypeDelegate)
        {
            _parentTypeDelegate = new (parentTypeDelegate);
            return this;
        }

        public MyClassBuilder()
        {
            SubTypes = new System.Collections.Generic.List<MyNamespace.Domain.Builders.MyClassBuilder>();
            _parentTypeDelegate = new (() => default(MyNamespace.Domain.Builders.MyClassBuilder?));
        }

        public MyClassBuilder(MyNamespace.Domain.MyClass source)
        {
            SubTypes = new System.Collections.Generic.List<MyNamespace.Domain.Builders.MyClassBuilder>();
            SubTypes.AddRange(source.SubTypes.Select(x => new MyNamespace.Domain.Builders.MyClassBuilder(x)));
            _parentTypeDelegate = new (() => source.ParentType == null ? null : new MyNamespace.Domain.Builders.MyClassBuilder(source.ParentType));
        }

        protected System.Lazy<MyNamespace.Domain.Builders.MyClassBuilder?> _parentTypeDelegate;
    }
}
");
    }

    [Fact]
    public void Can_Generate_Core_Record_Using_ModelTransformation_And_Automatic_Builder_Properties()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<TestCSharpClassBaseModelTransformationCoreRecords>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace.Domain
{
    public partial record MyClass
    {
        public System.Collections.Generic.IReadOnlyCollection<MyNamespace.Domain.MyClass> SubTypes
        {
            get;
        }

        public MyNamespace.Domain.MyClass? ParentType
        {
            get;
        }

        public MyClass(System.Collections.Generic.IEnumerable<MyNamespace.Domain.MyClass> subTypes, MyNamespace.Domain.MyClass? parentType)
        {
            this.SubTypes = new System.Collections.ObjectModel.ReadOnlyCollection<MyNamespace.Domain.MyClass>(subTypes);
            this.ParentType = parentType;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_Core_Interface_Using_ModelTransformation_And_Automatic_Builder_Properties()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<TestCSharpClassBaseModelTransformationCoreInterfaces>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotUsed
{
    public interface IMyClass
    {
        System.Collections.Generic.IReadOnlyCollection<MyNamespace.Domain.MyClass> SubTypes
        {
            get;
        }

        MyNamespace.Domain.MyClass? ParentType
        {
            get;
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_Core_Builder_Interface_Using_ModelTransformation_And_Automatic_Builder_Properties()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<TestCSharpClassBaseModelTransformationCoreBuilderInterfaces>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NotUsed
{
    public interface IMyClassBuilder
    {
        System.Collections.Generic.List<MyNamespace.Domain.Builders.MyClassBuilder> SubTypes
        {
            get;
            set;
        }

        MyNamespace.Domain.Builders.MyClassBuilder? ParentType
        {
            get;
            set;
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_Base_Builder_Using_ModelTransformation_And_Automatic_Builder_Properties()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<TestCSharpClassBaseModelTransformationBaseBuilders>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace.Domain.Builders
{
    public abstract partial class MyBaseClassBuilder<TBuilder, TEntity> : MyBaseClassBuilder
        where TEntity : MyNamespace.Domain.MyBaseClass
        where TBuilder : MyBaseClassBuilder<TBuilder, TEntity>
    {
        public abstract TEntity BuildTyped();

        public override MyNamespace.Domain.MyBaseClass Build()
        {
            return BuildTyped();
        }

        public TBuilder WithBaseProperty(System.Text.StringBuilder baseProperty)
        {
            BaseProperty = baseProperty;
            return (TBuilder)this;
        }

        public TBuilder WithBaseProperty(System.Func<System.Text.StringBuilder> basePropertyDelegate)
        {
            _basePropertyDelegate = new (basePropertyDelegate);
            return (TBuilder)this;
        }

        public TBuilder WithBaseProperty(string value)
        {
            if (BaseProperty == null)
                BaseProperty = new System.Text.StringBuilder();
            BaseProperty.Clear().Append(value);
            return (TBuilder)this;
        }

        public TBuilder AppendToBaseProperty(string value)
        {
            if (BaseProperty == null)
                BaseProperty = new System.Text.StringBuilder();
            BaseProperty.Append(value);
            return (TBuilder)this;
        }

        public TBuilder AppendLineToBaseProperty(string value)
        {
            if (BaseProperty == null)
                BaseProperty = new System.Text.StringBuilder();
            BaseProperty.AppendLine(value);
            return (TBuilder)this;
        }

        public TBuilder AddChildren(System.Collections.Generic.IEnumerable<MyNamespace.Domain.Builders.MyBaseClassBuilder> children)
        {
            return AddChildren(children.ToArray());
        }

        public TBuilder AddChildren(params MyNamespace.Domain.Builders.MyBaseClassBuilder[] children)
        {
            Children.AddRange(children);
            return (TBuilder)this;
        }

        protected MyBaseClassBuilder() : base()
        {
        }

        protected MyBaseClassBuilder(MyNamespace.Domain.MyBaseClass source) : base(source)
        {
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_NonGeneric_Base_Builder_Using_ModelTransformation_And_Automatic_Builder_Properties()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<TestCSharpClassBaseModelTransformationNonGenericBaseBuilders>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace.Domain.Builders
{
    public abstract partial class MyBaseClassBuilder
    {
        public System.Text.StringBuilder BaseProperty
        {
            get
            {
                return _basePropertyDelegate.Value;
            }
            set
            {
                _basePropertyDelegate = new (() => value);
            }
        }

        public System.Collections.Generic.List<MyNamespace.Domain.Builders.MyBaseClassBuilder> Children
        {
            get;
            set;
        }

        public abstract MyNamespace.Domain.MyBaseClass Build();

        protected MyBaseClassBuilder()
        {
            Children = new System.Collections.Generic.List<MyNamespace.Domain.Builders.MyBaseClassBuilder>();
            _basePropertyDelegate = new (() => new System.Text.StringBuilder());
        }

        protected MyBaseClassBuilder(MyNamespace.Domain.MyBaseClass source)
        {
            Children = new System.Collections.Generic.List<MyNamespace.Domain.Builders.MyBaseClassBuilder>();
            _basePropertyDelegate = new (() => new System.Text.StringBuilder(source.BaseProperty));
            Children = source.Children.Select(x => MyNamespace.Domain.Builders.MyBaseClassBuilderFactory.Create(x)).ToList();
        }

        protected System.Lazy<System.Text.StringBuilder> _basePropertyDelegate;
    }
}
");
    }

    [Fact]
    public void Can_Generate_Base_Record_Using_ModelTransformation_And_Automatic_Builder_Properties()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<TestCSharpClassBaseModelTransformationBaseRecords>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace.Domain
{
    public abstract partial record MyBaseClass
    {
        public string BaseProperty
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<MyNamespace.Domain.MyBaseClass> Children
        {
            get;
        }

        protected MyBaseClass(string baseProperty, System.Collections.Generic.IEnumerable<MyNamespace.Domain.MyBaseClass> children)
        {
            this.BaseProperty = baseProperty;
            this.Children = new System.Collections.ObjectModel.ReadOnlyCollection<MyNamespace.Domain.MyBaseClass>(children);
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_Override_Builder_Using_ModelTransformation_And_Automatic_Builder_Properties()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<TestCSharpClassBaseModelTransformationOverrideBuilders>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace.Domain.Builders
{
    public partial class MyDerivedClassBuilder : MyBaseClassBuilder<MyDerivedClassBuilder, MyNamespace.Domain.MyDerivedClass>
    {
        public MyNamespace.Domain.Builders.MyClassBuilder RequiredDomainProperty
        {
            get
            {
                return _requiredDomainPropertyDelegate.Value;
            }
            set
            {
                _requiredDomainPropertyDelegate = new (() => value);
            }
        }

        public override MyNamespace.Domain.MyDerivedClass BuildTyped()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            return new MyNamespace.Domain.MyDerivedClass(RequiredDomainProperty?.Build()!, BaseProperty?.ToString(), Children.Select(x => x.Build()));
            #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public MyDerivedClassBuilder WithRequiredDomainProperty(MyNamespace.Domain.Builders.MyClassBuilder requiredDomainProperty)
        {
            RequiredDomainProperty = requiredDomainProperty;
            return this;
        }

        public MyDerivedClassBuilder WithRequiredDomainProperty(System.Func<MyNamespace.Domain.Builders.MyClassBuilder> requiredDomainPropertyDelegate)
        {
            _requiredDomainPropertyDelegate = new (requiredDomainPropertyDelegate);
            return this;
        }

        public MyDerivedClassBuilder() : base()
        {
            _requiredDomainPropertyDelegate = new (() => new MyNamespace.Domain.Builders.MyClassBuilder());
        }

        public MyDerivedClassBuilder(MyNamespace.Domain.MyDerivedClass source) : base(source)
        {
            _requiredDomainPropertyDelegate = new (() => new MyNamespace.Domain.Builders.MyClassBuilder(source.RequiredDomainProperty));
        }

        protected System.Lazy<MyNamespace.Domain.Builders.MyClassBuilder> _requiredDomainPropertyDelegate;
    }
}
");
    }

    [Fact]
    public void Can_Generate_Override_Record_Using_ModelTransformation_And_Automatic_Builder_Properties()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<TestCSharpClassBaseModelTransformationOverrideRecords>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace.Domain
{
    public partial record MyDerivedClass : MyNamespace.Domain.MyBaseClass
    {
        public MyNamespace.Domain.MyClass RequiredDomainProperty
        {
            get;
        }

        public MyDerivedClass(MyNamespace.Domain.MyClass requiredDomainProperty, string baseProperty, System.Collections.Generic.IEnumerable<MyNamespace.Domain.MyBaseClass> children) : base(baseProperty, children)
        {
            this.RequiredDomainProperty = requiredDomainProperty;
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_ServiceCollectionExtensions_For_OverrideTypes()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<TestCSharpClassBaseModelTransformationOverrideServiceCollectionExtension>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace.Domain.Extensions
{
    public static partial class ServiceCollectionExtensions
    {
        private static Microsoft.Extensions.DependencyInjection.IServiceCollection AddHandlers(this Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection)
        {
            return serviceCollection
            .AddSingleton<IHandler, MyDerivedClassHandler>()
            ;
        }
    }
}
");
    }

    [Fact]
    public void Can_Generate_BuilderFactory_For_Inherited_Builders()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<TestCSharpClassBaseModelTransformationOverrideBuilderFactory>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace.Domain.Builders
{
    public static partial class MyClassBuilderFactory
    {
        public static MyNamespace.Domain.Builders.MyClassBuilder Create(MyNamespace.Domain.MyClass instance)
        {
            return registeredTypes.ContainsKey(instance.GetType()) ? registeredTypes[instance.GetType()].Invoke(instance) : throw new System.ArgumentOutOfRangeException(""Unknown instance type: "" + instance.GetType().FullName);
        }

        public static void Register(System.Type type, Func<MyNamespace.Domain.MyClass,MyClassBuilder> createDelegate)
        {
            registeredTypes.Add(type, createDelegate);
        }

        private static Dictionary<System.Type,Func<MyNamespace.Domain.MyClass,MyClassBuilder>> registeredTypes = new Dictionary<System.Type, Func<MyNamespace.Domain.MyClass, MyClassBuilder>>
        {
            { typeof(MyNamespace.Domain.MyDerivedClass),x => new MyNamespace.Domain.Builders.MyClass.MyDerivedClassBuilder((MyNamespace.Domain.MyDerivedClass)x) },
        };
    }
}
");
    }

    [Fact]
    public void Can_Generate_BuilderFactory_For_Inherited_Builders_With_Custom_Code()
    {
        // Arrange
        var settings = new CodeGenerationSettings
        (
            basePath: @"C:\Temp\ModelFramework",
            generateMultipleFiles: false,
            skipWhenFileExists: false,
            dryRun: true
        );

        // Act
        var generatedCode = GenerateCode.For<TestCSharpClassBaseModelTransformationOverrideBuilderFactoryCustomCode>(settings);
        var actual = generatedCode.TemplateFileManager.MultipleContentBuilder.Contents.First().Builder.ToString();

        // Assert
        actual.NormalizeLineEndings().Should().Be(@"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyNamespace.Domain.Builders
{
    public static partial class MyClassBuilderFactory
    {
        public static MyNamespace.Domain.Builders.MyClassBuilder Create(MyNamespace.Domain.MyClass instance)
        {
            // custom code goes here
            return registeredTypes.ContainsKey(instance.GetType()) ? registeredTypes[instance.GetType()].Invoke(instance) : throw new System.ArgumentOutOfRangeException(""Unknown instance type: "" + instance.GetType().FullName);
        }

        public static void Register(System.Type type, Func<MyNamespace.Domain.MyClass,MyClassBuilder> createDelegate)
        {
            registeredTypes.Add(type, createDelegate);
        }

        private static Dictionary<System.Type,Func<MyNamespace.Domain.MyClass,MyClassBuilder>> registeredTypes = new Dictionary<System.Type, Func<MyNamespace.Domain.MyClass, MyClassBuilder>>
        {
            { typeof(MyNamespace.Domain.MyDerivedClass),x => new MyNamespace.Domain.Builders.MyClass.MyDerivedClassBuilder((MyNamespace.Domain.MyDerivedClass)x) },
        };
    }
}
");
    }

    [Fact]
    public void GetModelMappings_Returns_Correct_Result()
    {
        // Arrange
        var sut = new TestCSharpClassBaseModelTransformationCoreRecords();

        // Act
        var actual = sut.GetModelMappingsEx();

        // Assert
        actual.Should().BeEquivalentTo(new[]
        {
            new KeyValuePair<string, string>("MyProject.CodeGeneration.Models.I", "MyNamespace."),
            new KeyValuePair<string, string>("MyProject.CodeGeneration.Models.Domains.", "MyNamespace.Domains."),
            new KeyValuePair<string, string>("MyProject.CodeGeneration.Models.Contracts.", "MyNamespace.Contracts."),
            new KeyValuePair<string, string>("MyProject.CodeGeneration.Models.Abstractions.", "MyNamespace.Abstractions."),
            new KeyValuePair<string, string>("MyProject.CodeGeneration.I", "MyNamespace.I"),
        });
    }

    [Fact]
    public void GetCoreModels_Returns_Correct_Result_Without_Using_Models()
    {
        // Arrange
        var sut = new TestCSharpClassBaseModelTransformationCoreRecords();

        // Act
        var actual = sut.GetCoreModelsEx();

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void GetAbstractModels_Returns_Correct_Result_Without_Using_Models()
    {
        // Arrange
        var sut = new TestCSharpClassBaseModelTransformationCoreRecords();

        // Act
        var actual = sut.GetAbstractModelsEx();

        // Assert
        actual.Should().BeEmpty();
    }

    [Fact]
    public void GetOverrideModels_Returns_Correct_Result_Without_Using_Models()
    {
        // Arrange
        var sut = new TestCSharpClassBaseModelTransformationCoreRecords();

        // Act
        var actual = sut.GetOverrideModelsEx(typeof(int));

        // Assert
        actual.Should().BeEmpty();
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
        protected override string RootNamespace => "ModelFramework";
        protected override string ProjectName => "ModelFramework";

        protected override string GetFullBasePath() => string.Empty;

        protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
        protected override Type RecordConcreteCollectionType => typeof(ReadOnlyCollection<>);
        protected override bool EnableNullableContext => true;
        protected override bool CreateCodeGenerationHeader => false;
        protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.Shared;

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

    private abstract class CustomPropertiesBase : CSharpClassBase
    {
        public override string Path => @"C:\Temp";
        public override string DefaultFileName => "GeneratedCode.cs";
        public override bool RecurseOnDeleteGeneratedFiles => false;
        protected override string RootNamespace => "ModelFramework";
        protected override string ProjectName => "MyProject";

        protected override string GetFullBasePath() => string.Empty;

        protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
        protected override Type RecordConcreteCollectionType => typeof(ReadOnlyCollection<>);
        protected override bool EnableNullableContext => true;
        protected override bool CreateCodeGenerationHeader => false;

        protected override void FixImmutableClassProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
            => FixStuff(typeBaseBuilder, false);

        protected override void FixImmutableBuilderProperties<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
            => FixStuff(typeBaseBuilder, true);

        private static void FixStuff<TBuilder, TEntity>(TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder, bool forBuilder)
            where TEntity : ITypeBase
            where TBuilder : TypeBaseBuilder<TBuilder, TEntity>
        {
            if (typeBaseBuilder == null)
            {
                // Not possible, but needs to be added because of .net standard 2.0
                return;
            }

            foreach (var property in typeBaseBuilder.Properties)
            {
                FixImmutableBuilderProperty(property, forBuilder);
            }
        }

        private static void FixImmutableBuilderProperty(ClassPropertyBuilder property, bool forBuilder)
        {
            var typeName = property.TypeName.ToString();
            if (typeName.StartsWith("Test.Contracts.", StringComparison.InvariantCulture))
            {
                if (forBuilder)
                {
                    property.ConvertSinglePropertyToBuilderOnBuilder(typeName.Replace("Test.Contracts.", string.Empty) + "Builder");
                }
                else
                {
                    property.WithTypeName(typeName.Replace("Test.Contracts.", string.Empty));
                }
            }
            else if (typeName.Contains("Collection<Test.Contracts.", StringComparison.InvariantCulture))
            {
                if (forBuilder)
                {
                    property.ConvertCollectionPropertyToBuilderOnBuilder
                    (
                        addNullChecks: false,
                        argumentType: typeName.Replace("Test.Contracts.", string.Empty, StringComparison.InvariantCulture).ReplaceSuffix(">", "Builder>", StringComparison.InvariantCulture)
                    );
                }
                else
                {
                    property.WithTypeName(typeName.Replace("Test.Contracts.", string.Empty, StringComparison.InvariantCulture));
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

    private abstract class GenericsBase : CSharpClassBase
    {
        public override string Path => @"C:\Temp";
        public override string DefaultFileName => "GeneratedCode.cs";
        public override bool RecurseOnDeleteGeneratedFiles => false;
        protected override string RootNamespace => "ModelFramework";
        protected override string ProjectName => "ModelFramework";

        protected override string GetFullBasePath() => string.Empty;

        protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
        protected override Type RecordConcreteCollectionType => typeof(ReadOnlyCollection<>);
        protected override bool EnableNullableContext => true;
        protected override bool CreateCodeGenerationHeader => false;

        protected ITypeBase[] GetModels() => new[]
        {
            new ClassBuilder()
                .WithName("TestClass")
                .WithNamespace("Test")
                .AddGenericTypeArguments("T")
                .AddProperties(new ClassPropertyBuilder().WithName("TestProperty").WithTypeName("T"))
                .Build()
        };
    }

    private sealed class GenericsRecords : GenericsBase
    {
        public override object CreateModel() => GetImmutableClasses(GetModels(), "Test");
    }

    private sealed class GenericsBuilders : GenericsBase
    {
        public override object CreateModel() => GetImmutableBuilderClasses(GetModels(), "Test", "Test.Builders");
    }

    private abstract class GenericArgumentBase : CSharpClassBase
    {
        public override string Path => @"C:\Temp";
        public override string DefaultFileName => "GeneratedCode.cs";
        public override bool RecurseOnDeleteGeneratedFiles => false;
        protected override string RootNamespace => "ModelFramework";
        protected override string ProjectName => "ModelFramework";

        protected override string GetFullBasePath() => string.Empty;

        protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
        protected override Type RecordConcreteCollectionType => typeof(ReadOnlyCollection<>);
        protected override bool EnableNullableContext => true;
        protected override bool CreateCodeGenerationHeader => false;

        protected ITypeBase[] GetModels() => new[]
        {
            new ClassBuilder()
                .WithName("TestClass")
                .WithNamespace("ModelFramework.Domain")
                .AddGenericTypeArguments("T")
                .AddProperties(new ClassPropertyBuilder().WithName("TestProperty").WithTypeName("ModelFramework.Domain.MyGenericType<T>"))
                .Build()
        };
    }

    ///private sealed class GenericArgumentRecords : GenericArgumentBase
    ///{
    ///    public override object CreateModel() => GetImmutableClasses(GetModels(), "Test");
    ///}

    private sealed class GenericArgumentBuilders : GenericArgumentBase
    {
        public override object CreateModel() => GetImmutableBuilderClasses(GetModels(), "Test", "Test.Builders");
    }
}
