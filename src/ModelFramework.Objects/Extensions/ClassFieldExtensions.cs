using System.Globalization;
using System.Text;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    public static class ClassFieldExtensions
    {
        public static string GetModifiers(this IClassField instance)
        {
            var customModifiers = instance.Metadata.GetValue<object?>(MetadataNames.CustomModifiers, () => null)?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(customModifiers))
            {
                return customModifiers;
            }

            var builder = new StringBuilder();
            if (!(instance.Protected && instance.Visibility != Visibility.Internal))
            {
                builder.AddWithCondition(instance.Visibility.ToString().ToLower(CultureInfo.InvariantCulture), true);
            }

            return builder.AddWithCondition("protected", instance.Protected)
                .AddWithCondition("abstract", instance.Abstract)
                .AddWithCondition("virtual", instance.Virtual)
                .AddWithCondition("static", instance.Static)
                .AddWithCondition("readonly", instance.ReadOnly)
                .AddWithCondition("const", instance.Constant)
                .ToString();
        }
    }
}
