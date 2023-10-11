namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddConstructorsFeatureTests : TestBase<AddConstructorsFeature>
{
    public class Process : AddConstructorsFeatureTests
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
            InitializeParser(sourceModel);
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: hasBaseClass ? new ClassBuilder().WithName("BaseClass").BuildTyped() : null),
                constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: false),
                classSettings: new ImmutableClassPipelineBuilderSettings(inheritanceSettings: new ImmutableClassPipelineBuilderInheritanceSettings(enableInheritance: true))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
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
                    "Property3 = new System.Collections.Generic.List<int>();"
                );
            }
            else
            {
                ctor.CodeStatements.Should().BeEmpty();
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Adds_Copy_Constructor_For_Abstract_Builder(bool hasBaseClass)
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser(sourceModel);
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
            model.Constructors.Should().HaveCount(2);
            var ctor = model.Constructors[model.Constructors.Count - 1];
            ctor.Protected.Should().BeTrue();
            ctor.ChainCall.Should().Be("base(source)");
            ctor.Parameters.Should().ContainSingle();
            var parameter = ctor.Parameters.Single();
            parameter.Name.Should().Be("source");
            parameter.TypeName.Should().Be("SomeNamespace.SomeClass");
            ctor.CodeStatements.Should().BeEmpty();
        }

        [Fact]
        public void Adds_Default_Constructor_For_Non_Abstract_Builder()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser(sourceModel);
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: false),
                constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: false),
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
            ctor.Parameters.Should().BeEmpty();
            ctor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            ctor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property2 = string.Empty;",
                "Property3 = new System.Collections.Generic.List<int>();"
            );
        }

        [Fact]
        public void Adds_Default_Constructor_For_Non_Abstract_Builder_With_CollectionType_Enumerable()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser(sourceModel);
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: false), constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: false),
                typeSettings: new PipelineBuilderTypeSettings(newCollectionTypeName: typeof(IEnumerable<>).WithoutGenerics()),
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
            ctor.Parameters.Should().BeEmpty();
            ctor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            ctor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property2 = string.Empty;",
                "Property3 = System.Linq.Enumerable.Empty<int>();"
            );
        }

        [Fact]
        public void Adds_Default_Constructor_For_Non_Abstract_Builder_With_BaseClass()
        {
            // Arrange
            var sourceModel = CreateModel("MyBaseClass");
            InitializeParser(sourceModel);
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: false),
                constructorSettings: new PipelineBuilderConstructorSettings(addCopyConstructor: false),
                classSettings: new ImmutableClassPipelineBuilderSettings(inheritanceSettings: new ImmutableClassPipelineBuilderInheritanceSettings(enableInheritance: true))
                );
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            sut.Process(context);

            // Assert
            model.Constructors.Should().ContainSingle();
            var ctor = model.Constructors.Single();
            ctor.Protected.Should().BeTrue();
            ctor.ChainCall.Should().Be("base()");
            ctor.Parameters.Should().BeEmpty();
            ctor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            ctor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "Property2 = string.Empty;",
                "Property3 = new System.Collections.Generic.List<int>();"
            );
        }

        [Fact]
        public void Adds_Copy_Constructor_For_Non_Abstract_Builder()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser(sourceModel);
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
            model.Constructors.Should().HaveCount(2);
            var ctor = model.Constructors[model.Constructors.Count - 1];
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
            InitializeParser(sourceModel);
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
            model.Constructors.Should().HaveCount(2);
            var ctor = model.Constructors[model.Constructors.Count - 1];
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
            InitializeParser(sourceModel);
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
            model.Constructors.Should().HaveCount(2);
            var ctor = model.Constructors[model.Constructors.Count - 1];
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

        private void InitializeParser(TypeBase sourceModel)
        {
            var parser = Fixture.Freeze<IFormattableStringParser>();
            parser.Parse(Arg.Any<string>(), Arg.Any<IFormatProvider>(), Arg.Any<object?>())
                  .Returns(x => Result<string>.Success(x.ArgAt<string>(0)
                    .Replace("{Name}", sourceModel.Name, StringComparison.Ordinal)
                    .Replace("{NullCheck}", "/* null check goes here */ ", StringComparison.Ordinal)
                    ));
        }

        private static TypeBase CreateModel(string baseClass = "")
            => new ClassBuilder()
                .WithName("SomeClass")
                .WithNamespace("SomeNamespace")
                .WithBaseClass(baseClass)
                .AddProperties(new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)))
                .AddProperties(new ClassPropertyBuilder().WithName("Property2").WithType(typeof(string)))
                .AddProperties(new ClassPropertyBuilder().WithName("Property3").WithType(typeof(List<int>)))
                .Build();
    }
}
