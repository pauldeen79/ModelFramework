namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddFluentMethodsForNonCollectionPropertiesFeatureTests : TestBase<AddFluentMethodsForNonCollectionPropertiesFeature>
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
            var settings = new PipelineBuilderSettings(nameSettings: new PipelineBuilderNameSettings(setMethodNameFormatString: string.Empty));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
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
            var settings = new PipelineBuilderSettings(nameSettings: new PipelineBuilderNameSettings(setMethodNameFormatString: "With{Name}"));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
            model.Methods.Should().HaveCount(3);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2", "WithProperty3");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property1", "property2", "property3");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName.FixTypeName()).Should().BeEquivalentTo("System.Int32", "System.String", "System.Collections.Generic.List<System.Int32>");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeEquivalentTo(default(object));
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property1 = property1;",
                "return this;",
                "Property2 = property2;",
                "return this;",
                "Property3 = property3;",
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
            var settings = new PipelineBuilderSettings(nameSettings: new PipelineBuilderNameSettings(setMethodNameFormatString: "With{Name}"), generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
            model.Methods.Should().HaveCount(3);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2", "WithProperty3");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property1", "property2", "property3");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName.FixTypeName()).Should().BeEquivalentTo("System.Int32", "System.String", "System.Collections.Generic.List<System.Int32>");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeEquivalentTo(default(object));
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "/* argument null check goes here */",
                "Property1 = property1;",
                "return this;",
                "/* argument null check goes here */",
                "Property2 = property2;",
                "return this;",
                "/* argument null check goes here */",
                "Property3 = property3;",
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
            var settings = new PipelineBuilderSettings(nameSettings: new PipelineBuilderNameSettings(setMethodNameFormatString: "With{Name}"), generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
            model.Methods.Should().HaveCount(3);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2", "WithProperty3");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property1", "property2", "property3");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName.FixTypeName()).Should().BeEquivalentTo("System.Int32", "System.String", "System.Collections.Generic.List<System.Int32>");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeEquivalentTo(default(object));
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "/* custom argument null check goes here */",
                "Property1 = property1;",
                "return this;",
                "/* custom argument null check goes here */",
                "Property2 = property2;",
                "return this;",
                "/* custom argument null check goes here */",
                "Property3 = property3;",
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
            var settings = new PipelineBuilderSettings(nameSettings: new PipelineBuilderNameSettings(setMethodNameFormatString: "With{Name}"));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
            model.Methods.Should().HaveCount(3);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2", "WithProperty3");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property1", "property2", "property3");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName).Should().BeEquivalentTo("CustomProperty1", "CustomProperty2", "CustomProperty3");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeEquivalentTo(default(object));
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property1 = property1;",
                "return this;",
                "Property2 = property2;",
                "return this;",
                "Property3 = property3;",
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
            var settings = new PipelineBuilderSettings(nameSettings: new PipelineBuilderNameSettings(setMethodNameFormatString: "With{Name}"));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
            model.Methods.Should().HaveCount(3);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2", "WithProperty3");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property1", "property2", "property3");
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
            var settings = new PipelineBuilderSettings(nameSettings: new PipelineBuilderNameSettings(setMethodNameFormatString: "With{Name}"));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
            model.Methods.Should().HaveCount(3);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2", "WithProperty3");
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property1 = property1; // custom",
                "return this;",
                "Property2 = property2; // custom",
                "return this;",
                "Property3 = property3; // custom",
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
            var settings = new PipelineBuilderSettings(nameSettings: new PipelineBuilderNameSettings(builderNameFormatString: "My{Class.Name}Builder"));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Continue);
            model.Methods.Should().HaveCount(3);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2", "WithProperty3");
        }
    }
}
