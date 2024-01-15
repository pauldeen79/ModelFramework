namespace ClassFramework.Pipelines.Shared.PlaceholderProcessors;

public class TypeProcessor : IPipelinePlaceholderProcessor
{
    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        if (context is not PipelineContext<Type> pipelineContext)
        {
            return Result.Continue<string>();
        }

        var name = pipelineContext.Model.Name;
        var nameNoInterfacePrefix = pipelineContext.Model.IsInterface
            && pipelineContext.Model.Name.StartsWith("I")
            && pipelineContext.Model.Name.Length >= 2
            && pipelineContext.Model.Name.Substring(1, 1).Equals(pipelineContext.Model.Name.Substring(1, 1).ToUpperInvariant(), StringComparison.Ordinal)
            ? name.Substring(1)
            : name;

        var fullName = pipelineContext.Model.FullName;
        var ns = pipelineContext.Model.Namespace;

        return value switch
        {
            nameof(Type.Name) or $"Class.{nameof(Type.Name)}" => Result.Success(name),
            $"{nameof(Type.Name)}Lower" or $"Class.{nameof(Type.Name)}Lower" => Result.Success(name.ToLower(formatProvider.ToCultureInfo())),
            $"{nameof(Type.Name)}Upper" or $"Class.{nameof(Type.Name)}Upper" => Result.Success(name.ToUpper(formatProvider.ToCultureInfo())),
            $"{nameof(Type.Name)}Pascal" or $"Class.{nameof(Type.Name)}Pascal" => Result.Success(name.ToPascalCase(formatProvider.ToCultureInfo())),
            $"{nameof(Type.Namespace)}" or $"Class.{nameof(Type.Namespace)}" => Result.Success(ns),
            $"{nameof(Type.FullName)}" or $"Class.{nameof(Type.FullName)}" => Result.Success(fullName),
            $"{nameof(Type.Name)}NoInterfacePrefix" or $"Class.{nameof(Type.Name)}NoInterfacePrefix" => Result.Success(nameNoInterfacePrefix),
            _ => Result.Continue<string>()
        };
    }
}
