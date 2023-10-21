namespace ClassFramework.Pipelines.Builder.PlaceholderProcessors;

public class ContextSourceModelProcessor : IPlaceholderProcessor
{
    public int Order => 10;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        if (context is not PipelineContext<ClassBuilder, BuilderContext> pipelineContext)
        {
            return Result.Continue<string>();
        }

        var model = pipelineContext.Context.SourceModel;

        return value switch
        {
            nameof(TypeBase.Name) or $"Class.{nameof(TypeBase.Name)}" => Result.Success(model.Name),
            $"{nameof(TypeBase.Name)}Lower" => Result.Success(model.Name.ToLower(formatProvider.ToCultureInfo())),
            $"{nameof(TypeBase.Name)}Upper" => Result.Success(model.Name.ToUpper(formatProvider.ToCultureInfo())),
            $"{nameof(TypeBase.Name)}Pascal" => Result.Success(model.Name.ToPascalCase(formatProvider.ToCultureInfo())),
            nameof(TypeBase.Namespace) => Result.Success(model.Namespace),
            _ => Result.Continue<string>()
        };
    }
}
