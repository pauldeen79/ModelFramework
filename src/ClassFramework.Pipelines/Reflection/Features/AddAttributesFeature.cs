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
            .Select(x => ConvertToDomainAttribute(x, context))
            .Where(x => context.Context.Settings.CopySettings.CopyAttributePredicate?.Invoke(x) ?? true)
            .Select(x => new AttributeBuilder(x))
        );

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, ReflectionContext>> ToBuilder()
        => new AddAttributesFeatureBuilder();

    private Domain.Attribute ConvertToDomainAttribute(System.Attribute source, PipelineContext<TypeBaseBuilder, ReflectionContext> context)
    {
        var prefilled = context.Context.Settings.GenerationSettings.InitializeDelegate is not null
            ? context.Context.Settings.GenerationSettings.InitializeDelegate(source)
            : null;

        var builder = new AttributeBuilder();
        
        if (prefilled is not null)
        {
            builder
                .WithName(prefilled.Name)
                .AddParameters(prefilled.Parameters)
                .AddMetadata(prefilled.Metadata);
        }
        else
        {
            builder.WithName(source.GetType().FullName);
        }

        return context.Context.MapAttribute(builder.Build());
    }
}
