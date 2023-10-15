namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddCopyConstructorFeatureTests : TestBase<AddCopyConstructorFeature>
{
    public class Process : AddCopyConstructorFeatureTests
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
        [InlineData(true)]
        [InlineData(false)]
        public void Adds_Copy_Constructor_For_Abstract_Builder(bool hasBaseClass)
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: hasBaseClass ? new ClassBuilder().WithName("BaseClass").BuildTyped() : null, isAbstract: hasBaseClass),
                constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: true),
                classSettings: new ImmutableClassPipelineBuilderSettings(inheritanceSettings: new ImmutableClassPipelineBuilderInheritanceSettings(enableInheritance: true))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Constructors.Should().ContainSingle();
            var ctor = model.Constructors.Single();
            ctor.Protected.Should().BeTrue();
            ctor.ChainCall.Should().Be("base(source)");
            ctor.Parameters.Should().ContainSingle();
            var parameter = ctor.Parameters.Single();
            parameter.Name.Should().Be("source");
            parameter.TypeName.Should().Be("SomeNamespace.SomeClass");
            ctor.CodeStatements.Should().BeEmpty();
        }

        [Fact]
        public void Adds_Copy_Constructor_For_Non_Abstract_Builder()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: false),
                constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: true),
                classSettings: new ImmutableClassPipelineBuilderSettings(inheritanceSettings: new ImmutableClassPipelineBuilderInheritanceSettings(enableInheritance: false))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Constructors.Should().ContainSingle();
            var ctor = model.Constructors.Single();
            ctor.Protected.Should().BeFalse();
            ctor.ChainCall.Should().BeEmpty();
            ctor.Parameters.Should().ContainSingle();
            var parameter = ctor.Parameters.Single();
            parameter.Name.Should().Be("source");
            parameter.TypeName.Should().Be("SomeNamespace.SomeClass");
            ctor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            ctor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property3 = new System.Collections.Generic.List<int>();",
                "Property1 = source.Property1;",
                "Property2 = source.Property2;",
                "/* null check goes here */ Property3.AddRange(source.Property3);"
            );
        }

        [Fact]
        public void Adds_Copy_Constructor_For_Non_Abstract_Builder_With_BaseClass()
        {
            // Arrange
            var sourceModel = CreateModel("MyBaseClass");
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: false),
                constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: true),
                classSettings: new ImmutableClassPipelineBuilderSettings(inheritanceSettings: new ImmutableClassPipelineBuilderInheritanceSettings(enableInheritance: true))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Constructors.Should().ContainSingle();
            var ctor = model.Constructors.Single();
            ctor.Protected.Should().BeTrue();
            ctor.ChainCall.Should().Be("base(source)");
            ctor.Parameters.Should().ContainSingle();
            var parameter = ctor.Parameters.Single();
            parameter.Name.Should().Be("source");
            parameter.TypeName.Should().Be("SomeNamespace.SomeClass");
            ctor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            ctor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property3 = new System.Collections.Generic.List<int>();",
                "Property1 = source.Property1;",
                "Property2 = source.Property2;",
                "/* null check goes here */ Property3.AddRange(source.Property3);"
            );
        }

        [Fact]
        public void Creates_Correct_Copy_Constructor_Code_For_Non_Abstract_Builder_With_CollectionType_Enumerable()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: false),
                constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: true), typeSettings: new PipelineBuilderTypeSettings(newCollectionTypeName: typeof(IEnumerable<>).WithoutGenerics()),
                classSettings: new ImmutableClassPipelineBuilderSettings(inheritanceSettings: new ImmutableClassPipelineBuilderInheritanceSettings(enableInheritance: false))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Constructors.Should().ContainSingle();
            var ctor = model.Constructors.Single();
            ctor.Protected.Should().BeFalse();
            ctor.ChainCall.Should().BeEmpty();
            ctor.Parameters.Should().ContainSingle();
            var parameter = ctor.Parameters.Single();
            parameter.Name.Should().Be("source");
            parameter.TypeName.Should().Be("SomeNamespace.SomeClass");
            ctor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            ctor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property3 = System.Linq.Enumerable.Empty<int>();",
                "Property1 = source.Property1;",
                "Property2 = source.Property2;",
                "/* null check goes here */ Property3 = Property3.Concat(source.Property3);"
            );
        }
    }
}
