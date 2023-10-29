namespace ClassFramework.Pipelines.Shared.PlaceholderProcessors;

public class TypeBaseProcessor : IPipelinePlaceholderProcessor
{
    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        if (context is not PipelineContext<TypeBase> pipelineContext)
        {
            return Result.Continue<string>();
        }

        return value switch
        {
            nameof(TypeBase.Name) or $"Class.{nameof(TypeBase.Name)}" => Result.Success(pipelineContext.Model.Name),
            $"{nameof(TypeBase.Name)}Lower" or $"Class.{nameof(TypeBase.Name)}Lower" => Result.Success(pipelineContext.Model.Name.ToLower(formatProvider.ToCultureInfo())),
            $"{nameof(TypeBase.Name)}Upper" or $"Class.{nameof(TypeBase.Name)}Upper" => Result.Success(pipelineContext.Model.Name.ToUpper(formatProvider.ToCultureInfo())),
            $"{nameof(TypeBase.Name)}Pascal" or $"Class.{nameof(TypeBase.Name)}Pascal" => Result.Success(pipelineContext.Model.Name.ToPascalCase(formatProvider.ToCultureInfo())),
            $"{nameof(TypeBase.Namespace)}" or $"Class.{nameof(TypeBase.Namespace)}" => Result.Success(pipelineContext.Model.Namespace),
            $"FullName" or "Class.FullName" => Result.Success(pipelineContext.Model.GetFullName()),
            _ => Result.Continue<string>()
        };
    }
}
