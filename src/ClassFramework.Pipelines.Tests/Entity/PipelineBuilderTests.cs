﻿namespace ClassFramework.Pipelines.Tests.Entity;

public class PipelineBuilderTests : IntegrationTestBase<IPipelineBuilder<IConcreteTypeBuilder, EntityContext>>
{
    public class Process : PipelineBuilderTests
    {
        private EntityContext CreateContext(bool addProperties = true) => new EntityContext
        (
            CreateGenericModel(addProperties),
            CreateEntitySettings
            (
                allowGenerationWithoutProperties: false
            ),
            CultureInfo.InvariantCulture
        );

        private ClassBuilder Model { get; } = new();

        [Fact]
        public void Sets_Partial()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().NotBeNull();
            result.Value!.Partial.Should().BeTrue();
        }

        [Fact]
        public void Sets_Namespace_And_Name_According_To_Settings()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext());

            // Assert
            result.Status.Should().Be(ResultStatus.Ok);
            result.Value.Should().NotBeNull();
            result.Value!.Name.Should().Be("MyClass");
            result.Value.Namespace.Should().Be("MyNamespace");
        }

        [Fact]
        public void Returns_Invalid_When_SourceModel_Does_Not_Have_Properties_And_AllowGenerationWithoutProperties_Is_False()
        {
            // Arrange
            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, CreateContext(addProperties: false));

            // Assert
            result.Status.Should().Be(ResultStatus.Invalid);
            result.ErrorMessage.Should().Be("To create an entity class, there must be at least one property");
        }
    }

    public class IntegrationTests : PipelineBuilderTests
    {
        private ClassBuilder Model { get; } = new();

        [Fact]
        public void Creates_ReadOnly_Entity_With_NamespaceMapping()
        {
            // Arrange
            var model = CreateModelWithCustomTypeProperties();
            var namespaceMappings = CreateNamespaceMappings();
            var settings = CreateEntitySettings(
                namespaceMappings: namespaceMappings,
                addNullChecks: true,
                enableNullableReferenceTypes: true,
                newCollectionTypeName: typeof(IReadOnlyCollection<>).WithoutGenerics(),
                collectionTypeName: typeof(ReadOnlyValueCollection<>).WithoutGenerics());
            var context = CreateContext(model, settings);

            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            result.Value.Should().NotBeNull();

            result.Value!.Name.Should().Be("MyClass");
            result.Value.Namespace.Should().Be("MyNamespace");
            result.Value.Interfaces.Should().BeEmpty();

            var ctors = (result.Value as IConstructorsContainerBuilder)?.Constructors;
            ctors.Should().ContainSingle();
            var copyConstructor = ctors!.Single();
            copyConstructor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            copyConstructor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "if (property3 is null) throw new System.ArgumentNullException(nameof(property3));",
                "if (property5 is null) throw new System.ArgumentNullException(nameof(property5));",
                "if (property7 is null) throw new System.ArgumentNullException(nameof(property7));",
                "this.Property1 = property1;",
                "this.Property2 = property2;",
                "this.Property3 = property3;",
                "this.Property4 = property4;",
                "this.Property5 = property5;",
                "this.Property6 = property6;",
                "this.Property7 = new CrossCutting.Common.ReadOnlyValueCollection<MyNamespace.MyClass>(property7);",
                "this.Property8 = property8 is null ? null : new CrossCutting.Common.ReadOnlyValueCollection<MyNamespace.MyClass>(property8);"
            );

            result.Value.Fields.Should().BeEmpty();

            result.Value.Properties.Select(x => x.Name).Should().BeEquivalentTo
            (
                "Property1",
                "Property2",
                "Property3",
                "Property4",
                "Property5",
                "Property6",
                "Property7",
                "Property8"
            );
            result.Value.Properties.Select(x => x.TypeName).Should().BeEquivalentTo
            (
                "System.Int32",
                "System.Nullable<System.Int32>",
                "System.String",
                "System.String",
                "MyNamespace.MyClass",
                "MyNamespace.MyClass",
                "System.Collections.Generic.IReadOnlyCollection<MyNamespace.MyClass>",
                "System.Collections.Generic.IReadOnlyCollection<MyNamespace.MyClass>"
            );
            result.Value.Properties.Select(x => x.IsNullable).Should().BeEquivalentTo
            (
                new[]
                {
                    false,
                    true,
                    false,
                    true,
                    false,
                    true,
                    false,
                    true
                }
            );
            result.Value.Properties.Select(x => x.HasGetter).Should().AllBeEquivalentTo(true);
            result.Value.Properties.SelectMany(x => x.GetterCodeStatements).Should().BeEmpty();
            result.Value.Properties.Select(x => x.HasInitializer).Should().AllBeEquivalentTo(false);
            result.Value.Properties.Select(x => x.HasSetter).Should().AllBeEquivalentTo(false);
            result.Value.Properties.SelectMany(x => x.SetterCodeStatements).Should().BeEmpty();
        }

        [Fact]
        public void Creates_Observable_Entity_With_NamespaceMapping()
        {
            // Arrange
            var model = CreateModelWithCustomTypeProperties();
            var namespaceMappings = CreateNamespaceMappings();
            var settings = CreateEntitySettings(
                namespaceMappings: namespaceMappings,
                addNullChecks: true,
                enableNullableReferenceTypes: true,
                addBackingFields: true,
                createAsObservable: true,
                newCollectionTypeName: typeof(ObservableCollection<>).WithoutGenerics(),
                collectionTypeName: typeof(ObservableValueCollection<>).WithoutGenerics());
            var context = CreateContext(model, settings);

            var sut = CreateSut().Build();

            // Act
            var result = sut.Process(Model, context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            result.Value.Should().NotBeNull();

            result.Value!.Name.Should().Be("MyClass");
            result.Value.Namespace.Should().Be("MyNamespace");
            result.Value.Interfaces.Should().BeEquivalentTo("System.ComponentModel.INotifyPropertyChanged");

            var ctors = (result.Value as IConstructorsContainerBuilder)?.Constructors;
            ctors.Should().ContainSingle();
            var copyConstructor = ctors!.Single();
            copyConstructor.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            copyConstructor.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "if (property3 is null) throw new System.ArgumentNullException(nameof(property3));",
                "if (property5 is null) throw new System.ArgumentNullException(nameof(property5));",
                "if (property7 is null) throw new System.ArgumentNullException(nameof(property7));",
                "this._property1 = property1;",
                "this._property2 = property2;",
                "this._property3 = property3;",
                "this._property4 = property4;",
                "this._property5 = property5;",
                "this._property6 = property6;",
                "this.Property7 = new CrossCutting.Common.ObservableValueCollection<MyNamespace.MyClass>(property7);",
                "this.Property8 = property8 is null ? null : new CrossCutting.Common.ObservableValueCollection<MyNamespace.MyClass>(property8);"
            );

            // non collection type properties have a backing field, so we can implement INotifyPropertyChanged
            result.Value.Fields.Select(x => x.Name).Should().BeEquivalentTo
            (
                "_property1",
                "_property2",
                "_property3",
                "_property4",
                "_property5",
                "_property6",
                "PropertyChanged"
            );
            result.Value.Fields.Select(x => x.TypeName).Should().BeEquivalentTo
            (
                "System.Int32",
                "System.Nullable<System.Int32>",
                "System.String",
                "System.String",
                "MyNamespace.MyClass",
                "MyNamespace.MyClass",
                "System.ComponentModel.PropertyChangedEventHandler"
            );
            result.Value.Fields.Select(x => x.IsNullable).Should().BeEquivalentTo
            (
                new[]
                {
                    false,
                    true,
                    false,
                    true,
                    false,
                    true,
                    true
                }
            );

            result.Value.Properties.Select(x => x.Name).Should().BeEquivalentTo
            (
                "Property1",
                "Property2",
                "Property3",
                "Property4",
                "Property5",
                "Property6",
                "Property7",
                "Property8"
            );
            result.Value.Properties.Select(x => x.TypeName).Should().BeEquivalentTo
            (
                "System.Int32",
                "System.Nullable<System.Int32>",
                "System.String",
                "System.String",
                "MyNamespace.MyClass",
                "MyNamespace.MyClass",
                "System.Collections.ObjectModel.ObservableCollection<MyNamespace.MyClass>",
                "System.Collections.ObjectModel.ObservableCollection<MyNamespace.MyClass>"
            );
            result.Value.Properties.Select(x => x.IsNullable).Should().BeEquivalentTo
            (
                new[]
                {
                    false,
                    true,
                    false,
                    true,
                    false,
                    true,
                    false,
                    true
                }
            );
            result.Value.Properties.Select(x => x.HasGetter).Should().AllBeEquivalentTo(true);
            result.Value.Properties.SelectMany(x => x.GetterCodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "return _property1;",
                "return _property2;",
                "return _property3;",
                "return _property4;",
                "return _property5;",
                "return _property6;"
            );
            result.Value.Properties.Select(x => x.HasInitializer).Should().AllBeEquivalentTo(false);
            result.Value.Properties.Select(x => x.HasSetter).Should().BeEquivalentTo
            (
                new[]
                {
                    true,
                    true,
                    true,
                    true,
                    true,
                    true,
                    false,
                    false
                }
            );
            result.Value.Properties.SelectMany(x => x.SetterCodeStatements).OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "_property1 = value;",
                "PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Property1)));",
                "_property2 = value;",
                "PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Property2)));",
                "_property3 = value ?? throw new System.ArgumentNullException(nameof(value));",
                "PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Property3)));",
                "_property4 = value;",
                "PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Property4)));",
                "_property5 = value ?? throw new System.ArgumentNullException(nameof(value));",
                "PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Property5)));",
                "_property6 = value;",
                "PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof(Property6)));"
            );
        }

        private static EntityContext CreateContext(IConcreteType model, Pipelines.Entity.PipelineSettings settings)
            => new(model, settings, CultureInfo.InvariantCulture);
    }
}
