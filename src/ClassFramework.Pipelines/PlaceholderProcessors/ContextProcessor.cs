namespace ClassFramework.Pipelines.PlaceholderProcessors;

public class ContextProcessor : IPlaceholderProcessor
{
    public int Order => 10;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        if (context is PipelineContext<TypeBuilder, BuilderPipelineBuilderSettings> pipelineContext)
        {
            return value switch
            {
                nameof(TypeBuilder.Name) => Result<string>.Success(pipelineContext.Model.Name),
                $"{nameof(TypeBuilder.Name)}Lower" => Result<string>.Success(pipelineContext.Model.Name.ToLower(formatProvider as CultureInfo ?? CultureInfo.CurrentCulture)),
                $"{nameof(TypeBuilder.Name)}Upper" => Result<string>.Success(pipelineContext.Model.Name.ToUpper(formatProvider as CultureInfo ?? CultureInfo.CurrentCulture)),
                $"{nameof(TypeBuilder.Name)}Pascal" => Result<string>.Success(pipelineContext.Model.Name.ToPascalCase()),
                nameof(TypeBuilder.Namespace) => Result<string>.Success(pipelineContext.Model.Namespace),
                _ => Result<string>.Continue()
            };
        }

        return Result<string>.Continue();
    }
}
