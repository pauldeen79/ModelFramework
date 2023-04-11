namespace ModelFramework.Common.Tests.Test.Contracts;

public interface IChild
{
    [Required, MinLength(1), MaxLength(10), StringLength(10, MinimumLength = 1), RegularExpression("something"), Url]
    string ChildProperty { get; }
    [Range(1, 10)]
    int NumericProperty { get; }
}
