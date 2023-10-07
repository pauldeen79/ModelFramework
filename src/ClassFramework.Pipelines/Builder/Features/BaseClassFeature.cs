namespace ClassFramework.Pipelines.Builder.Features;

public class BaseClassFeatureBuilder : IBuilder<IPipelineFeature<ClassBuilder, PipelineBuilderContext>>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public BaseClassFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<ClassBuilder, PipelineBuilderContext> Build()
        => new BaseClassFeature(_formattableStringParser);
}

public class BaseClassFeature : IPipelineFeature<ClassBuilder, PipelineBuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public BaseClassFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public void Process(PipelineContext<ClassBuilder, PipelineBuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        context.Model.BaseClass = GetImmutableBuilderClassBaseClass(context.Context.SourceModel, context);
    }

    public IBuilder<IPipelineFeature<ClassBuilder, PipelineBuilderContext>> ToBuilder()
        => new BaseClassFeatureBuilder(_formattableStringParser);

    private string GetImmutableBuilderClassBaseClass(TypeBase instance, PipelineContext<ClassBuilder, PipelineBuilderContext> context)
    {
        var genericTypeArgumentsString = instance.GetGenericTypeArgumentsString();

        var isNotForAbstractBuilder = context.Context.Settings.InheritanceSettings.EnableEntityInheritance
            && context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
            && context.Context.Settings.InheritanceSettings.BaseClass is null
            && !context.Context.Settings.IsForAbstractBuilder;

        var isAbstract = context.Context.Settings.InheritanceSettings.EnableEntityInheritance
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

        //if (context.Context.Settings.InheritanceSettings.EnableEntityInheritance
        //    && context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
        //    && context.Context.Settings.InheritanceSettings.BaseClass is not null
        //    && !context.Context.Settings.IsForAbstractBuilder
        //    && context.Context.Settings.InheritanceSettings.RemoveDuplicateWithMethods)
        //{
        //    var ns = string.IsNullOrEmpty(context.Context.Settings.InheritanceSettings.BaseClassBuilderNameSpace)
        //        ? string.Empty
        //        : $"{context.Context.Settings.InheritanceSettings.BaseClassBuilderNameSpace}.";
        //    return $"{ns}{_formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, new PipelineContext<ClassBuilder, PipelineBuilderContext>(context.Model, new PipelineBuilderContext(context.Context.Settings.InheritanceSettings.BaseClass!, context.Context.Settings, context.Context.FormatProvider))).GetValueOrThrow()}<{_formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, context).GetValueOrThrow()}{genericTypeArgumentsString}, {instance.FormatInstanceName(false, context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate)}{genericTypeArgumentsString}>";
        //}

        var instanceNameBuilder = _formattableStringParser
            .Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, context)
            .GetValueOrThrow();

        return instance.GetCustomValueForInheritedClass
        (
            context.Context.Settings,
            cls => context.Context.Settings.InheritanceSettings.EnableBuilderInheritance && context.Context.Settings.InheritanceSettings.BaseClass is not null
                ? $"{GetBaseClassName(context, cls)}{genericTypeArgumentsString}"
                : $"{GetBaseClassName(context, cls)}<{instanceNameBuilder}{genericTypeArgumentsString}, {instance.FormatInstanceName(false, context.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate)}{genericTypeArgumentsString}>"
        );
    }

    private string GetBaseClassName(PipelineContext<ClassBuilder, PipelineBuilderContext> context, Class cls)
        => _formattableStringParser
            .Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, new PipelineContext<ClassBuilder, PipelineBuilderContext>(context.Model, new PipelineBuilderContext(CreateTypeBase(cls.BaseClass), context.Context.Settings, context.Context.FormatProvider)))
            .GetValueOrThrow();

    private static TypeBase CreateTypeBase(string baseClass)
        => new ClassBuilder()
            .WithNamespace(baseClass.GetNamespaceWithDefault())
            .WithName(baseClass.GetClassName())
            .Build();
}
