namespace ClassFramework.Pipelines.Builder.Features;

public class BaseClassFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public BaseClassFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, BuilderContext> Build()
        => new BaseClassFeature(_formattableStringParser);
}

public class BaseClassFeature : IPipelineFeature<ClassBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public BaseClassFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<ClassBuilder> Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        var baseClassResult = GetBuilderBaseClass(context.Context.SourceModel, context);
        if (!baseClassResult.IsSuccessful())
        {
            return Result.FromExistingResult<ClassBuilder>(baseClassResult);
        }

        context.Model.BaseClass = baseClassResult.Value!;

        return Result.Continue<ClassBuilder>();
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new BaseClassFeatureBuilder(_formattableStringParser);

    private Result<string> GetBuilderBaseClass(TypeBase instance, PipelineContext<ClassBuilder, BuilderContext> context)
    {
        var genericTypeArgumentsString = instance.GetGenericTypeArgumentsString();

        var isNotForAbstractBuilder = context.Context.Settings.ClassSettings.InheritanceSettings.EnableInheritance
            && context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
            && context.Context.Settings.InheritanceSettings.BaseClass is null
            && !context.Context.Settings.IsForAbstractBuilder;

        var isAbstract = context.Context.Settings.ClassSettings.InheritanceSettings.EnableInheritance
            && context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
            && context.Context.Settings.InheritanceSettings.BaseClass is not null
            && !context.Context.Settings.IsForAbstractBuilder
            && context.Context.Settings.InheritanceSettings.IsAbstract;

        var nameResult = _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, context);
        if (!nameResult.IsSuccessful())
        {
            return nameResult;
        }

        if (isNotForAbstractBuilder || isAbstract)
        {
            return Result.Success($"{nameResult.Value}{genericTypeArgumentsString}");
        }

        if (context.Context.Settings.ClassSettings.InheritanceSettings.EnableInheritance
            && context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
            && context.Context.Settings.InheritanceSettings.BaseClass is not null
            && !context.Context.Settings.IsForAbstractBuilder) // note that originally, this was only enabled when RemoveDuplicateWithMethods was true. But I don't know why you don't want this... The generics ensure that we don't have to duplicate them, right?
        {
            var ns = string.IsNullOrEmpty(context.Context.Settings.InheritanceSettings.BaseClassBuilderNameSpace)
                ? string.Empty
                : $"{context.Context.Settings.InheritanceSettings.BaseClassBuilderNameSpace}.";

            var inheritanceNameResult = _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, new PipelineContext<ClassBuilder, BuilderContext>(context.Model, new BuilderContext(context.Context.Settings.InheritanceSettings.BaseClass!, context.Context.Settings, context.Context.FormatProvider)));
            if (!inheritanceNameResult.IsSuccessful())
            {
                return inheritanceNameResult;
            }

            return Result.Success($"{ns}{inheritanceNameResult.Value}<{nameResult.Value}{genericTypeArgumentsString}, {instance.GetFullName()}{genericTypeArgumentsString}>");
        }

        return instance.GetCustomValueForInheritedClass
        (
            context.Context.Settings.ClassSettings,
            baseClassContainer =>
            {
                var baseClassResult = GetBaseClassName(context, baseClassContainer);
                if (!baseClassResult.IsSuccessful())
                {
                    return baseClassResult;
                }

                return Result.Success(context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
                    ? $"{baseClassResult.Value}{genericTypeArgumentsString}"
                    : $"{baseClassResult.Value}<{nameResult.Value}{genericTypeArgumentsString}, {instance.GetFullName()}{genericTypeArgumentsString}>");
                }
        );
    }

    private Result<string> GetBaseClassName(PipelineContext<ClassBuilder, BuilderContext> context, IBaseClassContainer baseClassContainer)
    {
        var newContext = new PipelineContext<ClassBuilder, BuilderContext>
        (
            context.Model,
            new BuilderContext(CreateTypeBase(baseClassContainer.BaseClass!), context.Context.Settings, context.Context.FormatProvider)
        );

        return _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, newContext);
    }

    private static TypeBase CreateTypeBase(string baseClass)
        => new ClassBuilder()
            .WithNamespace(baseClass.GetNamespaceWithDefault())
            .WithName(baseClass.GetClassName())
            .Build();
}
