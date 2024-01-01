﻿namespace ClassFramework.Pipelines.Tests.Reflection.Features;

public class SetBaseClassFeatureTests : TestBase<Pipelines.Reflection.Features.SetBaseClassFeature>
{
    public class Process : SetBaseClassFeatureTests
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
        public void Sets_BaseClass_When_Available_Using_SourceModel_BaseClass()
        {
            // Arrange
            var sut = CreateSut();
            var sourceModel = typeof(MyBaseClassTestClass);
            var model = new ClassBuilder();
            var settings = CreateReflectionSettings();
            var context = new PipelineContext<TypeBaseBuilder, ReflectionContext>(model, new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.BaseClass.Should().Be("ClassFramework.Pipelines.Tests.Reflection.Features.MyBaseClassTestClassBase");
        }

        [Fact]
        public void Does_Not_Set_BaseClass_When_Not_Available_Using_SourceModel_BaseClass()
        {
            // Arrange
            var sut = CreateSut();
            var sourceModel = typeof(MyClass);
            var model = new ClassBuilder().WithBaseClass("Old value");
            var settings = CreateReflectionSettings();
            var context = new PipelineContext<TypeBaseBuilder, ReflectionContext>(model, new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.BaseClass.Should().BeEmpty();
        }

        [Fact]
        public void Sets_BaseClass_When_Available_Using_Inheritance_From_Settings()
        {
            // Arrange
            var sut = CreateSut();
            var sourceModel = typeof(MyClass);
            var model = new ClassBuilder();
            var settings = CreateReflectionSettings(
                useBaseClassFromSourceModel: false,
                enableEntityInheritance: true,
                baseClass: new ClassBuilder().WithName("MyBaseClass").BuildTyped());
            var context = new PipelineContext<TypeBaseBuilder, ReflectionContext>(model, new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.BaseClass.Should().Be("MyBaseClass");
        }

        [Fact]
        public void Does_Not_Set_BaseClass_When_Available_Using_Inheritance_From_Settings_When_BaseClass_Is_Empty()
        {
            // Arrange
            var sut = CreateSut();
            var sourceModel = typeof(MyClass);
            var model = new ClassBuilder();
            var settings = CreateReflectionSettings(
                useBaseClassFromSourceModel: false,
                enableEntityInheritance: true,
                baseClass: null);
            var context = new PipelineContext<TypeBaseBuilder, ReflectionContext>(model, new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.BaseClass.Should().BeEmpty();
        }

        [Fact]
        public void Does_Not_Set_BaseClass_When_UseBaseClassFromSource_Is_False()
        {
            // Arrange
            var sut = CreateSut();
            var sourceModel = typeof(MyBaseClassTestClass);
            var model = new ClassBuilder().WithBaseClass("Old value");
            var settings = CreateReflectionSettings(useBaseClassFromSourceModel: false);
            var context = new PipelineContext<TypeBaseBuilder, ReflectionContext>(model, new ReflectionContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.BaseClass.Should().BeEmpty();
        }
    }
}

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
#pragma warning disable S2094 // Classes should not be empty
internal sealed class MyBaseClassTestClass : MyBaseClassTestClassBase
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
#pragma warning restore S2094 // Classes should not be empty
{
}

#pragma warning disable CA1812 // Avoid uninstantiated internal classes
#pragma warning disable S2094 // Classes should not be empty
internal class MyBaseClassTestClassBase
#pragma warning restore CA1812 // Avoid uninstantiated internal classes
#pragma warning restore S2094 // Classes should not be empty
{
}
