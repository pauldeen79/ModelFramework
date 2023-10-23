namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddFluentMethodsForCollectionPropertiesFeatureTests : TestBase<AddFluentMethodsForCollectionPropertiesFeature>
{
    public class Process : AddFluentMethodsForCollectionPropertiesFeatureTests
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
        public void Does_Not_Add_Method_When_AddMethodNameFormatString_Is_Empty()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(nameSettings: new PipelineBuilderNameSettings(addMethodNameFormatString: string.Empty));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            sourceModel.Methods.Should().BeEmpty();
        }

        [Fact]
        public void Adds_Methods_When_AddMethodNameFormatString_Is_Not_Empty()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(nameSettings: new PipelineBuilderNameSettings(addMethodNameFormatString: "Add{Name}"));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().HaveCount(2);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("AddProperty3", "AddProperty3");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property3", "property3");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName).Should().BeEquivalentTo("System.Collections.Generic.IEnumerable<System.Int32>", "System.Int32[]");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeEquivalentTo(default(object));
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "return AddProperty3(property3.ToArray());",
                "Property3.AddRange(property3);",
                "return this;"
            );
        }
    }
}
