namespace ClassFramework.Domain.Tests;

public class AttributeBaseTests
{
    public class Constructor
    {
        [Fact]
        public void Does_Not_Throw_On_Null_Parameters()
        {
            // Act & Assert
            this.Invoking(_ => new AttributeBase(parameters: null!, Enumerable.Empty<Metadata>(), "Name"))
                .Should().NotThrow();
        }
    }
}
