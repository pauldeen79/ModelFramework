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
            return value switch
            {
                "EntityNamespace" => formattableStringParser.Parse(pipelineContext.Context.Settings.NameSettings.EntityNamespaceFormatString, pipelineContext.Context.FormatProvider, pipelineContext.Context),
                "EntityNameSuffix" => Result.Success(pipelineContext.Context.Settings.ConstructorSettings.ValidateArguments == ArgumentValidationType.Shared
                    ? "Base"
                    : string.Empty),
                _ => _pipelinePlaceholderProcessors.Select(x => x.Process(value, formatProvider, new PipelineContext<TypeBase>(pipelineContext.Context.Model), formattableStringParser)).FirstOrDefault(x => x.Status != ResultStatus.Continue)
                    ?? Result.Continue<string>()
            };
        }

        return Result.Continue<string>();
    }
}
