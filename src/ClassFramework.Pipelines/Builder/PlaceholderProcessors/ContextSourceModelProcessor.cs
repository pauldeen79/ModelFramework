namespace ClassFramework.Pipelines.Builder.PlaceholderProcessors;

public class ContextSourceModelProcessor : IPlaceholderProcessor
{
    public int Order => 10;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        if (context is PipelineContext<ClassBuilder, BuilderPipelineBuilderContext> pipelineContext)
        {
            var model = pipelineContext.Context.SourceModel;
            return value switch
            {
                nameof(TypeBase.Name) => Result<string>.Success(model.Name),
                $"{nameof(TypeBase.Name)}Lower" => Result<string>.Success(model.Name.ToLower(formatProvider.ToCultureInfo())),
                $"{nameof(TypeBase.Name)}Upper" => Result<string>.Success(model.Name.ToUpper(formatProvider.ToCultureInfo())),
                $"{nameof(TypeBase.Name)}Pascal" => Result<string>.Success(model.Name.ToPascalCase(formatProvider.ToCultureInfo())),
                nameof(TypeBase.Namespace) => Result<string>.Success(model.Namespace),
                _ => Result<string>.Continue()
            };
        }

        return Result<string>.Continue();
    }
}
