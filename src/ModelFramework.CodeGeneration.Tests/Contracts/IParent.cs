namespace ModelFramework.Common.Tests.Test.Contracts;

public interface IParent
{
    [StringLength(10, MinimumLength = 1)]
    string ParentProperty { get; }
    IChild Child { get; }
    IReadOnlyCollection<IChild> Children { get; }
    IReadOnlyCollection<string> Strings { get; }
}
