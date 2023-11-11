namespace ClassFramework.Pipelines.Shared.PlaceholderProcessors;

public class TypeBaseProcessor : IPipelinePlaceholderProcessor
{
    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        if (context is not PipelineContext<TypeBase> pipelineContext)
        {
            return Result.Continue<string>();
        }

        var name = pipelineContext.Model.Name;
        var fullName = pipelineContext.Model.GetFullName();
        var ns = pipelineContext.Model.Namespace;

        return value switch
        {
            nameof(TypeBase.Name) or $"Class.{nameof(TypeBase.Name)}" => Result.Success(name),
            $"{nameof(TypeBase.Name)}Lower" or $"Class.{nameof(TypeBase.Name)}Lower" => Result.Success(name.ToLower(formatProvider.ToCultureInfo())),
            $"{nameof(TypeBase.Name)}Upper" or $"Class.{nameof(TypeBase.Name)}Upper" => Result.Success(name.ToUpper(formatProvider.ToCultureInfo())),
            $"{nameof(TypeBase.Name)}Pascal" or $"Class.{nameof(TypeBase.Name)}Pascal" => Result.Success(name.ToPascalCase(formatProvider.ToCultureInfo())),
            $"{nameof(TypeBase.Namespace)}" or $"Class.{nameof(TypeBase.Namespace)}" => Result.Success(ns),
            $"FullName" or "Class.FullName" => Result.Success(fullName),
            _ => Result.Continue<string>()
        };
    }
}
