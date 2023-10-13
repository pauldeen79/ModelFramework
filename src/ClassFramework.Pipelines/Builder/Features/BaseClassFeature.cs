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

    public void Process(PipelineContext<ClassBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.BaseClass = GetImmutableBuilderClassBaseClass(context.Context.SourceModel, context);
    }

    public IBuilder<IPipelineFeature<ClassBuilder, BuilderContext>> ToBuilder()
        => new BaseClassFeatureBuilder(_formattableStringParser);

    private string GetImmutableBuilderClassBaseClass(TypeBase instance, PipelineContext<ClassBuilder, BuilderContext> context)
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

        if (isNotForAbstractBuilder || isAbstract)
        {
            return _formattableStringParser
                .Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, context)
                .GetValueOrThrow() + genericTypeArgumentsString;
        }

        if (context.Context.Settings.ClassSettings.InheritanceSettings.EnableInheritance
            && context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
            && context.Context.Settings.InheritanceSettings.BaseClass is not null
            && !context.Context.Settings.IsForAbstractBuilder) // note that originally, this was only enabled when RemoveDuplicateWithMethods was true. But I don't know why you don't want this... The generics ensure that we don't have to duplicate them, right?
        {
            var ns = string.IsNullOrEmpty(context.Context.Settings.InheritanceSettings.BaseClassBuilderNameSpace)
                ? string.Empty
                : $"{context.Context.Settings.InheritanceSettings.BaseClassBuilderNameSpace}.";

            return $"{ns}{_formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, new PipelineContext<ClassBuilder, BuilderContext>(context.Model, new BuilderContext(context.Context.Settings.InheritanceSettings.BaseClass!, context.Context.Settings, context.Context.FormatProvider))).GetValueOrThrow()}<{_formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, context).GetValueOrThrow()}{genericTypeArgumentsString}, {instance.FormatInstanceName(false, context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate)}{genericTypeArgumentsString}>";
        }

        var builderName = _formattableStringParser
            .Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, context)
            .GetValueOrThrow();

        return instance.GetCustomValueForInheritedClass
        (
            context.Context.Settings,
            baseClassContainer => context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
                ? $"{GetBaseClassName(context, baseClassContainer)}{genericTypeArgumentsString}"
                : $"{GetBaseClassName(context, baseClassContainer)}<{builderName}{genericTypeArgumentsString}, {instance.FormatInstanceName(false, context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate)}{genericTypeArgumentsString}>"
        );
    }

    private string GetBaseClassName(PipelineContext<ClassBuilder, BuilderContext> context, IBaseClassContainer baseClassContainer)
    {
        var newContext = new PipelineContext<ClassBuilder, BuilderContext>
        (
            context.Model,
            new BuilderContext(CreateTypeBase(baseClassContainer.BaseClass!), context.Context.Settings, context.Context.FormatProvider)
        );

        return _formattableStringParser
            .Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, newContext)
            .GetValueOrThrow();
    }

    private static TypeBase CreateTypeBase(string baseClass)
        => new ClassBuilder()
            .WithNamespace(baseClass.GetNamespaceWithDefault())
            .WithName(baseClass.GetClassName())
            .Build();
}
