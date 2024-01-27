﻿namespace ClassFramework.Pipelines.Builder.Features;

public class BaseClassFeatureBuilder : IBuilderFeatureBuilder
{
    private readonly IFormattableStringParser _formattableStringParser;

    public BaseClassFeatureBuilder(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public IPipelineFeature<IConcreteTypeBuilder, BuilderContext> Build()
        => new BaseClassFeature(_formattableStringParser);
}

public class BaseClassFeature : IPipelineFeature<IConcreteTypeBuilder, BuilderContext>
{
    private readonly IFormattableStringParser _formattableStringParser;

    public BaseClassFeature(IFormattableStringParser formattableStringParser)
    {
        _formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
    }

    public Result<IConcreteTypeBuilder> Process(PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        context = context.IsNotNull(nameof(context));

        var baseClassResult = GetBuilderBaseClass(context.Context.SourceModel, context);
        if (!baseClassResult.IsSuccessful())
        {
            return Result.FromExistingResult<IConcreteTypeBuilder>(baseClassResult);
        }

        context.Model.WithBaseClass(baseClassResult.Value!);

        return Result.Continue<IConcreteTypeBuilder>();
    }

    public IBuilder<IPipelineFeature<IConcreteTypeBuilder, BuilderContext>> ToBuilder()
        => new BaseClassFeatureBuilder(_formattableStringParser);

    private Result<string> GetBuilderBaseClass(IType instance, PipelineContext<IConcreteTypeBuilder, BuilderContext> context)
    {
        var genericTypeArgumentsString = instance.GetGenericTypeArgumentsString();

        var isNotForAbstractBuilder = context.Context.Settings.EntitySettings.InheritanceSettings.EnableInheritance
            && context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
            && context.Context.Settings.InheritanceSettings.BaseClass is null
            && !context.Context.Settings.IsForAbstractBuilder;

        var isAbstract = context.Context.Settings.EntitySettings.InheritanceSettings.EnableInheritance
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

        if (context.Context.Settings.EntitySettings.InheritanceSettings.EnableInheritance
            && context.Context.Settings.InheritanceSettings.EnableBuilderInheritance
            && context.Context.Settings.InheritanceSettings.BaseClass is not null
            && !context.Context.Settings.IsForAbstractBuilder) // note that originally, this was only enabled when RemoveDuplicateWithMethods was true. But I don't know why you don't want this... The generics ensure that we don't have to duplicate them, right?
        {
            var inheritanceNameResult = _formattableStringParser.Parse
            (
                context.Context.Settings.NameSettings.BuilderNameFormatString,
                context.Context.FormatProvider,
                new PipelineContext<IConcreteTypeBuilder, BuilderContext>(context.Model, new BuilderContext(context.Context.Settings.InheritanceSettings.BaseClass!, context.Context.Settings, context.Context.FormatProvider))
            );
            if (!inheritanceNameResult.IsSuccessful())
            {
                return inheritanceNameResult;
            }

            return Result.Success($"{context.Context.Settings.InheritanceSettings.BaseClassBuilderNameSpace.AppendWhenNotNullOrEmpty(".")}{inheritanceNameResult.Value}<{nameResult.Value}{genericTypeArgumentsString}, {instance.GetFullName()}{genericTypeArgumentsString}>");
        }

        return instance.GetCustomValueForInheritedClass
        (
            context.Context.Settings.EntitySettings.InheritanceSettings.EnableInheritance,
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

    private Result<string> GetBaseClassName(PipelineContext<IConcreteTypeBuilder, BuilderContext> context, IBaseClassContainer baseClassContainer)
    {
        var newContext = new PipelineContext<IConcreteTypeBuilder, BuilderContext>
        (
            context.Model,
            new BuilderContext(CreateTypeBase(context.Context.MapTypeName(baseClassContainer.BaseClass!)), context.Context.Settings, context.Context.FormatProvider)
        );

        return _formattableStringParser.Parse(context.Context.Settings.NameSettings.BuilderNameFormatString, context.Context.FormatProvider, newContext);
    }

    private static TypeBase CreateTypeBase(string baseClass)
        => new ClassBuilder()
            .WithNamespace(baseClass.GetNamespaceWithDefault())
            .WithName(baseClass.GetClassName())
            .Build();
}
