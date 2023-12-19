namespace ClassFramework.Pipelines.Reflection.Features;

public class AddAttributesFeatureBuilder : IReflectionFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, ReflectionContext> Build()
        => new AddAttributesFeature();
}

public class AddAttributesFeature : IPipelineFeature<TypeBaseBuilder, ReflectionContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, ReflectionContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.CopySettings.CopyAttributes)
        {
            return Result.Continue<TypeBaseBuilder>();
        }

        context.Model.AddAttributes(context.Context.SourceModel.GetCustomAttributes(true)
            .OfType<System.Attribute>()
            .Where(x => x.GetType().FullName != "System.Runtime.CompilerServices.NullableContextAttribute"
                     && x.GetType().FullName != "System.Runtime.CompilerServices.NullableAttribute")
            .Select(x => context.Context.MapAttribute(x.ConvertToDomainAttribute(context.Context.Settings.GenerationSettings.AttributeInitializeDelegate)))
            .Where(x => context.Context.Settings.CopySettings.CopyAttributePredicate?.Invoke(x) ?? true)
            .Select(x => new AttributeBuilder(x))
        );

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, ReflectionContext>> ToBuilder()
        => new AddAttributesFeatureBuilder();
}
