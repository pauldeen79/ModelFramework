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
        GenerateCode.For<CommonRecords>(Settings, multipleContentBuilder);

        GenerateCode.For<DatabaseBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<DatabaseRecords>(Settings, multipleContentBuilder);

        GenerateCode.For<ObjectsBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsRecords>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsBaseBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsNonGenericBaseBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsBaseRecords>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsOverrideBuilders>(Settings, multipleContentBuilder);
        GenerateCode.For<ObjectsOverrideRecords>(Settings, multipleContentBuilder);

        GenerateCode.For<ObjectsCodeStatements>(Settings, multipleContentBuilder);
        GenerateCode.For<DatabaseCodeStatements>(Settings, multipleContentBuilder);

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

        public TestClassBuilder WithTestProperty(string testProperty)
        {
            TestProperty = testProperty;
            return this;
        }

        public TestClassBuilder WithTestProperty(System.Func<string> testPropertyDelegate)
        {
            _testPropertyDelegate = new (testPropertyDelegate);
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

        protected System.Lazy<string> _testPropertyDelegate;
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
            #pragma warning disable CS8603 // Possible null reference return.
            _testPropertyDelegate = new (() => default);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public TestClassBuilder(Test.TestClass source)
        {
            TestProperty = new TestClassBuilder(source.TestProperty);
        }

        protected System.Lazy<TestClassBuilder> _testPropertyDelegate;
    }
#nullable restore
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
#nullable enable
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
            return new MyNamespace.Domain.MyClass(SubTypes.Select(x => x.Build()), ParentType?.Build());
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
            #pragma warning disable CS8603 // Possible null reference return.
            _parentTypeDelegate = new (() => default);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public MyClassBuilder(MyNamespace.Domain.MyClass source)
        {
            SubTypes = new System.Collections.Generic.List<MyNamespace.Domain.Builders.MyClassBuilder>();
            SubTypes.AddRange(source.SubTypes.Select(x => new MyNamespace.Domain.Builders.MyClassBuilder(x)));
            _parentTypeDelegate = new (() => source.ParentType == null ? null : new MyNamespace.Domain.Builders.MyClassBuilder(source.ParentType));
        }

        protected System.Lazy<MyNamespace.Domain.Builders.MyClassBuilder?> _parentTypeDelegate;
    }
