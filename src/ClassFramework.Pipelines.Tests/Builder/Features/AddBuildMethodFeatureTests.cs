namespace ClassFramework.Pipelines.Tests.Builder.Features;

public class AddBuildMethodFeatureTests : TestBase<AddBuildMethodFeature>
{
    public class Process : AddBuildMethodFeatureTests
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
        public void Does_Not_Add_Methods_When_EnableBuilderInheritance_And_IsAbstract_Are_Both_True()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").Build();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(enableBuilderInheritance: true, isAbstract: true);
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().BeEmpty();
        }

        [Fact]
        public void Adds_Build_Method_When_IsBuilderForAbstractEntity_Is_False()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").AddProperties(new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string))).Build();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings();
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().ContainSingle();
            var method = model.Methods.Single();
            method.Name.Should().Be("Build");
            method.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            method.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "return new SomeNamespace.SomeClass { MyProperty = MyProperty };"
            );
        }

        [Fact]
        public void Adds_Build_And_BuildTyped_Methods_When_IsBuilderForAbstractEntity_Is_True()
        {
            // Arrange
            var sourceModel = new ClassBuilder().WithName("SomeClass").WithNamespace("SomeNamespace").AddProperties(new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string))).Build();
            InitializeParser();
            var sut = CreateSut();
            var model = new ClassBuilder();
            var settings = CreateBuilderSettings(enableEntityInheritance: true);
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, new BuilderContext(sourceModel, settings, CultureInfo.InvariantCulture));

            // Act
            var result = sut.Process(context);

            // Assert
            result.IsSuccessful().Should().BeTrue();
            model.Methods.Should().HaveCount(2);

            var buildMethod = model.Methods.SingleOrDefault(x => x.Name == "Build");
            buildMethod.Should().NotBeNull(because: "Build method should exist");
            buildMethod!.Abstract.Should().BeFalse();
            buildMethod.TypeName.Should().Be("SomeNamespace.SomeClass");
            buildMethod.CodeStatements.Should().AllBeOfType<StringCodeStatementBuilder>();
            buildMethod.CodeStatements.OfType<StringCodeStatementBuilder>().Select(x => x.Statement).Should().BeEquivalentTo
            (
                "return BuildTyped();"
            );

            var buildTypedMethod = model.Methods.SingleOrDefault(x => x.Name == "BuildTyped");
            buildTypedMethod.Should().NotBeNull(because: "BuildTyped method should exist");
            buildTypedMethod!.Abstract.Should().BeTrue();
            buildTypedMethod.TypeName.Should().Be("TEntity");
            buildTypedMethod.CodeStatements.Should().BeEmpty();
        }
    }
}
