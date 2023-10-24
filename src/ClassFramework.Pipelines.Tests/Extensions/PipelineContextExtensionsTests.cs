namespace ClassFramework.Pipelines.Tests.Extensions;

public class PipelineContextExtensionsTests : TestBase
{
    public class CreateEntityInstanciation : PipelineContextExtensionsTests
    {
        [Fact]
        public void Throws_On_Abstract_Class()
        {
            // Arrange
            var model = new ClassBuilder();
            var sourceModel = new ClassBuilder().WithNamespace("MyNamespace").WithName("MyClass").WithAbstract().AddProperties(new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string))).Build();
            InitializeParser();
            var builderContext = new BuilderContext(sourceModel, new Pipelines.Builder.PipelineBuilderSettings(), Fixture.Freeze<IFormatProvider>());
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, builderContext);
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act & Assert
            context.Invoking(x => x.CreateEntityInstanciation(formattableStringParser, string.Empty))
                   .Should().Throw<InvalidOperationException>().WithMessage("Cannot create an instance of an abstract class");
        }

        [Fact]
        public void Throws_On_Interface()
        {
            // Arrange
            var model = new ClassBuilder();
            var sourceModel = new InterfaceBuilder().WithNamespace("MyNamespace").WithName("MyClass").AddProperties(new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string))).Build();
            InitializeParser();
            var builderContext = new BuilderContext(sourceModel, new Pipelines.Builder.PipelineBuilderSettings(), Fixture.Freeze<IFormatProvider>());
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, builderContext);
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act & Assert
            context.Invoking(x => x.CreateEntityInstanciation(formattableStringParser, string.Empty))
                   .Should().Throw<InvalidOperationException>().WithMessage("Cannot create an instance of a type that does not have constructors");
        }

        [Fact]
        public void Returns_Correct_Result_For_Class_With_Public_Parameterless_Constructor()
        {
            // Arrange
            var model = new ClassBuilder();
            var sourceModel = new ClassBuilder().WithNamespace("MyNamespace").WithName("MyClass").AddProperties(new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string))).Build();
            InitializeParser();
            var builderContext = new BuilderContext(sourceModel, new Pipelines.Builder.PipelineBuilderSettings(), Fixture.Freeze<IFormatProvider>());
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, builderContext);
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = context.CreateEntityInstanciation(formattableStringParser, string.Empty);

            // Assert
            result.Should().Be("new MyNamespace.MyClass { MyProperty = MyProperty }");
        }

        [Fact]
        public void Returns_Correct_Result_For_Class_With_Public_Constructor_With_Parameters()
        {
            // Arrange
            var model = new ClassBuilder();
            var sourceModel = new ClassBuilder().WithNamespace("MyNamespace").WithName("MyClass").AddProperties(new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string))).AddConstructors(new ClassConstructorBuilder().AddParameter("myProperty", typeof(string))).Build();
            InitializeParser();
            var builderContext = new BuilderContext(sourceModel, new Pipelines.Builder.PipelineBuilderSettings(), Fixture.Freeze<IFormatProvider>());
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, builderContext);
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = context.CreateEntityInstanciation(formattableStringParser, string.Empty);

            // Assert
            result.Should().Be("new MyNamespace.MyClass(MyProperty)");
        }

        [Fact]
        public void Returns_Correct_Result_For_Struct_With_Public_Parameterless_Constructor()
        {
            // Arrange
            var model = new ClassBuilder();
            var sourceModel = new StructBuilder().WithNamespace("MyNamespace").WithName("MyClass").AddProperties(new ClassPropertyBuilder().WithName("MyProperty").WithType(typeof(string))).Build();
            InitializeParser();
            var builderContext = new BuilderContext(sourceModel, new Pipelines.Builder.PipelineBuilderSettings(), Fixture.Freeze<IFormatProvider>());
            var context = new PipelineContext<ClassBuilder, BuilderContext>(model, builderContext);
            var formattableStringParser = Fixture.Freeze<IFormattableStringParser>();

            // Act
            var result = context.CreateEntityInstanciation(formattableStringParser, string.Empty);

            // Assert
            result.Should().Be("new MyNamespace.MyClass { MyProperty = MyProperty }");
        }
    }
}
