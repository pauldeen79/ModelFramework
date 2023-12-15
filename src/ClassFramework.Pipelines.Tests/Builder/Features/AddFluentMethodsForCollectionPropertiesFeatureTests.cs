﻿namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddFluentMethodsForCollectionPropertiesFeatureTests : TestBase<Pipelines.Builder.Features.AddFluentMethodsForCollectionPropertiesFeature>
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
            var settings = CreateBuilderSettings(addMethodNameFormatString: string.Empty);
            var context = CreateContext(sourceModel, model, settings);

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
            var settings = CreateBuilderSettings(addMethodNameFormatString: "Add{Name}");
            var context = CreateContext(sourceModel, model, settings);

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

        [Fact]
        public void Adds_Methods_With_CustomBuilderArgumentType_When_Present()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentType).WithValue(typeof(IReadOnlyCollection<>).ReplaceGenericTypeName("MyCustomType")));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(addMethodNameFormatString: "Add{Name}");
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().HaveCount(2);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("AddProperty3", "AddProperty3");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property3", "property3");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName).Should().BeEquivalentTo("System.Collections.Generic.IEnumerable<MyCustomType>", "MyCustomType[]");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeEquivalentTo(default(object));
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "return AddProperty3(property3.ToArray());",
                "Property3.AddRange(property3);",
                "return this;"
            );
        }

        [Fact]
        public void Adds_Methods_With_ArgumentNullChecks()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                addNullChecks: true,
                addMethodNameFormatString: "Add{Name}");
            var context = CreateContext(sourceModel, model, settings);

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
                "return AddProperty3(property3?.ToArray() ?? throw new System.ArgumentNullException(nameof(property3)));",
                "if (property3 is null) throw new System.ArgumentNullException(nameof(property3));",
                "Property3.AddRange(property3);",
                "return this;"
            );
        }

        [Fact]
        public void Adds_Methods_With_ArgumentNullChecks_CsharpFriendlyName()
        {
            // Arrange
            var sourceModel = new ClassBuilder()
                .WithName("SomeClass")
                .WithNamespace("SomeNamespace")
                .AddProperties(new ClassPropertyBuilder().WithName("Delegate").WithType(typeof(List<int>)))
                .Build();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                addNullChecks: true,
                addMethodNameFormatString: "Add{Name}");
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().HaveCount(2);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("AddDelegate", "AddDelegate");
            model.Methods.Select(x => x.TypeName).Should().AllBe("SomeClassBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("delegate", "delegate");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName).Should().BeEquivalentTo("System.Collections.Generic.IEnumerable<System.Int32>", "System.Int32[]");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeEquivalentTo(default(object));
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "return AddDelegate(@delegate?.ToArray() ?? throw new System.ArgumentNullException(nameof(@delegate)));",
                "if (@delegate is null) throw new System.ArgumentNullException(nameof(@delegate));",
                "Delegate.AddRange(@delegate);",
                "return this;"
            );
        }

        [Fact]
        public void Adds_Methods_With_Enumerable()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                newCollectionTypeName: typeof(IEnumerable<>).WithoutGenerics(),
                addMethodNameFormatString: "Add{Name}");
            var context = CreateContext(sourceModel, model, settings);

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
                "Property3 = Property3.Concat(property3);",
                "return this;",
                "Property3 = Property3.Concat(property3);",
                "return this;"
            );
        }

        [Fact]
        public void Adds_Methods_With_Shared_Validation()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                addNullChecks: true,
                validateArguments: ArgumentValidationType.Shared,
                addMethodNameFormatString: "Add{Name}");
            var context = CreateContext(sourceModel, model, settings);

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
                "return AddProperty3(property3?.ToArray() ?? throw new System.ArgumentNullException(nameof(property3)));",
                "if (property3 is null) throw new System.ArgumentNullException(nameof(property3));",
                "if (Property3 is null) Property3 = new System.Collections.Generic.List<int>();",
                "Property3.AddRange(property3);",
                "return this;"
            );
        }

        [Fact]
        public void Adds_Method_For_BuilderForAbstractEntity()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(
                enableEntityInheritance: true,
                addMethodNameFormatString: "Add{Name}");
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().HaveCount(2);
            model.Methods.Select(x => x.Name).Should().BeEquivalentTo("AddProperty3", "AddProperty3");
            model.Methods.Select(x => x.TypeName).Should().AllBe("TBuilder");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.Name).Should().BeEquivalentTo("property3", "property3");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.TypeName).Should().BeEquivalentTo("System.Collections.Generic.IEnumerable<System.Int32>", "System.Int32[]");
            model.Methods.SelectMany(x => x.Parameters).Select(x => x.DefaultValue).Should().AllBeEquivalentTo(default(object));
            model.Methods.SelectMany(x => x.CodeStatements).Should().AllBeOfType<StringCodeStatementBuilder>();
            model.Methods.SelectMany(x => x.CodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "return AddProperty3(property3.ToArray());",
                "Property3.AddRange(property3);",
                "return (TBuilder)this;"
            );
        }

        [Theory]
        [InlineData(true, ArgumentValidationType.Shared)]
        [InlineData(false, ArgumentValidationType.Shared)]
        [InlineData(true, ArgumentValidationType.None)]
        [InlineData(false, ArgumentValidationType.None)]
        public void Returns_Error_When_Parsing_CustomBuilderArgumentType_Is_Not_Succesful(bool addNullChecks, ArgumentValidationType validateArguments)
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentType).WithValue("{Error}"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(addNullChecks: addNullChecks, validateArguments: validateArguments);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Returns_Error_When_Parsing_BuilderNameFormatString_Is_Not_Succesful()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(builderNameFormatString: "{Error}");
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Returns_Error_When_Parsing_AddMethodNameFormatString_Is_Not_Succesful()
        {
            // Arrange
            var sourceModel = CreateModel();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(addMethodNameFormatString: "{Error}");
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Returns_Error_When_Parsing_CustomBuilderArgumentNullCheckExpression_Is_Not_Succesful()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentNullCheckExpression).WithValue("{Error}"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(addNullChecks: true);
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        [Fact]
        public void Returns_Error_When_Parsing_CustomBuilderArgumentNullCheckExpression_Is_Not_Succesful_Using_Enumerable_CollectionType()
        {
            // Arrange
            var sourceModel = CreateModel(propertyMetadataBuilders: new MetadataBuilder().WithName(MetadataNames.CustomBuilderArgumentNullCheckExpression).WithValue("{Error}"));
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(addNullChecks: true, newCollectionTypeName: typeof(IEnumerable<>).WithoutGenerics());
            var context = CreateContext(sourceModel, model, settings);

            // Act
            var result = sut.Process(context);

            // Assert
            result.Status.Should().Be(ResultStatus.Error);
            result.ErrorMessage.Should().Be("Kaboom");
        }

        private static PipelineContext<IConcreteTypeBuilder, BuilderContext> CreateContext(IType sourceModel, ClassBuilder model, Pipelines.Builder.PipelineBuilderSettings settings)
            => new(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));
    }
}
