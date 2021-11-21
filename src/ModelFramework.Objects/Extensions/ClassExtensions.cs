using System.Linq;
using ModelFramework.Objects.Builders;
using ModelFramework.Objects.Contracts;
using ModelFramework.Objects.Settings;

namespace ModelFramework.Objects.Extensions
{
    public static class ClassExtensions
    {
        public static IClass ToImmutableBuilderClass(this IClass instance, ImmutableBuilderClassSettings settings)
            => instance.ToImmutableBuilderClassBuilder(settings).Build();

        public static ClassBuilder ToImmutableBuilderClassBuilder(this IClass instance, ImmutableBuilderClassSettings settings)
            => ((ITypeBase)instance).ToImmutableBuilderClassBuilder(settings.WithPoco(instance.IsPoco()));

        public static bool IsPoco(this IClass instance)
            => instance.Constructors.Count == 0 || instance.Constructors.Any(x => x.Parameters.Count == 0);
    }
}
