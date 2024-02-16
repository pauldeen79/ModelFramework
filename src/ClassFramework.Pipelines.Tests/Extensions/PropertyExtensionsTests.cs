﻿namespace ClassFramework.Pipelines.Tests.Extensions;

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
            sut.Invoking(x => x.GetBuilderMemberName(default, default, default, default, cultureInfo: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("cultureInfo");
        }
    }

    public class GetBuilderClassConstructorInitializer : PropertyExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_TypeSettings()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
            var formatProvider = Fixture.Freeze<IFormatProvider>();
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderConstructorInitializer(typeSettings: default!, formatProvider, new object(), string.Empty, string.Empty, formattableStringParser))
               .Should().Throw<ArgumentNullException>().WithParameterName("typeSettings");
        }

        [Fact]
        public void Throws_On_Null_FormatProvider()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
            var typeSettings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderConstructorInitializer(typeSettings, formatProvider: default!, new object(), string.Empty, string.Empty, formattableStringParser))
               .Should().Throw<ArgumentNullException>().WithParameterName("formatProvider");
        }

        [Fact]
        public void Throws_On_Null_ParentChildContext()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
            var typeSettings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            var formatProvider = Fixture.Freeze<IFormatProvider>();
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderConstructorInitializer(typeSettings, formatProvider, parentChildContext: default!, string.Empty, string.Empty, formattableStringParser))
               .Should().Throw<ArgumentNullException>().WithParameterName("parentChildContext");
        }

        [Fact]
        public void Throws_On_Null_MappedTypeName()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
            var typeSettings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            var formatProvider = Fixture.Freeze<IFormatProvider>();
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderConstructorInitializer(typeSettings, formatProvider, new object(), mappedTypeName: default!, string.Empty, formattableStringParser))
               .Should().Throw<ArgumentNullException>().WithParameterName("mappedTypeName");
        }

        [Fact]
        public void Throws_On_Null_NewCollectionTypeName()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
            var typeSettings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            var formatProvider = Fixture.Freeze<IFormatProvider>();
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderConstructorInitializer(typeSettings, formatProvider, new object(), string.Empty, newCollectionTypeName: default!, formattableStringParser))
               .Should().Throw<ArgumentNullException>().WithParameterName("newCollectionTypeName");
        }

        [Fact]
        public void Throws_On_Null_FormatStringParser()
        {
            // Arrange
            var sut = CreateSut().WithName("MyProperty").WithType(typeof(string)).WithIsNullable().Build();
            var typeSettings = Fixture.Freeze<IPipelineBuilderTypeSettings>();
            var formatProvider = Fixture.Freeze<IFormatProvider>();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderConstructorInitializer(typeSettings, formatProvider, new object(), string.Empty, string.Empty, formattableStringParser: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("formattableStringParser");
        }
    }
}
