namespace ClassFramework.Pipelines.Reflection.Features;

public class AddPropertiesFeatureBuilder : IReflectionFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, ReflectionContext> Build()
        => new AddPropertiesFeature();
}

public class AddPropertiesFeature : IPipelineFeature<TypeBaseBuilder, ReflectionContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, ReflectionContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.AddProperties(GetProperties(context));

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, ReflectionContext>> ToBuilder()
        => new AddPropertiesFeatureBuilder();

    private static IEnumerable<PropertyBuilder> GetProperties(PipelineContext<TypeBaseBuilder, ReflectionContext> context)
        => context.Context.SourceModel.GetPropertiesRecursively().Select
        (
            p => new PropertyBuilder()
                .WithName(p.Name)
                .WithTypeName(p.PropertyType.GetTypeName(p))
                .WithHasGetter(p.GetGetMethod() != null)
                .WithHasSetter(p.GetSetMethod() != null)
                .WithHasInitializer(p.IsInitOnly())
                .WithParentTypeFullName(p.DeclaringType.FullName == "System.Object"
                    ? string.Empty
                    : p.DeclaringType.FullName.WithoutGenerics())
                .WithIsNullable(p.IsNullable())
                .WithIsValueType(p.PropertyType.IsValueType || p.PropertyType.IsEnum)
                .WithVisibility(Array.Exists(p.GetAccessors(), m => m.IsPublic)
                    ? Visibility.Public
                    : Visibility.Private)
                .AddAttributes(p.GetCustomAttributes(true)
                    .OfType<System.Attribute>()
                    .Where(x => context.Context.Settings.CopySettings.CopyAttributes
                             && x.GetType().FullName != "System.Runtime.CompilerServices.NullableContextAttribute"
                             && x.GetType().FullName != "System.Runtime.CompilerServices.NullableAttribute")
                    .Select(x => x.ConvertToDomainAttribute(context.Context.Settings.GenerationSettings.AttributeInitializeDelegate))
                    .Where(x => context.Context.Settings.CopySettings.CopyAttributePredicate?.Invoke(x) ?? true)
                    .Select(x => x.ToBuilder()))
        );
}
