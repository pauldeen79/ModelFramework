namespace ClassFramework.TemplateFramework.Tests.ViewModels;

public class MethodViewModelTests : TestBase<MethodViewModel>
{
    public class ShouldRenderModifiers : MethodViewModelTests
    {
        [Fact]
        public void Throws_When_Model_Is_Null()
        {
            // Arrange
            var sut = CreateSut();
            sut.Model = null!;
            var context = Fixture.Freeze<ITemplateContext>();
            context.ParentContext.Returns(context); // note that we're using short-circuit here 8-) but who cares, we're just calling ParentContext.Model so it works.
            sut.Context = context;

            // Act & Assert
            sut.Invoking(x => _ = x.ShouldRenderModifiers)
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("Model");
        }

        [Fact]
        public void Throws_When_Context_Is_Null()
        {
            // Arrange
            var sut = CreateSut();
            sut.Model = new MethodBuilder().WithName("MyMethod").Build();
            sut.Context = null!;

            // Act & Assert
            sut.Invoking(x => _ = x.ShouldRenderModifiers)
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("Context");
        }

        [Theory]
        [InlineData(null, "class", true)]
        [InlineData(null, "interface", false)]
        [InlineData("", "class", true)]
        [InlineData("", "interface", false)]
        [InlineData("prefix", "class", false)]
        [InlineData("prefix", "interface", false)]
        public void Returns_Correct_Result(string? explicitInterfaceName, string parentModelType, bool expectedResult)
        {
            // Arrange
            object? parentModel = parentModelType switch
            {
                "class" => new ClassBuilder().WithName("MyClass").Build(),
                "interface" => new InterfaceBuilder().WithName("IMyInterface").Build(),
                _ => throw new NotSupportedException("Only 'class' and 'interface' are supported as parentModelType")
            };
            var sut = CreateSut();
            sut.Model = new MethodBuilder().WithName("MyMethod").WithExplicitInterfaceName(explicitInterfaceName).Build();
            var context = Fixture.Freeze<ITemplateContext>();
            context.ParentContext.Returns(context); // note that we're using short-circuit here 8-) but who cares, we're just calling ParentContext.Model so it works.
            context.Model.Returns(parentModel);
            sut.Context = context;

            // Act
            var result = sut.ShouldRenderModifiers;

            // Assert
            result.Should().Be(expectedResult);
        }
    }

    public class ReturnTypeName : MethodViewModelTests
    {
        [Fact]
        public void Throws_When_Model_Is_Null()
        {
            // Arrange
            var sut = CreateSut();
            sut.Model = null!;

            // Act & Assert
            sut.Invoking(x => _ = x.ReturnTypeName)
               .Should().Throw<ArgumentNullException>()
               .WithParameterName("Model");
        }

        [Fact]
        public void Gets_Csharp_Friendly_TypeName()
        {
            // Arrange
            var sut = CreateSut();
            sut.Model = new MethodBuilder().WithName("MyMethod").WithReturnType(typeof(int)).Build();

            // Act
            var result = sut.ReturnTypeName;

            // Assert
            result.Should().Be("int");
        }

        [Fact]
        public void Appends_Nullable_Notation()
        {
            // Arrange
            var sut = CreateSut();
            sut.Model = new MethodBuilder().WithName("MyMethod").WithReturnType(new ClassBuilder().WithName("MyType")).WithReturnTypeIsNullable().Build();

            // Act
            var result = sut.ReturnTypeName;

            // Assert
            result.Should().Be("MyType?");
        }
    }
}
