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
        var nameNoInterfacePrefix = pipelineContext.Model is Interface && pipelineContext.Model.Name.StartsWith("I")
            ? name.Substring(1)
            : name;

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
            $"{nameof(IType.Name)}NoInterfacePrefix" or $"Class.{nameof(IType.Name)}NoInterfacePrefix" => Result.Success(nameNoInterfacePrefix),
            _ => Result.Continue<string>()
        };
    }
}
