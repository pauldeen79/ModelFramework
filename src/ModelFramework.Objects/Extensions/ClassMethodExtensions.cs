using System;
using System.Linq;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    public static class ClassMethodExtensions
    {
        public static bool IsInterfaceMethod(this IClassMethod instance)
            => instance.Name.StartsWith("I") && instance.Name.Contains(".");

        public static string GetGenericTypeArgumentsString(this IClassMethod instance)
            => instance.GenericTypeArguments.Count > 0
                ? $"<{string.Join(", ", instance.GenericTypeArguments)}>"
                : string.Empty;

        public static string GetGenericTypeArgumentConstraintsString(this IClassMethod instance)
            => instance.GenericTypeArgumentConstraints.Any()
                ? string.Concat(Environment.NewLine,
                                "            ",
                                string.Join(string.Concat(Environment.NewLine, "            "), instance.GenericTypeArgumentConstraints))
                : string.Empty;
    }
}
