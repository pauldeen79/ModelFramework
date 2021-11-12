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
            var customModifiers = instance.Metadata.GetMetadataValue<object>(MetadataNames.CustomModifiers)?.ToString() ?? string.Empty;
            if (!string.IsNullOrEmpty(customModifiers))
            {
                return customModifiers;
            }

            var builder = new StringBuilder();
            if (!(instance.Protected && instance.Visibility != Visibility.Internal))
            {
                builder.AddWithCondition(instance.Visibility.ToString().ToLower(CultureInfo.InvariantCulture));
            }

            builder.AddWithCondition("protected", instance.Protected);
            builder.AddWithCondition("abstract", instance.Abstract);
            builder.AddWithCondition("virtual", instance.Virtual);
            builder.AddWithCondition("static", instance.Static);
            builder.AddWithCondition("readonly", instance.ReadOnly);
            builder.AddWithCondition("const", instance.Constant);
            return builder.ToString();
        }
    }
}
