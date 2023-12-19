namespace ClassFramework.Pipelines.Reflection.Features;

public class AddFieldsFeatureBuilder : IReflectionFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, ReflectionContext> Build()
        => new AddFieldsFeature();
}

public class AddFieldsFeature : IPipelineFeature<TypeBaseBuilder, ReflectionContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, ReflectionContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.AddFields(GetFields(context));

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, ReflectionContext>> ToBuilder()
        => new AddFieldsFeatureBuilder();

    private static IEnumerable<FieldBuilder> GetFields(PipelineContext<TypeBaseBuilder, ReflectionContext> context)
        => context.Context.SourceModel.GetFieldsRecursively().Select
        (
            f => new FieldBuilder()
                .WithName(f.Name)
                .WithTypeName(f.FieldType.GetTypeName(f))
                .WithStatic(f.IsStatic)
                .WithConstant(f.IsLiteral)
                .WithParentTypeFullName(f.DeclaringType.FullName == "System.Object"
                    ? string.Empty
                    : f.DeclaringType.FullName)
                .WithIsNullable(f.IsNullable())
                .WithIsValueType(f.FieldType.IsValueType || f.FieldType.IsEnum)
                .WithVisibility(f.IsPublic
                    ? Visibility.Public
                    : Visibility.Private)
                .AddAttributes(f.GetCustomAttributes(true)
                    .OfType<System.Attribute>()
                    .Where(x => x.GetType().FullName != "System.Runtime.CompilerServices.NullableContextAttribute"
                             && x.GetType().FullName != "System.Runtime.CompilerServices.NullableAttribute")
                    .Select(x => new AttributeBuilder(x.ConvertToDomainAttribute(context.Context.Settings.GenerationSettings.AttributeInitializeDelegate))))
        );
}
