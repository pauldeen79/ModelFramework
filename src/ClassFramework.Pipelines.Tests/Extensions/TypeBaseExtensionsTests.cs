﻿namespace ClassFramework.Pipelines.Tests.Extensions;

public class TypeBaseExtensionsTests : TestBase<ClassBuilder>
{
    public class IsMemberValidForBuilderClass : TypeBaseExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_ParentTypeContainer()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();

            // Act & Assert
            sut.Invoking(x => x.IsMemberValidForBuilderClass(parentTypeContainer: null!, CreateBuilderSettings()))
               .Should().Throw<ArgumentNullException>().WithParameterName("parentTypeContainer");
        }

        [Fact]
        public void Throws_On_Null_Settings()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();
            var parentTypeContainer = Fixture.Freeze<IParentTypeContainer>();

            // Act & Assert
            sut.Invoking(x => x.IsMemberValidForBuilderClass(parentTypeContainer, settings: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Returns_True_When_Entity_Inheritance_Is_Disabled()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();
            var parentTypeContainer = Fixture.Freeze<IParentTypeContainer>();
            var settings = CreateBuilderSettings(enableBuilderInheritance: false);

            // Act
            var result = sut.IsMemberValidForBuilderClass(parentTypeContainer, settings);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_Correct_Result_When_ParentTypeContainer_Is_Defined_On_TypeBase()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();
            var parentTypeContainer = Fixture.Freeze<IParentTypeContainer>();
            var settings = CreateBuilderSettings(
                inheritanceComparisonDelegate: (_, _) => false,
                enableEntityInheritance: true
            );

            // Act
            var result = sut.IsMemberValidForBuilderClass(parentTypeContainer, settings);

            // Assert
            result.Should().BeFalse();
        }
    }

    public class GetGenericTypeArgumentConstraintsString : TypeBaseExtensionsTests
    {
        [Fact]
        public void Returns_Empty_String_When_No_GenericArguments_Are_Present()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();

            // Act
            var result = sut.GetGenericTypeArgumentConstraintsString();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_String_When_No_GenericArgumentConstraints_Are_Present()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").AddGenericTypeArguments("T").Build();

            // Act
            var result = sut.GetGenericTypeArgumentConstraintsString();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_When_GenericArguments_Are_Present()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").AddGenericTypeArguments("T").AddGenericTypeArgumentConstraints("where T : class").Build();

            // Act
            var result = sut.GetGenericTypeArgumentConstraintsString();

            // Assert
            result.Should().Be(@"
        where T : class");
        }
    }

    public class GetCustomValueForInheritedClass : TypeBaseExtensionsTests
    {
        [Fact]
        public void Throws_When_Settings_Is_Null()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();

            // Act & Assert
            sut.Invoking(x => x.GetCustomValueForInheritedClass(settings: null!, _ => Result.Success(string.Empty)))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Throws_When_CustomValue_Is_Null()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();

            // Act & Assert
            sut.Invoking(x => x.GetCustomValueForInheritedClass(new Pipelines.Entity.PipelineBuilderSettings(), customValue: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("customValue");
        }

        [Fact]
        public void Returns_Empty_String_When_Entity_Inheritance_Is_Disabled()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();
            var settings = new Pipelines.Entity.PipelineBuilderSettings(inheritanceSettings: new Pipelines.Entity.PipelineBuilderInheritanceSettings(enableInheritance: false));

            // Act
            var result = sut.GetCustomValueForInheritedClass(settings, _ => Result.Success("CustomValue"));

            // Assert
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_String_When_Instance_Is_Not_A_BaseClassContainer()
        {
            // Arrange
            var sut = new StructBuilder().WithName("MyClass").Build();
            var settings = new Pipelines.Entity.PipelineBuilderSettings(inheritanceSettings: new Pipelines.Entity.PipelineBuilderInheritanceSettings(enableInheritance: true));

            // Act
            var result = sut.GetCustomValueForInheritedClass(settings, _ => Result.Success("CustomValue"));

            // Assert
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_String_When_BaseClass_Is_Empty()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();
            var settings = new Pipelines.Entity.PipelineBuilderSettings(inheritanceSettings: new Pipelines.Entity.PipelineBuilderInheritanceSettings(enableInheritance: true));

            // Act
            var result = sut.GetCustomValueForInheritedClass(settings, _ => Result.Success("CustomValue"));

            // Assert
            result.Value.Should().BeEmpty();
        }

        [Fact]
        public void Returns_CustomValue_When_BaseClass_Is_Not_Empty()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").WithBaseClass("SomeBaseClass").Build();
            var settings = new Pipelines.Entity.PipelineBuilderSettings(inheritanceSettings: new Pipelines.Entity.PipelineBuilderInheritanceSettings(enableInheritance: true));

            // Act
            var result = sut.GetCustomValueForInheritedClass(settings, _ => Result.Success("CustomValue"));

            // Assert
            result.Value.Should().Be("CustomValue");
        }
    }

    public class GetBuilderConstructorProperties : TypeBaseExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderConstructorProperties(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_When_Instance_Is_Not_A_ConstructorsContainer()
        {
            // Arrange
            var sut = new InterfaceBuilder().WithName("MyClass").Build();
            var settings = CreateBuilderSettings();
            var context = new BuilderContext(sut, settings, CultureInfo.InvariantCulture);

            // Act & Assert
            sut.Invoking(x => x.GetBuilderConstructorProperties(context))
               .Should().Throw<ArgumentException>()
               .WithParameterName("context")
               .WithMessage("Cannot get immutable builder constructor properties for type that does not have constructors (Parameter 'context')");
        }

        [Fact]
        public void Returns_All_Properties_With_Setter_Or_Initializer_When_Type_Has_Public_Parameterless_Constructor()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass")
                .AddProperties
                (
                    new PropertyBuilder().WithName("Property1").WithType(typeof(int)).WithHasSetter(true).WithHasInitializer(false),
                    new PropertyBuilder().WithName("Property2").WithType(typeof(int)).WithHasSetter(false).WithHasInitializer(true),
                    new PropertyBuilder().WithName("Property3").WithType(typeof(int)).WithHasSetter(false).WithHasInitializer(false) // this property should be skipped, because it does not have a setter or initializer
                )
                .Build();
            var settings = CreateBuilderSettings();
            var context = new BuilderContext(sut, settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.GetBuilderConstructorProperties(context);

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2");
        }

        [Fact]
        public void Returns_No_Properties_When_There_Is_No_Public_Constructor_With_Any_Parameters()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass")
                .AddProperties
                (
                    new PropertyBuilder().WithName("Property1").WithType(typeof(int)).WithHasSetter(true).WithHasInitializer(false),
                    new PropertyBuilder().WithName("Property2").WithType(typeof(int)).WithHasSetter(false).WithHasInitializer(true),
                    new PropertyBuilder().WithName("Property3").WithType(typeof(int)).WithHasSetter(false).WithHasInitializer(false) // this property should be skipped, because it does not have a setter or initializer
                )
                .AddConstructors(new ConstructorBuilder().WithVisibility(Visibility.Private)) // only private constructor present :)
                .Build();
            var settings = CreateBuilderSettings();
            var context = new BuilderContext(sut, settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.GetBuilderConstructorProperties(context);

            // Assert
            result.Select(x => x.Name).Should().BeEmpty();
        }

        [Fact]
        public void Returns_Properties_From_Instance_And_BaseClass_When_IsBuilderForOverrideEntity_Is_True_And_BaseClass_Is_Filled()
        {
            var sut = CreateSut().WithName("MyClass")
                .AddProperties(new PropertyBuilder().WithName("Property1").WithType(typeof(int)))
                .AddConstructors(new ConstructorBuilder()
                    .AddParameter("property1", typeof(int))
                    .AddParameter("property2", typeof(int))
                )
                .Build();
            var settings = CreateBuilderSettings(
                enableBuilderInheritance: true,
                baseClass: new ClassBuilder().WithName("MyBaseClass").AddProperties(new PropertyBuilder().WithName("Property2").WithType(typeof(int))).BuildTyped(),
                enableEntityInheritance: true
            );
            var context = new BuilderContext(sut, settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.GetBuilderConstructorProperties(context);

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2");
        }

        [Fact]
        public void Returns_Properties_From_Instance_When_IsBuilderForOverrideEntity_Is_True_But_BaseClass_Is_Not_Filled()
        {
            var sut = CreateSut().WithName("MyClass")
                .AddProperties(
                    new PropertyBuilder().WithName("Property1").WithType(typeof(int)),
                    new PropertyBuilder().WithName("Property2").WithType(typeof(int))
                )
                .AddConstructors(new ConstructorBuilder()
                    .AddParameter("property1", typeof(int))
                    .AddParameter("property2", typeof(int))
                )
                .Build();
            var settings = CreateBuilderSettings(
                enableBuilderInheritance: true,
                baseClass: null,
                enableEntityInheritance: true
            );
            var context = new BuilderContext(sut, settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.GetBuilderConstructorProperties(context);

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2");
        }

        [Fact]
        public void Returns_Properties_From_Instance_When_IsBuilderForOverrideEntity_Is_False()
        {
            var sut = CreateSut().WithName("MyClass")
                .AddProperties(
                    new PropertyBuilder().WithName("Property1").WithType(typeof(int)),
                    new PropertyBuilder().WithName("Property2").WithType(typeof(int))
                )
                .AddConstructors(new ConstructorBuilder()
                    .AddParameter("property1", typeof(int))
                    .AddParameter("property2", typeof(int))
                )
                .Build();
            var settings = CreateBuilderSettings(
                enableBuilderInheritance: false,
                baseClass: null,
                enableEntityInheritance: true
            );
            var context = new BuilderContext(sut, settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.GetBuilderConstructorProperties(context);

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2");
        }
    }

    public class GetBuilderClassFields : TypeBaseExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act & Assert
            sut.Invoking(x => x.GetBuilderClassFields(context: null!, formattableStringParser).ToArray())
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_On_Null_FormattableStringParser()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();
            var model = new ClassBuilder();
            var context = new PipelineContext<IConcreteTypeBuilder, BuilderContext>(model, new BuilderContext(sut, CreateBuilderSettings(), CultureInfo.InvariantCulture));

            // Act & Assert
            sut.Invoking(x => x.GetBuilderClassFields(context, formattableStringParser: null!).ToArray())
               .Should().Throw<ArgumentNullException>().WithParameterName("formattableStringParser");
        }

        [Fact]
        public void Returns_Empty_Sequence_For_Abstract_Builder()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass")
                .AddProperties
                (
                    new PropertyBuilder().WithName("Property1").WithType(typeof(int)),
                    new PropertyBuilder().WithName("Property2").WithType(typeof(int)),
                    new PropertyBuilder().WithName("Property3").WithType(typeof(int))
                )
                .Build();
            var settings = CreateBuilderSettings(
                addNullChecks: true,
                enableBuilderInheritance: true,
                baseClass: null,
                validateArguments: ArgumentValidationType.DomainOnly
            );
            var model = new ClassBuilder();
            var context = new PipelineContext<IConcreteTypeBuilder, BuilderContext>(model, new BuilderContext(sut, settings, CultureInfo.InvariantCulture));
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = sut.GetBuilderClassFields(context, formattableStringParser);

            // Assert
            result.Select(x => x.Value!.Name).Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_Sequence_When_Not_Using_NullChecks()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass")
                .AddProperties
                (
                    new PropertyBuilder().WithName("Property1").WithType(typeof(int)),
                    new PropertyBuilder().WithName("Property2").WithType(typeof(int)),
                    new PropertyBuilder().WithName("Property3").WithType(typeof(int))
                )
                .Build();
            var settings = CreateBuilderSettings(
                addNullChecks: false,
                enableBuilderInheritance: true,
                baseClass: new ClassBuilder().WithName("MyBaseClass").BuildTyped(),
                validateArguments: ArgumentValidationType.DomainOnly
            );
            var model = new ClassBuilder();
            var context = new PipelineContext<IConcreteTypeBuilder, BuilderContext>(model, new BuilderContext(sut, settings, CultureInfo.InvariantCulture));
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = sut.GetBuilderClassFields(context, formattableStringParser);

            // Assert
            result.Select(x => x.Value!.Name).Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_Sequence_When_OriginalValidateArguments_Is_Shared()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass")
                .AddProperties
                (
                    new PropertyBuilder().WithName("Property1").WithType(typeof(int)),
                    new PropertyBuilder().WithName("Property2").WithType(typeof(int)),
                    new PropertyBuilder().WithName("Property3").WithType(typeof(int))
                )
                .Build();
            var settings = CreateBuilderSettings(
                addNullChecks: true,
                enableBuilderInheritance: true,
                baseClass: new ClassBuilder().WithName("MyBaseClass").BuildTyped(),
                validateArguments: ArgumentValidationType.Shared
            );
            var model = new ClassBuilder();
            var context = new PipelineContext<IConcreteTypeBuilder, BuilderContext>(model, new BuilderContext(sut, settings, CultureInfo.InvariantCulture));
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = sut.GetBuilderClassFields(context, formattableStringParser);

            // Assert
            result.Select(x => x.Value!.Name).Should().BeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_On_NonAbstract_Builder_With_NullChecks_And_NonShared_OriginalValidateArguments()
        {
            // Arrange
            InitializeParser();
            var sut = CreateSut().WithName("MyClass")
                .AddProperties
                (
                    new PropertyBuilder().WithName("Property1").WithType(typeof(string)),
                    new PropertyBuilder().WithName("Property2").WithType(typeof(string)).WithIsNullable()
                )
                .Build();
            var settings = CreateBuilderSettings(
                addNullChecks: true,
                enableNullableReferenceTypes: true,
                enableBuilderInheritance: true,
                baseClass: new ClassBuilder().WithName("MyBaseClass").BuildTyped(),
                validateArguments: ArgumentValidationType.DomainOnly
            );
            var model = new ClassBuilder();
            var context = new PipelineContext<IConcreteTypeBuilder, BuilderContext>(model, new BuilderContext(sut, settings, CultureInfo.InvariantCulture));
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = sut.GetBuilderClassFields(context, formattableStringParser);

            // Assert
            result.Select(x => x.Value!.Name).Should().BeEquivalentTo("_property1");
            result.Select(x => x.Value!.TypeName).Should().AllBe(typeof(string).FullName);
            result.Select(x => x.Value!.IsValueType).Should().AllBeEquivalentTo(false);
        }

        [Fact]
        public void Returns_Correct_Result_On_NonAbstract_Builder_With_NullChecks_And_NonShared_OriginalValidateArguments_And_CustomBuilderArgumentType()
        {
            // Arrange
            InitializeParser();
            var sut = CreateSut().WithName("MyClass")
                .AddProperties
                (
                    new PropertyBuilder().WithName("Property1").WithType(typeof(string)).AddMetadata(MetadataNames.CustomBuilderArgumentType, "MyCustomType"),
                    new PropertyBuilder().WithName("Property2").WithType(typeof(string)),
                    new PropertyBuilder().WithName("Property3").WithType(typeof(string)).WithIsNullable()
                )
                .Build();
            var settings = CreateBuilderSettings(
                addNullChecks: true,
                enableNullableReferenceTypes: true,
                enableBuilderInheritance: true,
                baseClass: new ClassBuilder().WithName("MyBaseClass").BuildTyped(),
                validateArguments: ArgumentValidationType.DomainOnly
            );
            var model = new ClassBuilder();
            var context = new PipelineContext<IConcreteTypeBuilder, BuilderContext>(model, new BuilderContext(sut, settings, CultureInfo.InvariantCulture));
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = sut.GetBuilderClassFields(context, formattableStringParser);

            // Assert
            result.Select(x => x.Value!.Name).Should().BeEquivalentTo("_property1", "_property2");
            result.Select(x => x.Value!.TypeName).Should().BeEquivalentTo("MyCustomType", typeof(string).FullName);
            result.Select(x => x.Value!.IsValueType).Should().AllBeEquivalentTo(false);
        }
    }

    public class GetPropertiesFromClassAndBaseClass : TypeBaseExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_Settings()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass").Build();

            // Act & Assert
            sut.Invoking(x => x.GetPropertiesFromClassAndBaseClass(settings: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Returns_Valid_Properties_From_Instance()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass")
                .AddProperties(
                    new PropertyBuilder().WithName("Property1").WithType(typeof(int)).WithParentTypeFullName("1"),
                    new PropertyBuilder().WithName("Property2").WithType(typeof(int)).WithParentTypeFullName("2"),
                    new PropertyBuilder().WithName("Property3").WithType(typeof(int)).WithParentTypeFullName("1"))
                .Build();
            var settings = CreateBuilderSettings(
                enableEntityInheritance: true,
                enableBuilderInheritance: true,
                baseClass: null,
                inheritanceComparisonDelegate: (parent, type) => parent.ParentTypeFullName == "1"
            );

            // Act
            var result = sut.GetPropertiesFromClassAndBaseClass(settings).ToArray();

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property3");
        }

        [Fact]
        public void Returns_Merged_Properties_From_Instance_And_BaseClass_When_Present()
        {
            // Arrange
            var sut = CreateSut().WithName("MyClass")
                .AddProperties(
                    new PropertyBuilder().WithName("Property1").WithType(typeof(int)).WithParentTypeFullName("1"),
                    new PropertyBuilder().WithName("Property2").WithType(typeof(int)).WithParentTypeFullName("2"),
                    new PropertyBuilder().WithName("Property3").WithType(typeof(int)).WithParentTypeFullName("1"))
                .Build();
            var settings = CreateBuilderSettings(
                enableEntityInheritance: true,
                enableBuilderInheritance: true,
                baseClass: new ClassBuilder().WithName("MyBaseClassBuilder").AddProperties(new PropertyBuilder().WithName("Property4").WithType(typeof(int)).WithParentTypeFullName("3")).BuildTyped(),
                inheritanceComparisonDelegate: (parent, type) => parent.ParentTypeFullName == "1" || parent.ParentTypeFullName == "3"
            );

            // Act
            var result = sut.GetPropertiesFromClassAndBaseClass(settings).ToArray();

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property3", "Property4");
        }
    }
}
