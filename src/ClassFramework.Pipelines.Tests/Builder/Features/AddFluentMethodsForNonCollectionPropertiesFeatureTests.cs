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
            sut.Process(context);

            // Assert
            sourceModel.Methods.Should().BeEmpty();
        }

        [Fact]
        public void Adds_Method_When_SetMethodNameFormatString_Is_Empty()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(nameSettings: new PipelineBuilderNameSettings(setMethodNameFormatString: "With{Name}"));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Methods.Should().HaveCount(3);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("WithProperty1", "WithProperty2", "WithProperty3");
            model.Methods.Select(x => x.TypeName).Should().BeEquivalentTo("Property1Builder", "Property2Builder", "Property3Builder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property1", "property2", "property3");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName.FixTypeName()).Should().BeEquivalentTo("System.Int32", "System.String", "System.Collections.Generic.List<System.Int32>");
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
            sut.Process(context);

            // Assert
            model.Methods.Should().HaveCount(3);
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName).Should().BeEquivalentTo("CustomProperty1", "CustomProperty2", "CustomProperty3");
        }

        //TODO: Add unit test for context.Context.Settings.NameSettings.BuilderNameFormatString
        //TODO: Add unit test for MetadataNames.CustomBuilderWithDefaultPropertyValue
        //TODO: Add unit test for MetadataNames.CustomBuilderWithExpression
    }
}
