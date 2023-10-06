namespace ClassFramework.Pipelines.Builder.PlaceholderProcessors;

public class ContextProcessor : IPlaceholderProcessor
{
    public int Order => 10;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        if (context is PipelineContext<ClassBuilder, BuilderPipelineBuilderSettings> pipelineContext)
        {
            return value switch
            {
                nameof(ClassBuilder.Name) => Result<string>.Success(pipelineContext.Model.Name),
                $"{nameof(ClassBuilder.Name)}Lower" => Result<string>.Success(pipelineContext.Model.Name.ToLower(formatProvider as CultureInfo ?? CultureInfo.CurrentCulture)),
                $"{nameof(ClassBuilder.Name)}Upper" => Result<string>.Success(pipelineContext.Model.Name.ToUpper(formatProvider as CultureInfo ?? CultureInfo.CurrentCulture)),
                $"{nameof(ClassBuilder.Name)}Pascal" => Result<string>.Success(pipelineContext.Model.Name.ToPascalCase()),
                nameof(ClassBuilder.Namespace) => Result<string>.Success(pipelineContext.Model.Namespace),
                _ => Result<string>.Continue()
            };
        }

        return Result<string>.Continue();
    }
}
