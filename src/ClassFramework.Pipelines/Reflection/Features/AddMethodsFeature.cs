namespace ClassFramework.Pipelines.Reflection.Features;

public class AddMethodsFeatureBuilder : IReflectionFeatureBuilder
{
    public IPipelineFeature<TypeBaseBuilder, ReflectionContext> Build()
        => new AddMethodsFeature();
}

public class AddMethodsFeature : IPipelineFeature<TypeBaseBuilder, ReflectionContext>
{
    public Result<TypeBaseBuilder> Process(PipelineContext<TypeBaseBuilder, ReflectionContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.AddMethods(GetMethods(context));

        return Result.Continue<TypeBaseBuilder>();
    }

    public IBuilder<IPipelineFeature<TypeBaseBuilder, ReflectionContext>> ToBuilder()
        => new AddMethodsFeatureBuilder();

    private static IEnumerable<MethodBuilder> GetMethods(PipelineContext<TypeBaseBuilder, ReflectionContext> context)
        => context.Context.SourceModel.GetMethodsRecursively()
            .Where(m => m.Name != "<Clone>$" && !m.Name.StartsWith("get_") && !m.Name.StartsWith("set_") && m.Name != "GetType")
            .Select
            (
                m => new MethodBuilder()
                    .WithName(m.Name)
                    .WithTypeName(m.ReturnType.GetTypeName(m))
                    .WithVisibility(m.IsPublic
                        ? Visibility.Public
                        : Visibility.Private)
                    .WithStatic(m.IsStatic)
                    .WithVirtual(m.IsVirtual)
                    .WithAbstract(m.IsAbstract)
                    .WithParentTypeFullName(m.DeclaringType.FullName == "System.Object"
                        ? string.Empty
                        : m.DeclaringType.FullName)
                    .WithIsNullable(m.ReturnTypeIsNullable())
                    .WithIsValueType(m.ReturnType.IsValueType || m.ReturnType.IsEnum)
                    .AddParameters(m.GetParameters().Select
                    (
                        p => new ParameterBuilder()
                            .WithName(p.Name)
                            .WithTypeName(p.ParameterType.GetTypeName(m))
                            .WithIsNullable(p.IsNullable())
                            .WithIsValueType(p.ParameterType.IsValueType || p.ParameterType.IsEnum)
                            .AddAttributes(p.GetCustomAttributes(true)
                                .OfType<System.Attribute>()
                                .Where(x => x.GetType().FullName != "System.Runtime.CompilerServices.NullableContextAttribute"
                                            && x.GetType().FullName != "System.Runtime.CompilerServices.NullableAttribute")
                                .Select(x => new AttributeBuilder(x.ConvertToDomainAttribute(context.Context.Settings.GenerationSettings.AttributeInitializeDelegate))))
                    ))
                    .AddAttributes(m.GetCustomAttributes(true)
                        .OfType<System.Attribute>()
                        .Where(x => x.GetType().FullName != "System.Runtime.CompilerServices.NullableContextAttribute"
                                    && x.GetType().FullName != "System.Runtime.CompilerServices.NullableAttribute")
                        .Select(x => new AttributeBuilder(x.ConvertToDomainAttribute(context.Context.Settings.GenerationSettings.AttributeInitializeDelegate))))
            );
}
