namespace ClassFramework.Pipelines.Tests.Entity.Features;

public class AddPropertiesFeatureTests : TestBase<Pipelines.Entity.Features.AddPropertiesFeature>
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
        public void Adds_Properties_From_SourceModel()
        {
            // Arrange
            var sourceModel = CreateModel();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings();
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2", "Property3");
        }

        [Fact]
        public void Maps_TypeNames_Correctly()
        {
            // Arrange
            var sourceModel = CreateModelWithCustomTypeProperties();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(namespaceMappings: new[] { new NamespaceMapping("MySourceNamespace", "MyMappedNamespace", Enumerable.Empty<Metadata>()) });
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.Select(x => x.TypeName).Should().BeEquivalentTo
            (
                "System.Int32",
                "System.Nullable<System.Int32>",
                "System.String",
                "System.String",
                "MyMappedNamespace.MyClass",
                "MyMappedNamespace.MyClass",
                "CrossCutting.Common.ValueCollection<MyMappedNamespace.MyClass>",
                "CrossCutting.Common.ValueCollection<MyMappedNamespace.MyClass>"
            );
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Adds_Setters_When_Specified_In_Settings(bool addSetters)
        {
            // Arrange
            var sourceModel = CreateModel();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(addSetters: addSetters);
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.Select(x => x.HasSetter).Should().AllBeEquivalentTo(addSetters);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(Visibility.Public)]
        [InlineData(Visibility.Internal)]
        [InlineData(Visibility.Private)]
        public void Sets_SetterVisibility_From_Settings(Visibility? setterVisibility)
        {
            // Arrange
            var sourceModel = CreateModel();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(addSetters: true, setterVisibility: setterVisibility);
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.Select(x => x.SetterVisibility).Should().AllBeEquivalentTo(setterVisibility);
        }

        [Fact]
        public void Adds_Mapped_And_Filtered_Attributes_According_To_Settings()
        {
            // Arrange
            var sourceModel = new ClassBuilder()
                .WithName("SomeClass")
                .WithNamespace("SomeNamespace")
                .AddProperties
                (
                    new ClassPropertyBuilder()
                        .WithName("MyProperty")
                        .WithType(typeof(int))
                        .AddAttributes(
                            new AttributeBuilder().WithName("MyAttribute1"),
                            new AttributeBuilder().WithName("MyAttribute2"))
                ).Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(copyAttributePredicate: a => a.Name.EndsWith('2'));
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.Attributes).Select(x => x.Name).Should().BeEquivalentTo("MyAttribute2");
        }

        [Fact]
        public void Does_Not_Add_Fields_When_AddBackingFields_Is_False()
        {
            // Arrange
            var sourceModel = CreateModel();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(addBackingFields: false);
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Fields.Should().BeEmpty();
        }

        [Fact]
        public void Does_Not_Add_Property_GetterCodeStatements_When_AddBackingFields_Is_False()
        {
            // Arrange
            var sourceModel = CreateModel();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(addBackingFields: false);
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.GetterCodeStatements).Should().BeEmpty();
        }

        [Fact]
        public void Does_Not_Add_Property_SetterCodeStatements_When_AddBackingFields_Is_False()
        {
            // Arrange
            var sourceModel = CreateModel();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(addBackingFields: false);
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.SetterCodeStatements).Should().BeEmpty();
        }

        [Fact]
        public void Adds_Fields_When_AddBackingFields_Is_True()
        {
            // Arrange
            var sourceModel = CreateModel();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(addBackingFields: true);
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Fields.Select(x => x.Name).Should().BeEquivalentTo("_property1", "_property2");
        }

        [Fact]
        public void Adds_Property_GetterCodeStatements_When_AddBackingFields_Is_True()
        {
            // Arrange
            var sourceModel = CreateModel();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(addBackingFields: true);
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.GetterCodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "return _property1;",
                "return _property2;"
            );
        }

        [Fact]
        public void Adds_Property_SetterCodeStatements_When_AddBackingFields_Is_True()
        {
            // Arrange
            var sourceModel = CreateModel();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(addBackingFields: true);
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.SetterCodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "_property1 = value;",
                "_property2 = value;"
            );
        }

        [Fact]
        public void Adds_Property_SetterCodeStatements_With_NullChecks_When_AddBackingFields_Is_True_And_AddNullChecks_Is_True()
        {
            // Arrange
            var sourceModel = CreateModel();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateEntitySettings(addBackingFields: true, addNullChecks: true);
            var context = new PipelineContext<ClassBuilder, EntityContext>(model, new EntityContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Properties.SelectMany(x => x.SetterCodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "_property1 = value;",
                "_property2 = value ?? throw new System.ArgumentNullException(nameof(value));"
            );
        }
    }
}
