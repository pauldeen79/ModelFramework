namespace ClassFramework.Pipelines.Tests.Extensions;

public class PropertyExtensionsTests : TestBase<PropertyBuilder>
{
    public class GetDefaultValue : PropertyExtensionsTests
    {
        [Fact]
        public void Gets_Value_From_TypeName_When_Metadata_Is_Not_Found()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
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
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().AddMetadata(MetadataNames.CustomBuilderDefaultValue, null).Build();
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
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().AddMetadata(MetadataNames.CustomBuilderDefaultValue, new Literal("custom value", null)).Build();
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
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().AddMetadata(MetadataNames.CustomBuilderDefaultValue, "custom value").Build();
            var csharpExpressionCreator = Fixture.Freeze<ICsharpExpressionCreator>();
            csharpExpressionCreator.Create(Arg.Any<object?>()).Returns(x => x.ArgAt<object?>(0).ToStringWithNullCheck());

            // Act
            var result = sut.GetDefaultValue(csharpExpressionCreator, false, sut.TypeName);

            // Assert
            result.Should().Be("custom value");
        }
    }

    public class GetNullCheckSuffix : PropertyExtensionsTests
    {
        [Fact]
        public void Returns_Empty_String_When_AddNullChecks_Is_False()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).Build();

            // Act
            var result = sut.GetNullCheckSuffix("myProperty", false);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_String_When_AddNullChecks_Is_True_But_Property_Is_Nullable()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();

            // Act
            var result = sut.GetNullCheckSuffix("myProperty", true);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_String_When_AddNullChecks_Is_True_But_Property_Is_ValueType()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(int)).Build();

            // Act
            var result = sut.GetNullCheckSuffix("myProperty", true);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_NullThrowingExpression_When_AddNullChecks_Is_True_And_Property_Is_Not_ValueType_And_Not_Nullable()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).Build();

            // Act
            var result = sut.GetNullCheckSuffix("myProperty", true);

            // Assert
            result.Should().Be(" ?? throw new System.ArgumentNullException(nameof(myProperty))");
        }
    }

    public class GetInitializationName : PropertyExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_CultureInfo()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderMemberName(default, default, default, cultureInfo: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("cultureInfo");
        }
    }

    public class GetBuilderClassConstructorInitializer : PropertyExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderConstructorInitializer(context: default(PipelineContext<TypeBaseBuilder, BuilderContext>)!, formattableStringParser))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_On_Null_FormatStringParser()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(new ClassBuilder(), new BuilderContext(CreateModel(), new Pipelines.Builder.PipelineSettings(), CultureInfo.InvariantCulture));

            // Act & Assert
            sut.Invoking(x => x.GetBuilderConstructorInitializer(context, formattableStringParser: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("formattableStringParser");
        }
    }
}
