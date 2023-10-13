namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class BaseClassFeatureTests : TestBase<BaseClassFeature>
{
    public class Process : BaseClassFeatureTests
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
        public void Sets_BaseClass_For_BuilderInheritance_And_Not_For_Abstract_Builder_No_BaseClass()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass:  null),
                classSettings: new ImmutableClassPipelineBuilderSettings(inheritanceSettings: new ImmutableClassPipelineBuilderInheritanceSettings(enableInheritance: true))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.BaseClass.Should().Be("SomeClassBuilder");
        }

        [Fact]
        public void Sets_BaseClass_For_BuilderInheritance_And_Not_For_Abstract_Builder_With_BaseClass_And_Abstract()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: new ClassBuilder().WithName("BaseClass").BuildTyped(), isAbstract: true),
                classSettings: new ImmutableClassPipelineBuilderSettings(inheritanceSettings: new ImmutableClassPipelineBuilderInheritanceSettings(enableInheritance: true))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.BaseClass.Should().Be("SomeClassBuilder");
        }

        [Fact]
        public void Sets_BaseClass_For_BuilderInheritance_And_Not_For_Abstract_Builder_With_BaseClass_And_Not_Abstract_No_Builders_Namespace()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: new ClassBuilder().WithName("BaseClass").BuildTyped(), isAbstract: false),
                classSettings: new ImmutableClassPipelineBuilderSettings(inheritanceSettings: new ImmutableClassPipelineBuilderInheritanceSettings(enableInheritance: true))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.BaseClass.Should().Be("BaseClassBuilder<SomeClassBuilder, SomeNamespace.SomeClass>");
        }

        [Fact]
        public void Sets_BaseClass_For_BuilderInheritance_And_Not_For_Abstract_Builder_With_BaseClass_And_Not_Abstract_Builders_Namespace()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: new ClassBuilder().WithName("BaseClass").BuildTyped(), isAbstract: false, baseClassBuilderNameSpace: "BaseBuilders"),
                classSettings: new ImmutableClassPipelineBuilderSettings(inheritanceSettings: new ImmutableClassPipelineBuilderInheritanceSettings(enableInheritance: true))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.BaseClass.Should().Be("BaseBuilders.BaseClassBuilder<SomeClassBuilder, SomeNamespace.SomeClass>");
        }

        [Fact]
        public void Sets_BaseClass_For_BuilderInheritance_For_Abstract_Builder_With_BaseClass()
        {
            // Arrange
            var sourceModel = CreateModel("MyBaseClass");
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true),
                classSettings: new ImmutableClassPipelineBuilderSettings(inheritanceSettings: new ImmutableClassPipelineBuilderInheritanceSettings(enableInheritance: true))
                ).ForAbstractBuilder();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.BaseClass.Should().Be("MyBaseClassBuilder");
        }

        [Fact]
        public void Sets_BaseClass_For_No_BuilderInheritance_For_Abstract_Builder_With_BaseClass()
        {
            // Arrange
            var sourceModel = CreateModel("MyBaseClass");
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: false),
                classSettings: new ImmutableClassPipelineBuilderSettings(inheritanceSettings: new ImmutableClassPipelineBuilderInheritanceSettings(enableInheritance: true))
                ).ForAbstractBuilder();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.BaseClass.Should().Be("MyBaseClassBuilder<SomeClassBuilder, SomeNamespace.SomeClass>");
        }
    }
}
