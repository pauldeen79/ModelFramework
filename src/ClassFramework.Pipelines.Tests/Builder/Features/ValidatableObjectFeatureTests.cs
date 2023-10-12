namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class ValidatableObjectFeatureTests : TestBase<ValidatableObjectFeature>
{
    public class Process : ValidatableObjectFeatureTests
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
        public void Adds_IValidatableObject_Interface_When_IsBuilderForAbstractEntity_Is_False_And_Validation_Is_Shared_Between_Builder_And_Entity()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").AddProperties(new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string))).Build();
            InitializeParser(sourceModel);
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(classSettings: new ImmutableClassPipelineBuilderSettings(constructorSettings: new ImmutableClassPipelineBuilderConstructorSettings(validateArguments: ArgumentValidationType.Shared)));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Interfaces.Should().BeEquivalentTo("System.ComponentModel.DataAnnotations.IValidatableObject");
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("Validate");
            model.Methods.Single(x => x.Name == "Validate").CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "var instance = new SomeNamespace.SomeClassBase { MyProperty = MyProperty };",
                "var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();",
                "System.ComponentModel.DataAnnotations.Validator.TryValidateObject(instance, new System.ComponentModel.DataAnnotations.ValidationContext(instance), results, true);",
                "return results;"
            );
        }

        [Fact]
        public void Adds_IValidatableObject_Interface_When_IsBuilderForAbstractEntity_Is_False_And_Validation_Is_Shared_Between_Builder_And_Entity_No_Properties()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").Build();
            InitializeParser(sourceModel);
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(classSettings: new ImmutableClassPipelineBuilderSettings(constructorSettings: new ImmutableClassPipelineBuilderConstructorSettings(validateArguments: ArgumentValidationType.Shared)));
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Interfaces.Should().BeEquivalentTo("System.ComponentModel.DataAnnotations.IValidatableObject");
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("Validate");
            model.Methods.Single(x => x.Name == "Validate").CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "var instance = new SomeNamespace.SomeClassBase();",
                "var results = new System.Collections.Generic.List<System.ComponentModel.DataAnnotations.ValidationResult>();",
                "System.ComponentModel.DataAnnotations.Validator.TryValidateObject(instance, new System.ComponentModel.DataAnnotations.ValidationContext(instance), results, true);",
                "return results;"
            );
        }

        [Fact]
        public void Does_Not_Add_IValidatableObject_Interface_When_IsBuilderForAbstractEntity_Is_False_And_Validation_Is_Not_Shared_Between_Builder_And_Entity()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Interfaces.Should().BeEmpty();
            model.Methods.Should().BeEmpty();
        }
    }
}
