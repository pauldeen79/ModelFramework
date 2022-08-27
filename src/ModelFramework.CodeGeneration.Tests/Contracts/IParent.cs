namespace ModelFramework.Common.Tests.Test.Contracts;

public interface IParent
{
    string ParentProperty { get; }
    IChild Child { get; }
    IReadOnlyCollection<IChild> Children { get; }
}
