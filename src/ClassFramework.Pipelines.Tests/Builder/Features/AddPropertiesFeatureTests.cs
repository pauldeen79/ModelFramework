﻿namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddPropertiesFeatureTests : TestBase<Pipelines.Builder.Features.AddPropertiesFeature>
{
    public class Process : AddPropertiesFeatureTests
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
        public void Does_Not_Add_Properties_On_Abstract_Builder()
        {
            // Arrange
            var sourceModel = CreateModel();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(enableBuilderInheritance: true, isAbstract: true);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.Should().BeEmpty();
        }

        [Fact]
        public void Adds_Properties_On_Non_Abstract_Builder()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                enableBuilderInheritance: true,
                isAbstract: false,
                baseClass: new ClassBuilder().WithName("MyBaseClass").AddProperties(new ClassPropertyBuilder().WithName("Property4").WithType(typeof(int))).BuildTyped());
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.Select(x => x.Name).Should().BeEquivalentTo(sourceModel.Properties.Select(x => x.Name));
            model.Properties.Select(x => x.TypeName).Should().BeEquivalentTo(sourceModel.Properties.Select(x => x.TypeName.FixTypeName()));
            model.Properties.Select(x => x.IsNullable).Should().BeEquivalentTo(sourceModel.Properties.Select(x => x.IsNullable));
            model.Properties.Select(x => x.IsValueType).Should().BeEquivalentTo(sourceModel.Properties.Select(x => x.IsValueType));
        }

        [Fact]
        public void Uses_CustomBuilderArgumentType_When_Present()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentType).WithValue("MyCustomType"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                enableBuilderInheritance: true,
                isAbstract: false,
                baseClass: new ClassBuilder().WithName("MyBaseClass").AddProperties(new ClassPropertyBuilder().WithName("Property4").WithType(typeof(int))).BuildTyped());
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.Select(x => x.Name).Should().BeEquivalentTo(sourceModel.Properties.Select(x => x.Name));
            model.Properties.Select(x => x.TypeName).Should().AllBe("MyCustomType");
        }

        [Fact]
        public void Replaces_CollectionTypeName_Correctly()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                enableBuilderInheritance: true,
                isAbstract: false,
                baseClass: new ClassBuilder().WithName("MyBaseClass").AddProperties(new ClassPropertyBuilder().WithName("Property4").WithType(typeof(int))).BuildTyped(),
                newCollectionTypeName: typeof(ReadOnlyCollection<>).WithoutGenerics());
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.Select(x => x.Name).Should().BeEquivalentTo(sourceModel.Properties.Select(x => x.Name));
            model.Properties.Select(x => x.TypeName).Should().BeEquivalentTo("System.Int32", "System.String", "System.Collections.ObjectModel.ReadOnlyCollection<System.Int32>");
        }

        [Fact]
        public void Adds_Metadata_To_Properties_From_SourceModel_Properties()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName("MyMetadata").WithValue("Value"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings();
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.Metadata).Should().BeEquivalentTo(Enumerable.Range(1, 3).Select(_ => new MetadataBuilder().WithName("MyMetadata").WithValue("Value")));
        }

        [Fact]
        public void Adds_Attributes_To_Properties_From_SourceModel_Properties_When_CopyAttributes_Is_True()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName("MyMetadata").WithValue("Value"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(copyAttributes: true);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.Attributes).Select(x => x.Name).Should().NotBeEmpty();
            model.Properties.SelectMany(x => x.Attributes).Select(x => x.Name).Should().AllBeEquivalentTo("MyAttribute");
        }

        [Fact]
        public void Does_Not_Add_Attributes_To_Properties_From_SourceModel_Properties_When_CopyAttributes_Is_False()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName("MyMetadata").WithValue("Value"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(copyAttributes: false);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.Attributes).Should().BeEmpty();
        }

        [Fact]
        public void Does_Not_Add_CodeStatements_To_Properties_When_AddNullChecks_Is_False()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(addNullChecks: false);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.GetterCodeStatements).Should().BeEmpty();
            model.Properties.SelectMany(x => x.SetterCodeStatements).Should().BeEmpty();
        }

        [Fact]
        public void Does_Not_Add_CodeStatements_To_Properties_When_ValidateArguments_Is_Shared()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                addNullChecks: true,
                validateArguments: ArgumentValidationType.Shared);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.GetterCodeStatements).Should().BeEmpty();
            model.Properties.SelectMany(x => x.SetterCodeStatements).Should().BeEmpty();
        }

        [Theory]
        [InlineData(ArgumentValidationType.None)]
        [InlineData(ArgumentValidationType.DomainOnly)]
        public void Adds_CodeStatements_To_Properties_When_AddNullChecks_Is_True_And_ValidateArguments_Is(ArgumentValidationType validateArguments)
        {
            // Arrange
            var sourceModel = new ClassBuilder()
                .WithName("SomeClass")
                .WithNamespace("SomeNamespace")
                .AddProperties(new ClassPropertyBuilder().WithName("MyOptionalProperty").WithType(typeof(string)).WithIsNullable())
                .AddProperties(new ClassPropertyBuilder().WithName("MyRequiredProperty").WithType(typeof(string)))
                .BuildTyped();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                addNullChecks: true,
                enableNullableReferenceTypes: true,
                validateArguments: validateArguments);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.GetterCodeStatements).Should().NotBeEmpty();
            model.Properties.SelectMany(x => x.GetterCodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Properties.SelectMany(x => x.GetterCodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo("return _myRequiredProperty;");
            model.Properties.SelectMany(x => x.SetterCodeStatements).Should().NotBeEmpty();
            model.Properties.SelectMany(x => x.SetterCodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Properties.SelectMany(x => x.SetterCodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo("_myRequiredProperty = value ?? throw new System.ArgumentNullException(nameof(value));");
        }

        [Theory]
        [InlineData(ArgumentValidationType.None)]
        [InlineData(ArgumentValidationType.DomainOnly)]
        public void Adds_CodeStatements_To_Properties_With_CsharpFriendlyName_When_AddNullChecks_Is_True_And_ValidateArguments_Is(ArgumentValidationType validateArguments)
        {
            // Arrange
            var sourceModel = new ClassBuilder()
                .WithName("SomeClass")
                .WithNamespace("SomeNamespace")
                .AddProperties(new ClassPropertyBuilder().WithName("Delegate").WithType(typeof(string)))
                .BuildTyped();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                addNullChecks: true,
                enableNullableReferenceTypes: true,
                validateArguments: validateArguments);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.GetterCodeStatements).Should().NotBeEmpty();
            model.Properties.SelectMany(x => x.GetterCodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Properties.SelectMany(x => x.GetterCodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo("return _delegate;");
            model.Properties.SelectMany(x => x.SetterCodeStatements).Should().NotBeEmpty();
            model.Properties.SelectMany(x => x.SetterCodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Properties.SelectMany(x => x.SetterCodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo("_delegate = value ?? throw new System.ArgumentNullException(nameof(value));");
        }

        [Theory]
        [InlineData(ArgumentValidationType.None)]
        [InlineData(ArgumentValidationType.DomainOnly)]
        public void Adds_Fields_When_AddNullChecks_Is_True_And_ValidateArguments_Is(ArgumentValidationType validateArguments)
        {
            // Arrange
            var sourceModel = new ClassBuilder()
                .WithName("SomeClass")
                .WithNamespace("SomeNamespace")
                .AddProperties(new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string)))
                .BuildTyped();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                addNullChecks: true,
                enableNullableReferenceTypes: true,
                validateArguments: validateArguments);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Fields.Select(x => x.Name).Should().BeEquivalentTo("_myProperty");
        }

        [Fact]
        public void Returns_Error_When_Parsing_CustomBuilderArgumentType_Is_Not_Succesful()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentType).WithValue("{Error}"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings();
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
