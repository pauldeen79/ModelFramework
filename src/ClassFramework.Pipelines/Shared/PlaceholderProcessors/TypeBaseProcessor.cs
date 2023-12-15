namespace ClassFramework.Pipelines.Shared.PlaceholderProcessors;

public class TypeBaseProcessor : IPipelinePlaceholderProcessor
{
    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        if (context is not PipelineContext<IType> pipelineContext)
        {
            return Result.Continue<string>();
        }

        var name = pipelineContext.Model.Name;
        var fullName = pipelineContext.Model.GetFullName();
        var ns = pipelineContext.Model.Namespace;

        return value switch
        {
            nameof(IType.Name) or $"Class.{nameof(IType.Name)}" => Result.Success(name),
            $"{nameof(IType.Name)}Lower" or $"Class.{nameof(IType.Name)}Lower" => Result.Success(name.ToLower(formatProvider.ToCultureInfo())),
            $"{nameof(IType.Name)}Upper" or $"Class.{nameof(IType.Name)}Upper" => Result.Success(name.ToUpper(formatProvider.ToCultureInfo())),
            $"{nameof(IType.Name)}Pascal" or $"Class.{nameof(IType.Name)}Pascal" => Result.Success(name.ToPascalCase(formatProvider.ToCultureInfo())),
            $"{nameof(IType.Namespace)}" or $"Class.{nameof(IType.Namespace)}" => Result.Success(ns),
            $"FullName" or "Class.FullName" => Result.Success(fullName),
            _ => Result.Continue<string>()
        };
    }
}
