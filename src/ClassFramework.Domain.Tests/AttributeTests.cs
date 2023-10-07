namespace ClassFramework.Domain.Tests;

public class AttributeTests
{
    public class Constructor
    {
        [Fact]
        public void Throws_On_Null_Parameters()
        {
            // Act & Assert
            this.Invoking(_ => new Attribute(parameters: null!, Enumerable.Empty<Metadata>(), "Name"))
                .Should().Throw<ArgumentNullException>().WithParameterName("parameters");
        }

        [Fact]
        public void Throws_On_Null_Name()
        {
            // Act & Assert
            this.Invoking(_ => new Attribute(Enumerable.Empty<AttributeParameter>(), Enumerable.Empty<Metadata>(), name: null!))
                .Should().Throw<ArgumentNullException>().WithParameterName("name");
        }

        [Fact]
        public void Throws_On_Emtpty_Name()
        {
            // Act & Assert
            this.Invoking(_ => new Attribute(Enumerable.Empty<AttributeParameter>(), Enumerable.Empty<Metadata>(), name: string.Empty))
                .Should().Throw<ValidationException>();
        }
    }
}
