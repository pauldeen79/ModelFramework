namespace ClassFramework.Pipelines.Reflection.Features;

public class AddConstructorsFeatureBuilder : IReflectionFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, ReflectionContext> Build()
        => new AddConstructorsFeature();
}

public class AddConstructorsFeature : IPipelineFeature<TypeBaseBuilder, ReflectionContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, ReflectionContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.GenerationSettings.CreateConstructors
            || context.Model is not IConstructorsContainerBuilder constructorsContainerBuilder)
        {
            return Result.Continue<TypeBaseBuilder>();
        }

        constructorsContainerBuilder.AddConstructors(GetConstructors(context));

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, ReflectionContext>> ToBuilder()
        => new AddConstructorsFeatureBuilder();

    private static IEnumerable<ConstructorBuilder> GetConstructors(PipelineContext<TypeBaseBuilder, ReflectionContext> context)
        => context.Context.SourceModel.GetConstructors()
            .Select(x => new ConstructorBuilder()
                .AddParameters
                (
                    x.GetParameters().Select
                    (
                        p =>
                        new ParameterBuilder()
                            .WithName(p.Name)
                            .WithTypeName(p.ParameterType.FullName.FixTypeName())
                            .WithIsNullable(p.IsNullable())
                            .WithIsValueType(p.ParameterType.IsValueType || p.ParameterType.IsEnum)
                            .AddAttributes(p.GetCustomAttributes(true)
                                .OfType<System.Attribute>()
                                .Where(x => context.Context.Settings.CopySettings.CopyAttributes
                                         && x.GetType().FullName != "System.Runtime.CompilerServices.NullableContextAttribute"
                                         && x.GetType().FullName != "System.Runtime.CompilerServices.NullableAttribute")
                                .Select(x => x.ConvertToDomainAttribute(context.Context.Settings.GenerationSettings.AttributeInitializeDelegate))
                                .Where(x => context.Context.Settings.CopySettings.CopyAttributePredicate?.Invoke(x) ?? true)
                                .Select(x => x.ToBuilder()))
                    )
                )
                .AddAttributes(x.GetCustomAttributes(true)
                    .OfType<System.Attribute>()
                    .Where(x => context.Context.Settings.CopySettings.CopyAttributes
                                && x.GetType().FullName != "System.Runtime.CompilerServices.NullableContextAttribute"
                                && x.GetType().FullName != "System.Runtime.CompilerServices.NullableAttribute")
                    .Select(x => x.ConvertToDomainAttribute(context.Context.Settings.GenerationSettings.AttributeInitializeDelegate))
                    .Where(x => context.Context.Settings.CopySettings.CopyAttributePredicate?.Invoke(x) ?? true)
                    .Select(x => x.ToBuilder()))
        );
}