#nullable restore
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
#nullable enable
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
#nullable restore
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
#nullable enable
    public abstract partial class MyBaseClassBuilder<TBuilder, TEntity> : MyBaseClassBuilder
        where TEntity : MyNamespace.Domain.MyBaseClass
        where TBuilder : MyBaseClassBuilder<TBuilder, TEntity>
    {
        public abstract TEntity BuildTyped();

        public override MyNamespace.Domain.MyBaseClass Build()
        {
            return BuildTyped();
        }

        public TBuilder WithBaseProperty(string baseProperty)
        {
            BaseProperty = baseProperty;
            return (TBuilder)this;
        }

        public TBuilder WithBaseProperty(System.Func<string> basePropertyDelegate)
        {
            _basePropertyDelegate = new (basePropertyDelegate);
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
#nullable restore
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
#nullable enable
    public abstract partial class MyBaseClassBuilder
    {
        public string BaseProperty
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
            #pragma warning disable CS8603 // Possible null reference return.
            _basePropertyDelegate = new (() => string.Empty);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        protected MyBaseClassBuilder(MyNamespace.Domain.MyBaseClass source)
        {
            Children = new System.Collections.Generic.List<MyNamespace.Domain.Builders.MyBaseClassBuilder>();
            _basePropertyDelegate = new (() => source.BaseProperty);
            Children = source.Children.Select(x => MyNamespace.Domain.Builders.MyBaseClassBuilderFactory.Create(x)).ToList();
        }

        protected System.Lazy<string> _basePropertyDelegate;
    }
#nullable restore
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
#nullable enable
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
#nullable restore
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
#nullable enable
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

        public string BaseProperty
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

        public override MyNamespace.Domain.MyDerivedClass BuildTyped()
        {
            #pragma warning disable CS8604 // Possible null reference argument.
            return new MyNamespace.Domain.MyDerivedClass(RequiredDomainProperty?.Build(), BaseProperty, Children.Select(x => x.Build()));
            #pragma warning restore CS8604 // Possible null reference argument.
        }

        public override MyNamespace.Domain.MyBaseClass Build()
        {
            return BuildTyped();
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

        public MyDerivedClassBuilder WithBaseProperty(string baseProperty)
        {
            BaseProperty = baseProperty;
            return this;
        }

        public MyDerivedClassBuilder WithBaseProperty(System.Func<string> basePropertyDelegate)
        {
            _basePropertyDelegate = new (basePropertyDelegate);
            return this;
        }

        public MyDerivedClassBuilder AddChildren(System.Collections.Generic.IEnumerable<MyNamespace.Domain.Builders.MyBaseClassBuilder> children)
        {
            return AddChildren(children.ToArray());
        }

        public MyDerivedClassBuilder AddChildren(params MyNamespace.Domain.Builders.MyBaseClassBuilder[] children)
        {
            Children.AddRange(children);
            return this;
        }

        public MyDerivedClassBuilder WithBaseProperty(string baseProperty)
        {
            BaseProperty = baseProperty;
            return this;
        }

        public MyDerivedClassBuilder WithBaseProperty(System.Func<string> basePropertyDelegate)
        {
            _basePropertyDelegate = new (basePropertyDelegate);
            return this;
        }

        public MyDerivedClassBuilder AddChildren(System.Collections.Generic.IEnumerable<ModelFramework.CodeGeneration.Tests.CodeGenerationProviders.IMyBaseClass> children)
        {
            return AddChildren(children.ToArray());
        }

        public MyDerivedClassBuilder AddChildren(params ModelFramework.CodeGeneration.Tests.CodeGenerationProviders.IMyBaseClass[] children)
        {
            Children.AddRange(children);
            return this;
        }

        public MyDerivedClassBuilder() : base()
        {
            Children = new System.Collections.Generic.List<MyNamespace.Domain.Builders.MyBaseClassBuilder>();
            #pragma warning disable CS8603 // Possible null reference return.
            _requiredDomainPropertyDelegate = new (() => new MyNamespace.Domain.Builders.MyClassBuilder());
            _basePropertyDelegate = new (() => string.Empty);
            #pragma warning restore CS8603 // Possible null reference return.
        }

        public MyDerivedClassBuilder(MyNamespace.Domain.MyDerivedClass source) : base(source)
        {
            Children = new System.Collections.Generic.List<MyNamespace.Domain.Builders.MyBaseClassBuilder>();
            _requiredDomainPropertyDelegate = new (() => new MyNamespace.Domain.Builders.MyClassBuilder(source.RequiredDomainProperty));
            _basePropertyDelegate = new (() => source.BaseProperty);
            Children = source.Children.Select(x => MyNamespace.Domain.Builders.MyBaseClassBuilderFactory.Create(x)).ToList();
        }

        protected System.Lazy<MyNamespace.Domain.Builders.MyClassBuilder> _requiredDomainPropertyDelegate;

        protected System.Lazy<string> _basePropertyDelegate;
    }
#nullable restore
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
#nullable enable
    public partial record MyDerivedClass : MyNamespace.Domain.MyBaseClass
    {
        public MyNamespace.Domain.MyClass RequiredDomainProperty
        {
            get;
        }

        public string BaseProperty
        {
            get;
        }

        public System.Collections.Generic.IReadOnlyCollection<MyNamespace.Domain.MyBaseClass> Children
        {
            get;
        }

        public MyDerivedClass(MyNamespace.Domain.MyClass requiredDomainProperty, string baseProperty, System.Collections.Generic.IEnumerable<MyNamespace.Domain.MyBaseClass> children) : base(baseProperty, children)
        {
            this.RequiredDomainProperty = requiredDomainProperty;
            this.BaseProperty = baseProperty;
            this.Children = new System.Collections.ObjectModel.ReadOnlyCollection<MyNamespace.Domain.MyBaseClass>(children);
            System.ComponentModel.DataAnnotations.Validator.ValidateObject(this, new System.ComponentModel.DataAnnotations.ValidationContext(this, null, null), true);
        }
    }
#nullable restore
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
#nullable enable
    public static partial class ServiceCollectionExtensions
    {
        private static Microsoft.Extensions.DependencyInjection.IServiceCollection AddHandlers(this Microsoft.Extensions.DependencyInjection.IServiceCollection serviceCollection)
        {
            return serviceCollection
            .AddSingleton<IHandler, MyDerivedClassHandler>()
            ;
        }
    }
#nullable restore
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
#nullable enable
    public static class MyClassBuilderFactory
    {
        public static MyNamespace.Domain.Builders.MyClassBuilder Create(MyNamespace.Domain.MyClass instance)
        {
            return registeredTypes.ContainsKey(instance.GetType()) ? registeredTypes[instance.GetType()].Invoke(instance) : throw new ArgumentOutOfRangeException(""Unknown instance type: "" + instance.GetType().FullName);
        }

        public static void Register(System.Type type, Func<MyNamespace.Domain.MyClass,MyClassBuilder> createDelegate)
        {
            registeredTypes.Add(type, createDelegate);
        }

        private static Dictionary<Type,Func<MyNamespace.Domain.MyClass,MyClassBuilder>> registeredTypes = new Dictionary<Type, Func<MyNamespace.Domain.MyClass, MyClassBuilder>>
{
    {typeof(MyNamespace.MyDerivedClass),x => new MyNamespace.Domain.Builders.MyClasss.MyDerivedClassBuilder((MyNamespace.MyDerivedClass)x)},
}
;
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
        protected override string RootNamespace => "ModelFramework";

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
            var typeName = property.TypeName;
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
