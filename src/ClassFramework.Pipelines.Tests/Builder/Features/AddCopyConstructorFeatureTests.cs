﻿namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddCopyConstructorFeatureTests : TestBase<Pipelines.Builder.Features.AddCopyConstructorFeature>
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
            var settings = CreateBuilderSettings(
                enableBuilderInheritance: true,
                baseClass: hasBaseClass ? new ClassBuilder().WithName("BaseClass").BuildTyped() : null,
                isAbstract: hasBaseClass,
                addCopyConstructor: true,
                enableEntityInheritance: true);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
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
            var settings = CreateBuilderSettings(
                enableBuilderInheritance: false,
                addCopyConstructor: true,
                enableEntityInheritance: false);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
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
                "Property3.AddRange(source.Property3);"
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
            var settings = CreateBuilderSettings(
                enableBuilderInheritance: false,
                addCopyConstructor: true,
                enableEntityInheritance: true);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
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
                "Property3.AddRange(source.Property3);"
            );
        }

        [Fact]
        public void Adds_Copy_Constructor_For_Non_Abstract_Builder_With_NullChecks()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                addNullChecks: true,
                enableBuilderInheritance: false,
                addCopyConstructor: true,
                enableEntityInheritance: false);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
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
                "if (source is null) throw new System.ArgumentNullException(nameof(source));",
                "_property3 = new System.Collections.Generic.List<int>();",
                "Property1 = source.Property1;",
                "_property2 = source.Property2;",
                "if (source.Property3 is not null) Property3.AddRange(source.Property3);"
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
            var settings = CreateBuilderSettings(
                enableBuilderInheritance: false,
                addCopyConstructor: true,
                newCollectionTypeName: typeof(IEnumerable<>).WithoutGenerics(),
                enableEntityInheritance: false);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
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
                "Property3 = Property3.Concat(source.Property3);"
            );
        }

        [Fact]
        public void Returns_Error_When_Parsing_CustomBuilderConstructorInitializeExpression_Is_Not_Successful()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderConstructorInitializeExpression).WithValue("{Error}"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                enableBuilderInheritance: false,
                addCopyConstructor: true,
                enableEntityInheritance: false);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Returns_Error_When_Parsing_CustomBuilderArgumentType_Is_Not_Successful()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentType).WithValue("{Error}"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                enableBuilderInheritance: false,
                addCopyConstructor: true,
                enableEntityInheritance: false);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        private static PipelineContext<IConcreteTypeBuilder, BuilderContext> CreateContext(IConcreteType sourceModel, ClassBuilder model, Pipelines.Builder.PipelineBuilderSettings settings)
            => new(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));
    }
}
