﻿namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddFluentMethodsForNonCollectionPropertiesFeatureTests : TestBase<Pipelines.Builder.Features.AddFluentMethodsForNonCollectionPropertiesFeature>
{
    public class Process : AddFluentMethodsForNonCollectionPropertiesFeatureTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut();

            // Act & Assert
            sut.Invoking(x => x.Process(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }
        
        [Fact]
        public void Does_Not_Add_Method_When_SetMethodNameFormatString_Is_Empty()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(setMethodNameFormatString: string.Empty);
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            sourceModel.Methods.Should().BeEmpty();
        }

        [Fact]
        public void Adds_Method_When_SetMethodNameFormatString_Is_Not_Empty()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(setMethodNameFormatString: "With{Name}");
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().HaveCount(2);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property1", "property2");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName).Should().BeEquivalentTo("System.Int32", "System.String");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeEquivalentTo(default(object));
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property1 = property1;",
                "return this;",
                "Property2 = property2;",
                "return this;"
            );
        }

        [Fact]
        public void Adds_Method_With_Default_ArgumentNullChecks_When_SetMethodNameFormatString_Is_Not_Empty()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                setMethodNameFormatString: "With{Name}",
                addNullChecks: true);
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().HaveCount(2);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property1", "property2");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName).Should().BeEquivalentTo("System.Int32", "System.String");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeEquivalentTo(default(object));
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "/* argument null check goes here */",
                "Property1 = property1;",
                "return this;",
                "/* argument null check goes here */",
                "Property2 = property2;",
                "return this;"
            );
        }

        [Fact]
        public void Adds_Method_With_Custom_ArgumentNullChecks_When_SetMethodNameFormatString_Is_Not_Empty()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentNullCheckExpression).WithValue("/* custom argument null check goes here */"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                setMethodNameFormatString: "With{Name}",
                addNullChecks: true);
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().HaveCount(2);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property1", "property2");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName).Should().BeEquivalentTo("System.Int32", "System.String");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeEquivalentTo(default(object));
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "/* custom argument null check goes here */",
                "Property1 = property1;",
                "return this;",
                "/* custom argument null check goes here */",
                "Property2 = property2;",
                "return this;"
            );
        }

        [Fact]
        public void Uses_CustomBuilderArgumentType_When_Present()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentType).WithValue("Custom{Name}"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(setMethodNameFormatString: "With{Name}");
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().HaveCount(2);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property1", "property2");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName).Should().BeEquivalentTo("CustomProperty1", "CustomProperty2");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeEquivalentTo(default(object));
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property1 = property1;",
                "return this;",
                "Property2 = property2;",
                "return this;"
            );
        }

        [Fact]
        public void Uses_CustomBuilderWithDefaultPropertyValue_When_Present()
        {
            // Arrange
            // Note that this doesn't seem logical for this unit test, but in code generation the Literal is needed for correct formatting of literal values.
            // If you would use a string without wrapping it in a Literal, then it will get formatted to "customDefaultValue" which may not be what you want.
            // Or, in case you just want a default boolean value, you might also use true and false directly, without wrapping it in a Literal...
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithDefaultPropertyValue).WithValue(new Literal("customDefaultValue")));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(setMethodNameFormatString: "With{Name}");
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().HaveCount(2);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property1", "property2");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeOfType<Literal>();
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).OfType<Literal>().Select(x => x.Value).Should().AllBeEquivalentTo("customDefaultValue");
        }

        [Fact]
        public void Uses_CustomBuilderWithExpression_When_Present()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderWithExpression).WithValue("{Name} = {NamePascal}; // custom"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(setMethodNameFormatString: "With{Name}");
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().HaveCount(2);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2");
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property1 = property1; // custom",
                "return this;",
                "Property2 = property2; // custom",
                "return this;"
            );
        }

        [Fact]
        public void Uses_Correct_BuilderNameFormatString()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(builderNameFormatString: "My{Class.Name}Builder");
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().HaveCount(2);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2");
        }

        [Fact]
        public void Returns_Error_When_Parsing_BuilderNameFormatString_Is_Not_Succesful()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentType).WithValue("{Error}"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);


            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }
    }
}
