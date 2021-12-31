using System.Diagnostics.CodeAnalysis;

namespace ModelFramework.Generators.Objects.Tests
{
#nullable enable
    [ExcludeFromCodeCoverage]
    public class NullableTestClass
    {
#pragma warning disable CA1051 // Do not declare visible instance fields
        public string? ValueField;
#pragma warning restore CA1051 // Do not declare visible instance fields
        public string? Value { get; set; }

        public string? GetValue(string? input)
        {
            return string.Empty;
        }
    }
#nullable disable
}
