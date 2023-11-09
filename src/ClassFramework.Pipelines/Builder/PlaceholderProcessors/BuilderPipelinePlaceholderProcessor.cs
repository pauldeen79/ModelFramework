namespace ClassFramework.Pipelines.Builder.PlaceholderProcessors;

public class BuilderPipelinePlaceholderProcessor : IPlaceholderProcessor
{
    private readonly IEnumerable<IPipelinePlaceholderProcessor> _pipelinePlaceholderProcessors;

    public BuilderPipelinePlaceholderProcessor(IEnumerable<IPipelinePlaceholderProcessor> pipelinePlaceholderProcessors)
    {
        _pipelinePlaceholderProcessors = pipelinePlaceholderProcessors.IsNotNull(nameof(pipelinePlaceholderProcessors));
    }

    public int Order => 20;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));

        if (context is PipelineContext<ClassBuilder, BuilderContext> pipelineContext)
        {
            return GetResultForPipelineContext(value, formatProvider, formattableStringParser, pipelineContext);
        }

        if (context is ParentChildContext<BuilderContext, ClassProperty> parentChildContext)
        {
            return GetResultForParentChildContext(value, formatProvider, formattableStringParser, parentChildContext);
        }

        return Result.Continue<string>();
    }

    private Result<string> GetResultForPipelineContext(string value, IFormatProvider formatProvider, IFormattableStringParser formattableStringParser, PipelineContext<ClassBuilder, BuilderContext> pipelineContext)
        => value switch
        {
            "NullCheck.Source" => Result.Success(pipelineContext.Context.Settings.GenerationSettings.AddNullChecks
                ? $"if (source is null) throw new {typeof(ArgumentNullException).FullName}(nameof(source));"
                : string.Empty),
            "BuildersNamespace" => formattableStringParser.Parse(pipelineContext.Context.Settings.NameSettings.BuilderNamespaceFormatString, pipelineContext.Context.FormatProvider, pipelineContext.Context),
            _ => _pipelinePlaceholderProcessors.Select(x => x.Process(value, formatProvider, new PipelineContext<TypeBase>(pipelineContext.Context.SourceModel), formattableStringParser)).FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Continue<string>()
        };

    private Result<string> GetResultForParentChildContext(string value, IFormatProvider formatProvider, IFormattableStringParser formattableStringParser, ParentChildContext<BuilderContext, ClassProperty> parentChildContext)
        => value switch
        {
            "NullCheck.Source.Argument" => Result.Success(parentChildContext.ParentContext.Context.Settings.GenerationSettings.AddNullChecks
                ? $"if (source.{parentChildContext.ChildContext.Name} is not null) "
                : string.Empty),
            "NullCheck.Argument" => Result.Success(parentChildContext.ParentContext.Context.Settings.GenerationSettings.AddNullChecks
                ? $"if ({parentChildContext.ChildContext.Name.ToPascalCase(formatProvider.ToCultureInfo()).GetCsharpFriendlyName()} is null) throw new {typeof(ArgumentNullException).FullName}(nameof({parentChildContext.ChildContext.Name.ToPascalCase(formatProvider.ToCultureInfo()).GetCsharpFriendlyName()}));"
                : string.Empty),
            "NullableRequiredSuffix" => Result.Success(parentChildContext.ChildContext.IsNullable || !parentChildContext.ParentContext.Context.Settings.GenerationSettings.EnableNullableReferenceTypes
                ? string.Empty
                : "!"),
            "NullableSuffix" => Result.Success(parentChildContext.ChildContext.IsNullable && (parentChildContext.ChildContext.IsValueType || parentChildContext.ParentContext.Context.Settings.GenerationSettings.EnableNullableReferenceTypes)
                ? "?"
                : string.Empty),
            "BuildersNamespace" => formattableStringParser.Parse(parentChildContext.ParentContext.Context.Settings.NameSettings.BuilderNamespaceFormatString, parentChildContext.ParentContext.Context.FormatProvider, parentChildContext.ParentContext.Context),
            _ => _pipelinePlaceholderProcessors.Select(x => x.Process(value, formatProvider, new PipelineContext<ClassProperty>(parentChildContext.ChildContext), formattableStringParser)).FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? _pipelinePlaceholderProcessors.Select(x => x.Process(value, formatProvider, new PipelineContext<TypeBase>(parentChildContext.ParentContext.Context.SourceModel), formattableStringParser)).FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Continue<string>()
        };
}
