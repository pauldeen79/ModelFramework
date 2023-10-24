namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddDefaultConstructorFeatureTests : TestBase<AddDefaultConstructorFeature>
{
    public class Process : AddDefaultConstructorFeatureTests
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

        [Theory]
        [InlineData(true, false, true)]
        [InlineData(false, true, false)]
        public void Adds_Default_Constructor_For_Abstract_Builder(bool hasBaseClass, bool expectedProtected, bool expectedCodeStatements)
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: hasBaseClass ? new ClassBuilder().WithName("BaseClass").BuildTyped() : null),
                constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: false),
                classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: true))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Constructors.Should().ContainSingle();
            var ctor = model.Constructors.Single();
            ctor.Protected.Should().Be(expectedProtected);
            ctor.ChainCall.Should().Be(!hasBaseClass ? "base()" : string.Empty); // sounds unlogical, but this is the non-abstract base class for the builder, and it needs the base() chaincall to the abstract base class for the builder
            ctor.Parameters.Should().BeEmpty();
            if (expectedCodeStatements)
            {
                ctor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
                ctor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
                (
                    "Property2 = string.Empty;",
                    "Property3 = new System.Collections.Generic.List<int>();",
                    "SetDefaultValues();"
                );
            }
            else
            {
                ctor.CodeStatements.Should().BeEmpty();
            }
        }

        [Fact]
        public void Adds_Default_Constructor_For_Non_Abstract_Builder()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: false),
                constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: false),
                classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: false))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Constructors.Should().ContainSingle();
            var ctor = model.Constructors.Single();
            ctor.Protected.Should().BeFalse();
            ctor.ChainCall.Should().BeEmpty();
            ctor.Parameters.Should().BeEmpty();
            ctor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            ctor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property2 = string.Empty;",
                "Property3 = new System.Collections.Generic.List<int>();",
                "SetDefaultValues();"
            );
        }

        [Fact]
        public void Adds_Default_Constructor_For_Non_Abstract_Builder_With_CollectionType_Enumerable()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: false), constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: false),
                typeSettings: new PipelineBuilderTypeSettings(newCollectionTypeName: typeof(IEnumerable<>).WithoutGenerics()),
                classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: false))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Constructors.Should().ContainSingle();
            var ctor = model.Constructors.Single();
            ctor.Protected.Should().BeFalse();
            ctor.ChainCall.Should().BeEmpty();
            ctor.Parameters.Should().BeEmpty();
            ctor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            ctor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property2 = string.Empty;",
                "Property3 = System.Linq.Enumerable.Empty<int>();",
                "SetDefaultValues();"
            );
        }

        [Fact]
        public void Adds_Default_Constructor_For_Non_Abstract_Builder_With_BaseClass()
        {
            // Arrange
            var sourceModel = CreateModel("MyBaseClass");
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: false),
                constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: false),
                classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: true))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Constructors.Should().ContainSingle();
            var ctor = model.Constructors.Single();
            ctor.Protected.Should().BeTrue();
            ctor.ChainCall.Should().Be("base()");
            ctor.Parameters.Should().BeEmpty();
            ctor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            ctor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property2 = string.Empty;",
                "Property3 = new System.Collections.Generic.List<int>();",
                "SetDefaultValues();"
            );
        }

        [Fact]
        public void Adds_Default_Constructor_Without_DefaultValue_Initialization_When_SetDefaultValues_Is_Set_To_False()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: false),
                constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: false, setDefaultValues: false),
                classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: false))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Constructors.Should().ContainSingle();
            var ctor = model.Constructors.Single();
            ctor.Protected.Should().BeFalse();
            ctor.ChainCall.Should().BeEmpty();
            ctor.Parameters.Should().BeEmpty();
            ctor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            ctor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property3 = new System.Collections.Generic.List<int>();"
            );
        }
    }
}
