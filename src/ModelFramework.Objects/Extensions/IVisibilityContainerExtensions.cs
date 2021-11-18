using System.Globalization;
using System.Text;
using ModelFramework.Common.Contracts;
using ModelFramework.Common.Extensions;
using ModelFramework.Objects.Contracts;

namespace ModelFramework.Objects.Extensions
{
    public static class IVisibilityContainerExtensions
    {
        public static string GetModifiers<T>(this T instance)
            where T : IMetadataContainer, IVisibilityContainer
        {
            var customModifiers = instance.Metadata.GetMetadataStringValue(MetadataNames.CustomModifiers);
            if (!string.IsNullOrEmpty(customModifiers))
            {
                return customModifiers;
            }
            var builder = new StringBuilder();
            if (instance is IExtendedVisibilityContainer extendedVisibilityContainer)
            {
                builder.AddWithCondition("protected", extendedVisibilityContainer.Protected);
                if (!(extendedVisibilityContainer.Protected && instance.Visibility != Visibility.Internal))
                {
                    builder.AddWithCondition(instance.Visibility.ToString().ToLower(CultureInfo.InvariantCulture), true);
                }
                builder.AddWithCondition("static", extendedVisibilityContainer.Static);
                builder.AddWithCondition("abstract", extendedVisibilityContainer.Abstract);
                builder.AddWithCondition("virtual", extendedVisibilityContainer.Virtual);
                builder.AddWithCondition("override", extendedVisibilityContainer.Override);
            }
            else
            {
                builder.AddWithCondition(instance.Visibility.ToString().ToLower(CultureInfo.InvariantCulture), true);
                if (instance is IClass cls)
                {
                    builder.AddWithCondition("sealed", cls.Sealed);
                    builder.AddWithCondition("static", cls.Static);
                }
                if (instance is ITypeBase typeBase)
                {
                    builder.AddWithCondition("partial", typeBase.Partial);
                }
            }
            return builder.ToString();
        }
    }
}
