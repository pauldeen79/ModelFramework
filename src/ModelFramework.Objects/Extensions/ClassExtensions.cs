namespace ModelFramework.Objects.Extensions;

public static class ClassExtensions
{
    public static IClass ToImmutableBuilderClass(this IClass instance, ImmutableBuilderClassSettings settings)
        => instance.ToImmutableBuilderClassBuilder(settings).BuildTyped();

    public static ClassBuilder ToImmutableBuilderClassBuilder(this IClass instance, ImmutableBuilderClassSettings settings)
        => ((ITypeBase)instance).ToImmutableBuilderClassBuilder(settings);

    public static bool HasPublicParameterlessConstructor(this IClass instance)
        => instance.Constructors.Count == 0 || instance.Constructors.Any(x => x.Parameters.Count == 0);
}
