namespace ClassFramework.Domain.Builders.Tests;

public class AttributeBuilderTests
{
    public class AddNameAndParameter
    {
        [Fact]
        public void Sets_Name_And_Parameter_Correctly()
        {
            // Arrange
            var sut = new AttributeBuilder();

            // Act
            var actual = sut.AddNameAndParameter("System.ComponentModel.ReadOnly", true);

            // Assert
            actual.Name.Should().Be("System.ComponentModel.ReadOnly");
            actual.Parameters.Should().ContainSingle();
            actual.Parameters[0].Value.Should().Be(true);
        }
    }

    public class ForCodeGenerator
    {
        [Fact]
        public void Adds_Parameters_Correctly()
        {
            // Arrange
            var sut = new AttributeBuilder();

            // Act
            var actual = sut.ForCodeGenerator("MyGenerator", "1.0.0.0");

            // Assert
            actual.Name.Should().Be(typeof(GeneratedCodeAttribute).FullName);
            actual.Parameters.Should().BeEquivalentTo(new[]
            {
                new AttributeParameterBuilder().WithValue("MyGenerator"),
                new AttributeParameterBuilder().WithValue("1.0.0.0")
            });
        }
    }

    // Note that this is actually generated code, but I want to prove that this is working correctly
    public class AddParameters
    {
        [Fact]
        public void Throws_On_Null_Parameters_As_Array()
        {
            // Arrange
            var sut = new AttributeBuilder();
            AttributeParameterBuilder[] parameters = default!;

            // Act & Assert
            sut.Invoking(x => x.AddParameters(parameters: parameters))
               .Should().Throw<ArgumentNullException>().WithParameterName("parameters");
        }

        [Fact]
        public void Throws_On_Null_Parameters_As_Enumerable()
        {
            // Arrange
            var sut = new AttributeBuilder();
            IEnumerable<AttributeParameterBuilder> parameters = default!;

            // Act & Assert
            sut.Invoking(x => x.AddParameters(parameters: parameters))
               .Should().Throw<ArgumentNullException>().WithParameterName("parameters");
        }
    }

    public class WithName
    {
        [Fact]
        public void Throws_On_Null_Type()
        {
            // Arrange
            var sut = new AttributeBuilder();

            // Act & Assert
            sut.Invoking(x => _ = x.WithName(sourceType: null!))
               .Should().Throw<ArgumentNullException>().WithParameterName("sourceType");
        }

        [Fact]
        public void Sets_Name_Correctly_When_SourceType_Is_Not_Null()
        {
            // Arrange
            var sut = new AttributeBuilder();

            // Act
            var result = sut.WithName(typeof(AttributeBuilderTests));

            // Assert
            result.Name.Should().Be("ClassFramework.Domain.Builders.Tests.AttributeBuilderTests");
        }
    }
}
