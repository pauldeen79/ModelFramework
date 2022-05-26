namespace ModelFramework.Common.Tests;

public class SystemTests
{
    [Fact]
    public void Can_Compare_Two_ImmutableClasses()
    {
        // Arrange
        var md1 = new Metadata("Value1", "Name1");
        var md2 = new Metadata("Value2", "Name2");
        var sut1 = new MyRecord(new ValueCollection<IMetadata>(new[] { md1, md2 }));
        var sut2 = new MyRecord(new ValueCollection<IMetadata>(new[] { md1, md2 }));

        // Act
        var actual = sut1.Equals(sut2);

        // Assert
        actual.Should().BeTrue();
    }
}

public interface IMetadataContainer2
{
    IReadOnlyCollection<IMetadata> Metadata { get; }
}

public record MyRecord : IMetadataContainer2
{
    public MyRecord(IEnumerable<IMetadata> metadata) => Metadata = new ValueCollection<IMetadata>(metadata);

    public IReadOnlyCollection<IMetadata> Metadata { get; }
}
