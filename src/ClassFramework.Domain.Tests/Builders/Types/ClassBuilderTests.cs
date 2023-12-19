namespace ClassFramework.Domain.Builders.Tests.Types;

public class ClassBuilderTests
{
    public class WithBaseClass
    {
        [Fact]
        public void WithBaseClass_Throws_On_Null_BaseClassType()
        {
            // Arrange
            var sut = new ClassBuilder();

            // Act & Assert
            sut.Invoking(x => x.WithBaseClass(baseClassType: default!))
               .Should().Throw<ArgumentNullException>().WithParameterName("baseClassType");
        }

        [Fact]
        public void WithBaseClass_Returns_Correct_Result()
        {
            // Arrange
            var sut = new ClassBuilder();

            // Act
            var result = sut.WithBaseClass(typeof(string));

            // Assert
            result.BaseClass.Should().Be(typeof(string).FullName);
        }

        [Fact]
        public void Can_Convert_Entity_To_Builder()
        {
            // Arrange
            var entity = new ClassBuilder().WithName("MyClass").Build();

            // Act
            var builder = entity.ToBuilder();

            // Assert
            builder.Should().BeOfType<ClassBuilder>();
        }

        [Fact]
        public void Can_Convert_Builder_To_Entity()
        {
            // Arrange
            var builder = new ClassBuilder().WithName("MyClass");

            // Act
            var entity = builder.Build();

            // Assert
            entity.Should().BeOfType<Class>();
        }
    }
}
