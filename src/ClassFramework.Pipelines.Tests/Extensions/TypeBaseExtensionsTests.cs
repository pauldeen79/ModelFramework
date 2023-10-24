﻿namespace ClassFramework.Pipelines.Tests.Extensions;

public class TypeBaseExtensionsTests : TestBase
{
    public class IsMemberValidForImmutableBuilderClass : TypeBaseExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_ParentTypeContainer()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();

            // Act & Assert
            sut.Invoking(x => x.IsMemberValidForImmutableBuilderClass(parentTypeContainer: null!, new PipelineBuilderSettings()))
               .Should().Throw<ArgumentNullException>().WithParameterName("parentTypeContainer");
        }

        [Fact]
        public void Throws_On_Null_Settings()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();
            var parentTypeContainer = Fixture.Freeze<IParentTypeContainer>();

            // Act & Assert
            sut.Invoking(x => x.IsMemberValidForImmutableBuilderClass(parentTypeContainer, settings: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Returns_True_When_Entity_Inheritance_Is_Disabled()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();
            var parentTypeContainer = Fixture.Freeze<IParentTypeContainer>();
            var settings = new PipelineBuilderSettings(classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: false)));

            // Act
            var result = sut.IsMemberValidForImmutableBuilderClass(parentTypeContainer, settings);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Returns_Correct_Result_When_ParentTypeContainer_Is_Defined_On_TypeBase()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();
            var parentTypeContainer = Fixture.Freeze<IParentTypeContainer>();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(inheritanceComparisonDelegate: (_, _) => false),
                classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: true))
            );

            // Act
            var result = sut.IsMemberValidForImmutableBuilderClass(parentTypeContainer, settings);

            // Assert
            result.Should().BeFalse();
        }
    }

    public class GetGenericTypeArgumentsString
    {
        [Fact]
        public void Returns_Empty_String_When_No_GenericArguments_Are_Present()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();

            // Act
            var result = sut.GetGenericTypeArgumentsString();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_When_GenericArguments_Are_Present()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").AddGenericTypeArguments("T").Build();

            // Act
            var result = sut.GetGenericTypeArgumentsString();

            // Assert
            result.Should().Be("<T>");
        }
    }

    public class GetGenericTypeArgumentConstraintsString
    {
        [Fact]
        public void Returns_Empty_String_When_No_GenericArguments_Are_Present()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();

            // Act
            var result = sut.GetGenericTypeArgumentConstraintsString();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_String_When_No_GenericArgumentConstraints_Are_Present()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").AddGenericTypeArguments("T").Build();

            // Act
            var result = sut.GetGenericTypeArgumentConstraintsString();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_When_GenericArguments_Are_Present()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").AddGenericTypeArguments("T").AddGenericTypeArgumentConstraints("where T : class").Build();

            // Act
            var result = sut.GetGenericTypeArgumentConstraintsString();

            // Assert
            result.Should().Be(@"
        where T : class");
        }
    }

    public class GetCustomValueForInheritedClass
    {
        [Fact]
        public void Throws_When_Settings_Is_Null()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();

            // Act & Assert
            sut.Invoking(x => x.GetCustomValueForInheritedClass(settings: null!, _ => string.Empty))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Throws_When_CustomValue_Is_Null()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();

            // Act & Assert
            sut.Invoking(x => x.GetCustomValueForInheritedClass(new PipelineBuilderSettings(), customValue: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("customValue");
        }

        [Fact]
        public void Returns_Empty_String_When_Entity_Inheritance_Is_Disabled()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();
            var settings = new PipelineBuilderSettings(classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: false)));

            // Act
            var result = sut.GetCustomValueForInheritedClass(settings, _ => "CustomValue");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_String_When_Instance_Is_Not_A_BaseClassContainer()
        {
            // Arrange
            var sut = new StructBuilder().WithName("MyClass").Build();
            var settings = new PipelineBuilderSettings(classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: true)));

            // Act
            var result = sut.GetCustomValueForInheritedClass(settings, _ => "CustomValue");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_String_When_BaseClass_Is_Empty()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();
            var settings = new PipelineBuilderSettings(classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: true)));

            // Act
            var result = sut.GetCustomValueForInheritedClass(settings, _ => "CustomValue");

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Returns_CustomValue_When_BaseClass_Is_Not_Empty()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").WithBaseClass("SomeBaseClass").Build();
            var settings = new PipelineBuilderSettings(classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: true)));

            // Act
            var result = sut.GetCustomValueForInheritedClass(settings, _ => "CustomValue");

            // Assert
            result.Should().Be("CustomValue");
        }
    }

    public class GetImmutableBuilderConstructorProperties
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();

            // Act & Assert
            sut.Invoking(x => x.GetImmutableBuilderConstructorProperties(context: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_When_Instance_Is_Not_A_ConstructorsContainer()
        {
            // Arrange
            var sut = new InterfaceBuilder().WithName("MyClass").Build();
            var settings = new PipelineBuilderSettings();
            var context = new BuilderContext(sut, settings, CultureInfo.InvariantCulture);

            // Act & Assert
            sut.Invoking(x => x.GetImmutableBuilderConstructorProperties(context))
               .Should().Throw<ArgumentException>()
               .WithParameterName("context")
               .WithMessage("Cannot get immutable builder constructor properties for type that does not have constructors (Parameter 'context')");
        }

        [Fact]
        public void Returns_All_Properties_With_Setter_Or_Initializer_When_Type_Has_Public_Parameterless_Constructor()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass")
                .AddProperties
                (
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)).WithHasSetter(true).WithHasInitializer(false),
                    new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int)).WithHasSetter(false).WithHasInitializer(true),
                    new ClassPropertyBuilder().WithName("Property3").WithType(typeof(int)).WithHasSetter(false).WithHasInitializer(false) // this property should be skipped, because it does not have a setter or initializer
                )
                .Build();
            var settings = new PipelineBuilderSettings();
            var context = new BuilderContext(sut, settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.GetImmutableBuilderConstructorProperties(context);

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2");
        }

        [Fact]
        public void Returns_No_Properties_When_There_Is_No_Public_Constructor_With_Any_Parameters()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass")
                .AddProperties
                (
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)).WithHasSetter(true).WithHasInitializer(false),
                    new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int)).WithHasSetter(false).WithHasInitializer(true),
                    new ClassPropertyBuilder().WithName("Property3").WithType(typeof(int)).WithHasSetter(false).WithHasInitializer(false) // this property should be skipped, because it does not have a setter or initializer
                )
                .AddConstructors(new ClassConstructorBuilder().WithVisibility(Visibility.Private)) // only private constructor present :)
                .Build();
            var settings = new PipelineBuilderSettings();
            var context = new BuilderContext(sut, settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.GetImmutableBuilderConstructorProperties(context);

            // Assert
            result.Select(x => x.Name).Should().BeEmpty();
        }

        [Fact]
        public void Returns_Properties_From_Instance_And_BaseClass_When_IsBuilderForOverrideEntity_Is_True_And_BaseClass_Is_Filled()
        {
            var sut = new ClassBuilder().WithName("MyClass")
                .AddProperties(new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)))
                .AddConstructors(new ClassConstructorBuilder()
                    .AddParameter("property1", typeof(int))
                    .AddParameter("property2", typeof(int))
                )
                .Build();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: new ClassBuilder().WithName("MyBaseClass").AddProperties(new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int))).BuildTyped()),
                classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: true))
            );
            var context = new BuilderContext(sut, settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.GetImmutableBuilderConstructorProperties(context);

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2");
        }

        [Fact]
        public void Returns_Properties_From_Instance_When_IsBuilderForOverrideEntity_Is_True_But_BaseClass_Is_Not_Filled()
        {
            var sut = new ClassBuilder().WithName("MyClass")
                .AddProperties(
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)),
                    new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int))
                )
                .AddConstructors(new ClassConstructorBuilder()
                    .AddParameter("property1", typeof(int))
                    .AddParameter("property2", typeof(int))
                )
                .Build();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: null),
                classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: true))
            );
            var context = new BuilderContext(sut, settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.GetImmutableBuilderConstructorProperties(context);

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2");
        }

        [Fact]
        public void Returns_Properties_From_Instance_When_IsBuilderForOverrideEntity_Is_False()
        {
            var sut = new ClassBuilder().WithName("MyClass")
                .AddProperties(
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)),
                    new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int))
                )
                .AddConstructors(new ClassConstructorBuilder()
                    .AddParameter("property1", typeof(int))
                    .AddParameter("property2", typeof(int))
                )
                .Build();
            var settings = new PipelineBuilderSettings(
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: false, baseClass: null),
                classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: true))
            );
            var context = new BuilderContext(sut, settings, CultureInfo.InvariantCulture);

            // Act
            var result = sut.GetImmutableBuilderConstructorProperties(context);

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property2");
        }
    }

    public class GetImmutableBuilderClassFields : TypeBaseExtensionsTests
    {
        [Fact]
        public void Throws_On_Null_Context()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act & Assert
            sut.Invoking(x => x.GetImmutableBuilderClassFields(context: null!, formattableStringParser).ToArray())
               .Should().Throw<ArgumentNullException>().WithParameterName("context");
        }

        [Fact]
        public void Throws_On_Null_FormattableStringParser()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();
            var model = new ClassBuilder();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sut, new PipelineBuilderSettings(), CultureInfo.InvariantCulture));

            // Act & Assert
            sut.Invoking(x => x.GetImmutableBuilderClassFields(context, formattableStringParser: null!).ToArray())
               .Should().Throw<ArgumentNullException>().WithParameterName("formattableStringParser");
        }

        [Fact]
        public void Returns_Empty_Sequence_For_Abstract_Builder()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass")
                .AddProperties
                (
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)),
                    new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int)),
                    new ClassPropertyBuilder().WithName("Property3").WithType(typeof(int))
                )
                .Build();
            var settings = new PipelineBuilderSettings(
                generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true),
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: null),
                classSettings: new EntityPipelineBuilderSettings(constructorSettings: new EntityPipelineBuilderConstructorSettings(validateArguments: ArgumentValidationType.DomainOnly))
            );
            var model = new ClassBuilder();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sut, settings, CultureInfo.InvariantCulture));
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = sut.GetImmutableBuilderClassFields(context, formattableStringParser);

            // Assert
            result.Select(x => x.Name).Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_Sequence_When_Not_Using_NullChecks()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass")
                .AddProperties
                (
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)),
                    new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int)),
                    new ClassPropertyBuilder().WithName("Property3").WithType(typeof(int))
                )
                .Build();
            var settings = new PipelineBuilderSettings(
                generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: false),
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: new ClassBuilder().WithName("MyBaseClass").BuildTyped()),
                classSettings: new EntityPipelineBuilderSettings(constructorSettings: new EntityPipelineBuilderConstructorSettings(validateArguments: ArgumentValidationType.DomainOnly))
            );
            var model = new ClassBuilder();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sut, settings, CultureInfo.InvariantCulture));
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = sut.GetImmutableBuilderClassFields(context, formattableStringParser);

            // Assert
            result.Select(x => x.Name).Should().BeEmpty();
        }

        [Fact]
        public void Returns_Empty_Sequence_When_OriginalValidateArguments_Is_Shared()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass")
                .AddProperties
                (
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)),
                    new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int)),
                    new ClassPropertyBuilder().WithName("Property3").WithType(typeof(int))
                )
                .Build();
            var settings = new PipelineBuilderSettings(
                generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true),
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: new ClassBuilder().WithName("MyBaseClass").BuildTyped()),
                classSettings: new EntityPipelineBuilderSettings(constructorSettings: new EntityPipelineBuilderConstructorSettings(validateArguments: ArgumentValidationType.Shared))
            );
            var model = new ClassBuilder();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sut, settings, CultureInfo.InvariantCulture));
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = sut.GetImmutableBuilderClassFields(context, formattableStringParser);

            // Assert
            result.Select(x => x.Name).Should().BeEmpty();
        }

        [Fact]
        public void Returns_Correct_Result_On_NonAbstract_Builder_With_NullChecks_And_NonShared_OriginalValidateArguments()
        {
            // Arrange
            InitializeParser();
            var sut = new ClassBuilder().WithName("MyClass")
                .AddProperties
                (
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)),
                    new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int)),
                    new ClassPropertyBuilder().WithName("Property3").WithType(typeof(int))
                )
                .Build();
            var settings = new PipelineBuilderSettings(
                generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true),
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: new ClassBuilder().WithName("MyBaseClass").BuildTyped()),
                classSettings: new EntityPipelineBuilderSettings(constructorSettings: new EntityPipelineBuilderConstructorSettings(validateArguments: ArgumentValidationType.DomainOnly))
            );
            var model = new ClassBuilder();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sut, settings, CultureInfo.InvariantCulture));
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = sut.GetImmutableBuilderClassFields(context, formattableStringParser);

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("_property1", "_property2", "_property3");
            result.Select(x => x.TypeName).Should().AllBe(typeof(int).FullName);
            result.Select(x => x.IsValueType).Should().AllBeEquivalentTo(true);
        }

        [Fact]
        public void Returns_Correct_Result_On_NonAbstract_Builder_With_NullChecks_And_NonShared_OriginalValidateArguments_And_CustomBuilderArgumentType()
        {
            // Arrange
            InitializeParser();
            var sut = new ClassBuilder().WithName("MyClass")
                .AddProperties
                (
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)).AddMetadata(MetadataNames.CustomBuilderArgumentType, "MyCustomType"),
                    new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int)),
                    new ClassPropertyBuilder().WithName("Property3").WithType(typeof(int))
                )
                .Build();
            var settings = new PipelineBuilderSettings(
                generationSettings: new PipelineBuilderGenerationSettings(addNullChecks: true),
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: new ClassBuilder().WithName("MyBaseClass").BuildTyped()),
                classSettings: new EntityPipelineBuilderSettings(constructorSettings: new EntityPipelineBuilderConstructorSettings(validateArguments: ArgumentValidationType.DomainOnly))
            );
            var model = new ClassBuilder();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sut, settings, CultureInfo.InvariantCulture));
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = sut.GetImmutableBuilderClassFields(context, formattableStringParser);

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("_property1", "_property2", "_property3");
            result.Select(x => x.TypeName).Should().BeEquivalentTo("MyCustomType", typeof(int).FullName, typeof(int).FullName);
            result.Select(x => x.IsValueType).Should().AllBeEquivalentTo(true);
        }
    }

    public class GetPropertiesFromClassAndBaseClass
    {
        [Fact]
        public void Throws_On_Null_Settings()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass").Build();

            // Act & Assert
            sut.Invoking(x => x.GetPropertiesFromClassAndBaseClass(settings: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("settings");
        }

        [Fact]
        public void Returns_Valid_Properties_From_Instance()
        {
            // Arrange
            var sut = new ClassBuilder().WithName("MyClass")
                .AddProperties(
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)).WithParentTypeFullName("1"),
                    new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int)).WithParentTypeFullName("2"),
                    new ClassPropertyBuilder().WithName("Property3").WithType(typeof(int)).WithParentTypeFullName("1"))
                .Build();
            var settings = new PipelineBuilderSettings(
                classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: true)),
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: null, inheritanceComparisonDelegate: (parent, type) => parent.ParentTypeFullName == "1")
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
            var sut = new ClassBuilder().WithName("MyClass")
                .AddProperties(
                    new ClassPropertyBuilder().WithName("Property1").WithType(typeof(int)).WithParentTypeFullName("1"),
                    new ClassPropertyBuilder().WithName("Property2").WithType(typeof(int)).WithParentTypeFullName("2"),
                    new ClassPropertyBuilder().WithName("Property3").WithType(typeof(int)).WithParentTypeFullName("1"))
                .Build();
            var settings = new PipelineBuilderSettings(
                classSettings: new EntityPipelineBuilderSettings(inheritanceSettings: new EntityPipelineBuilderInheritanceSettings(enableInheritance: true)),
                inheritanceSettings: new PipelineBuilderInheritanceSettings(enableBuilderInheritance: true, baseClass: new ClassBuilder().WithName("MyBaseClassBuilder").AddProperties(new ClassPropertyBuilder().WithName("Property4").WithType(typeof(int)).WithParentTypeFullName("3")).BuildTyped(), inheritanceComparisonDelegate: (parent, type) => parent.ParentTypeFullName == "1" || parent.ParentTypeFullName == "3")
            );

            // Act
            var result = sut.GetPropertiesFromClassAndBaseClass(settings).ToArray();

            // Assert
            result.Select(x => x.Name).Should().BeEquivalentTo("Property1", "Property3", "Property4");
        }
    }
}