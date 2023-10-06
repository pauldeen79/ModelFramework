namespace ClassFramework.Pipelines.Builder.PlaceholderProcessors;

public class ContextProcessor : IPlaceholderProcessor
{
    public int Order => 10;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        if (context is PipelineContext<ClassBuilder, BuilderPipelineBuilderContext> pipelineContext)
        {
            var model = pipelineContext.Context.SourceModel;
            return value switch
            {
                nameof(ClassBuilder.Name) => Result<string>.Success(model.Name),
                $"{nameof(ClassBuilder.Name)}Lower" => Result<string>.Success(model.Name.ToLower(formatProvider.ToCultureInfo())),
                $"{nameof(ClassBuilder.Name)}Upper" => Result<string>.Success(model.Name.ToUpper(formatProvider.ToCultureInfo())),
                $"{nameof(ClassBuilder.Name)}Pascal" => Result<string>.Success(model.Name.ToPascalCase(formatProvider.ToCultureInfo())),
                nameof(ClassBuilder.Namespace) => Result<string>.Success(model.Namespace),
                _ => Result<string>.Continue()
            };
        }

        return Result<string>.Continue();
    }
}
