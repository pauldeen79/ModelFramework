namespace ClassFramework.Pipelines.Entity.PlaceholderProcessors;

public class EntityPipelinePlaceholderProcessor : IPlaceholderProcessor
{
    private readonly IEnumerable<IPipelinePlaceholderProcessor> _pipelinePlaceholderProcessors;

    public EntityPipelinePlaceholderProcessor(IEnumerable<IPipelinePlaceholderProcessor> pipelinePlaceholderProcessors)
    {
        _pipelinePlaceholderProcessors = pipelinePlaceholderProcessors.IsNotNull(nameof(pipelinePlaceholderProcessors));
    }

    public int Order => 10;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));

        if (context is PipelineContext<ClassBuilder, EntityContext> pipelineContext)
        {
            return GetResultForPipelineContext(value, formatProvider, formattableStringParser, pipelineContext);
        }

        if (context is ParentChildContext<EntityContext, ClassProperty> parentChildContext)
        {
            return GetResultForParentChildContext(value, formatProvider, formattableStringParser, parentChildContext);
        }

        return Result.Continue<string>();
    }

    private Result<string> GetResultForPipelineContext(string value, IFormatProvider formatProvider, IFormattableStringParser formattableStringParser, PipelineContext<ClassBuilder, EntityContext> pipelineContext)
        => value switch
        {
            "EntityNamespace" => formattableStringParser.Parse(pipelineContext.Context.Settings.NameSettings.EntityNamespaceFormatString, pipelineContext.Context.FormatProvider, pipelineContext.Context),
            "EntityNameSuffix" => Result.Success(pipelineContext.Context.Settings.ConstructorSettings.ValidateArguments == ArgumentValidationType.Shared
                ? "Base"
                : string.Empty),
            _ => _pipelinePlaceholderProcessors.Select(x => x.Process(value, formatProvider, new PipelineContext<TypeBase>(pipelineContext.Context.SourceModel), formattableStringParser)).FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Continue<string>()
        };

    private Result<string> GetResultForParentChildContext(string value, IFormatProvider formatProvider, IFormattableStringParser formattableStringParser, ParentChildContext<EntityContext, ClassProperty> parentChildContext)
        => value switch
        {
            "EntityNamespace" => formattableStringParser.Parse(parentChildContext.ParentContext.Context.Settings.NameSettings.EntityNamespaceFormatString, parentChildContext.ParentContext.Context.FormatProvider, parentChildContext.ParentContext.Context),
            "NullableRequiredSuffix" => Result.Success(parentChildContext.ParentContext.Context.Settings.NullCheckSettings.AddNullChecks || parentChildContext.ChildContext.IsValueType || parentChildContext.ChildContext.IsNullable || !parentChildContext.ParentContext.Context.Settings.TypeSettings.EnableNullableReferenceTypes
                ? string.Empty
                : "!"),
            _ => _pipelinePlaceholderProcessors.Select(x => x.Process(value, formatProvider, new ClassPropertyContext(parentChildContext.ChildContext, parentChildContext.Settings, formatProvider, parentChildContext.ParentContext.Context.MapTypeName(parentChildContext.ChildContext.TypeName)), formattableStringParser)).FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? _pipelinePlaceholderProcessors.Select(x => x.Process(value, formatProvider, new PipelineContext<TypeBase>(parentChildContext.ParentContext.Context.SourceModel), formattableStringParser)).FirstOrDefault(x => x.Status != ResultStatus.Continue)
                ?? Result.Continue<string>()
        };
}
