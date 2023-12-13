namespace ClassFramework.Pipelines.Reflection.Features;

public class AddAttributesFeatureBuilder : IReflectionFeatureBuilder
{
    public IPipelineFeature<ClassBuilder, ReflectionContext> Build()
        => new AddAttributesFeature();
}

public class AddAttributesFeature : IPipelineFeature<ClassBuilder, ReflectionContext>
{
    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, ReflectionContext> context)
    {
        context = context.IsNotNull(nameof(context));

        if (!context.Context.Settings.CopySettings.CopyAttributes)
        {
            return Result.Continue<ClassBuilder>();
        }

        context.Model.AddAttributes(context.Context.SourceModel.GetCustomAttributes(true)
            .OfType<System.Attribute>()
            .Where(x => x.GetType().FullName != "System.Runtime.CompilerServices.NullableContextAttribute"
                     && x.GetType().FullName != "System.Runtime.CompilerServices.NullableAttribute")
            .Select(x => ConvertToDomainAttribute(x, context))
            .Where(x => context.Context.Settings.CopySettings.CopyAttributePredicate?.Invoke(x) ?? true)
            .Select(x => new AttributeBuilder(x)));

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, ReflectionContext>> ToBuilder()
        => new AddAttributesFeatureBuilder();

    private Domain.Attribute ConvertToDomainAttribute(System.Attribute source, PipelineContext<ClassBuilder, ReflectionContext> context)
    {
        var prefilled = context.Context.Settings.GenerationSettings.InitializeDelegate is not null
            ? context.Context.Settings.GenerationSettings.InitializeDelegate(source)
            : null;

        var builder = new AttributeBuilder();
        
        if (prefilled is not null)
        {
            builder.Name = prefilled.Name;
            builder.Parameters = prefilled.Parameters;
            builder.Metadata = prefilled.Metadata;
        }
        else
        {
            builder.Name = source.GetType().FullName;
        }

        return context.Context.MapAttribute(builder.Build());
    }
}
