using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    public static class ClassMethodExtensions
    {
        public static bool IsInterfaceMethod(this IClassMethod instance)
            => instance.Name.StartsWith("I") && instance.Name.Contains(".");
    }
}
