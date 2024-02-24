namespace ClassFramework.Pipelines.BuilderInterface.PlaceholderProcessors;

public class BuilderInterfacePipelinePlaceholderProcessor : IPlaceholderProcessor
{
    private readonly IEnumerable<IPipelinePlaceholderProcessor> _pipelinePlaceholderProcessors;

    public BuilderInterfacePipelinePlaceholderProcessor(IEnumerable<IPipelinePlaceholderProcessor> pipelinePlaceholderProcessors)
    {
        _pipelinePlaceholderProcessors = pipelinePlaceholderProcessors.IsNotNull(nameof(pipelinePlaceholderProcessors));
    }

    public int Order => 20;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));

        if (context is PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> pipelineContext)
        {
            return GetResultForPipelineContext(value, formatProvider, formattableStringParser, pipelineContext);
        }

        if (context is ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext>, Property> parentChildContext)
        {
            return GetResultForParentChildContext(value, formatProvider, formattableStringParser, parentChildContext);
        }

        return Result.Continue<string>();
    }

    private Result<string> GetResultForPipelineContext(string value, IFormatProvider formatProvider, IFormattableStringParser formattableStringParser, PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext> pipelineContext)
        => value switch
        {
            "NullCheck.Source" => Result.Success(pipelineContext.Context.Settings.AddNullChecks
                ? pipelineContext.Context.CreateArgumentNullException("source")
                : string.Empty),
            "BuildersNamespace" => formattableStringParser.Parse(pipelineContext.Context.Settings.BuilderNamespaceFormatString, pipelineContext.Context.FormatProvider, pipelineContext.Context),
            _ => _pipelinePlaceholderProcessors.Select(x => x.Process(value, formatProvider, new PipelineContext<IType>(pipelineContext.Context.SourceModel), formattableStringParser)).FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Continue<string>()
        };

    private Result<string> GetResultForParentChildContext(string value, IFormatProvider formatProvider, IFormattableStringParser formattableStringParser, ParentChildContext<PipelineContext<IConcreteTypeBuilder, BuilderInterfaceContext>, Property> parentChildContext)
        => value switch
        {
            "NullCheck.Source.Argument" => Result.Success(parentChildContext.ParentContext.Context.Settings.AddNullChecks && parentChildContext.ParentContext.Context.Settings.AddValidationCode == ArgumentValidationType.None && !parentChildContext.ChildContext.IsNullable && !parentChildContext.ChildContext.IsValueType // only if the source entity does not use validation...
                ? $"if (source.{parentChildContext.ChildContext.Name} is not null) "
                : string.Empty),
            "NullCheck.Argument" => Result.Success(parentChildContext.ParentContext.Context.Settings.AddNullChecks && !parentChildContext.ChildContext.IsValueType && !parentChildContext.ChildContext.IsNullable
                ? parentChildContext.ParentContext.Context.CreateArgumentNullException(parentChildContext.ChildContext.Name.ToPascalCase(formatProvider.ToCultureInfo()).GetCsharpFriendlyName())
                : string.Empty),
            "NullableRequiredSuffix" => Result.Success(!parentChildContext.ParentContext.Context.Settings.AddNullChecks && !parentChildContext.ChildContext.IsValueType && parentChildContext.ChildContext.IsNullable && parentChildContext.ParentContext.Context.Settings.EnableNullableReferenceTypes
                ? "!"
                : string.Empty),
            "NullableSuffix" => Result.Success(parentChildContext.ChildContext.IsNullable && (parentChildContext.ChildContext.IsValueType || parentChildContext.ParentContext.Context.Settings.EnableNullableReferenceTypes)
                ? "?"
                : string.Empty),
            "BuildersNamespace" => formattableStringParser.Parse(parentChildContext.ParentContext.Context.Settings.BuilderNamespaceFormatString, parentChildContext.ParentContext.Context.FormatProvider, parentChildContext.ParentContext.Context),
            _ => _pipelinePlaceholderProcessors.Select(x => x.Process(value, formatProvider, new PropertyContext(parentChildContext.ChildContext, parentChildContext.Settings, formatProvider, parentChildContext.ParentContext.Context.MapTypeName(parentChildContext.ChildContext.TypeName), parentChildContext.Settings.BuilderNewCollectionTypeName), formattableStringParser)).FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? _pipelinePlaceholderProcessors.Select(x => x.Process(value, formatProvider, new PipelineContext<IType>(parentChildContext.ParentContext.Context.SourceModel), formattableStringParser)).FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Continue<string>()
        };
}
