namespace ClassFramework.Pipelines.Tests.Extensions;

public class ClassPropertyExtensionsTests : TestBase
{
    public class GetDefaultValue : ClassPropertyExtensionsTests
    {
        [Fact]
        public void Gets_Value_From_TypeName_When_Metadata_Is_Not_Found()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
            var csharpExpressionCreator = Fixture.Freeze<ICsharpExpressionCreator>();

            // Act
            var result = sut.GetDefaultValue(csharpExpressionCreator, false, sut.TypeName);

            // Assert
            result.Should().Be("default(System.String)");
        }

        [Fact]
        public void Gets_Value_From_TypeName_When_Metadata_Is_Found_But_Value_Is_Null()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().AddMetadata(MetadataNames.CustomBuilderDefaultValue, null).Build();
            var csharpExpressionCreator = Fixture.Freeze<ICsharpExpressionCreator>();

            // Act
            var result = sut.GetDefaultValue(csharpExpressionCreator, false, sut.TypeName);

            // Assert
            result.Should().Be("default(System.String)");
        }

        [Fact]
        public void Gets_Value_From_MetadataValue_Literal_When_Found()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().AddMetadata(MetadataNames.CustomBuilderDefaultValue, new Literal("custom value")).Build();
            var csharpExpressionCreator = Fixture.Freeze<ICsharpExpressionCreator>();

            // Act
            var result = sut.GetDefaultValue(csharpExpressionCreator, false, sut.TypeName);

            // Assert
            result.Should().Be("custom value");
        }

        [Fact]
        public void Gets_Value_From_MetadataValue_Non_Literal_When_Found()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().AddMetadata(MetadataNames.CustomBuilderDefaultValue, "custom value").Build();
            var csharpExpressionCreator = Fixture.Freeze<ICsharpExpressionCreator>();
            csharpExpressionCreator.Create(Arg.Any<object?>()).Returns(x => x.ArgAt<object?>(0).ToStringWithNullCheck());

            // Act
            var result = sut.GetDefaultValue(csharpExpressionCreator, false, sut.TypeName);

            // Assert
            result.Should().Be("custom value");
        }
    }

    public class GetNullCheckSuffix
    {
        [Fact]
        public void Returns_Empty_String_When_AddNullChecks_Is_False()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).Build();

            // Act
            var result = sut.GetNullCheckSuffix("myProperty", false);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_String_When_AddNullChecks_Is_True_But_Property_Is_Nullable()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();

            // Act
            var result = sut.GetNullCheckSuffix("myProperty", true);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_String_When_AddNullChecks_Is_True_But_Property_Is_ValueType()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(int)).Build();

            // Act
            var result = sut.GetNullCheckSuffix("myProperty", true);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_NullThrowingExpression_When_AddNullChecks_Is_True_And_Property_Is_Not_ValueType_And_Not_Nullable()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).Build();

            // Act
            var result = sut.GetNullCheckSuffix("myProperty", true);

            // Assert
            result.Should().Be(" ?? throw new System.ArgumentNullException(nameof(myProperty))");
        }
    }

    public class EnsureParentTypeFullName
    {
        [Fact]
        public void Returns_ClassProperty_With_Original_ParentTypeFullName_When_Filled()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithParentTypeFullName("Original").Build();
            var parentClass = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").BuildTyped();

            // Act
            var result = sut.EnsureParentTypeFullName(parentClass);

            // Assert
            result.ParentTypeFullName.Should().Be("Original");
        }

        [Fact]
        public void Returns_ClassProperty_With_ParentTypeFullName_From_ParentClass_When_Original_ParentTypeFullName_Is_Empty()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithParentTypeFullName(string.Empty).Build();
            var parentClass = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").BuildTyped();

            // Act
            var result = sut.EnsureParentTypeFullName(parentClass);

            // Assert
            result.ParentTypeFullName.Should().Be("MyNamespace.MyClass");
        }

        [Fact]
        public void Returns_ClassProperty_With_ParentTypeFullName_From_ParentClass_Without_Generics_When_Original_ParentTypeFullName_Is_Empty()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithParentTypeFullName(string.Empty).Build();
            var parentClass = new ClassBuilder().WithName("MyClass").WithNamespace("MyNamespace").AddGenericTypeArguments("T").BuildTyped();

            // Act
            var result = sut.EnsureParentTypeFullName(parentClass);

            // Assert
            result.ParentTypeFullName.Should().Be("MyNamespace.MyClass");
        }
    }

    public class GetInitializationName
    {
        [Fact]
        public void Throws_On_Null_CultureInfo()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderMemberName(default, default, default, cultureInfo: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("cultureInfo");
        }
    }

    public class GetBuilderClassConstructorInitializer : ClassPropertyExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderClassConstructorInitializer(context: null!, formattableStringParser, sut.TypeName))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_On_Null_FormatStringParser()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(new ClassBuilder(), new BuilderContext(CreateModel(), new Pipelines.Builder.PipelineBuilderSettings(), CultureInfo.InvariantCulture));

            // Act & Assert
            sut.Invoking(x => x.GetBuilderClassConstructorInitializer(context, formattableStringParser: null!, sut.TypeName))
               .Should().Throw<ArgumentNullException>().WithParameterName("formattableStringParser");
        }

        [Fact]
        public void Throws_On_Null_TypeName()
        {
            // Arrange
            var sut = new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(new ClassBuilder(), new BuilderContext(CreateModel(), new Pipelines.Builder.PipelineBuilderSettings(), CultureInfo.InvariantCulture));
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderClassConstructorInitializer(context, formattableStringParser: formattableStringParser, typeName: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("typeName");
        }
    }
}
