namespace ClassFramework.Pipelines.Shared.PlaceholderProcessors;

public class ClassPropertyProcessor : IPipelinePlaceholderProcessor
{
    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        if (context is not PipelineContext<ClassProperty> pipelineContext)
        {
            return Result.Continue<string>();
        }

        return value switch
        {
            nameof(ClassProperty.Name) => Result.Success(pipelineContext.Model.Name),
            $"{nameof(ClassProperty.Name)}Lower" => Result.Success(pipelineContext.Model.Name.ToLower(formatProvider.ToCultureInfo())),
            $"{nameof(ClassProperty.Name)}Upper" => Result.Success(pipelineContext.Model.Name.ToUpper(formatProvider.ToCultureInfo())),
            $"{nameof(ClassProperty.Name)}Pascal" => Result.Success(pipelineContext.Model.Name.ToPascalCase(formatProvider.ToCultureInfo())),
            $"{nameof(ClassProperty.Name)}PascalCsharpFriendlyName" => Result.Success(pipelineContext.Model.Name.ToPascalCase(formatProvider.ToCultureInfo()).GetCsharpFriendlyName()),
            nameof(ClassProperty.TypeName) => Result.Success(pipelineContext.Model.TypeName.FixTypeName()),
            $"{nameof(ClassProperty.TypeName)}.GenericArguments" => Result.Success(pipelineContext.Model.TypeName.FixTypeName().GetGenericArguments()),
            $"{nameof(ClassProperty.TypeName)}.GenericArgumentsWithBrackets" => Result.Success(pipelineContext.Model.TypeName.FixTypeName().GetGenericArguments(addBrackets: true)),
            $"{nameof(ClassProperty.TypeName)}.GenericArguments.ClassName" => Result.Success(pipelineContext.Model.TypeName.FixTypeName().GetGenericArguments().GetClassName()),
            $"{nameof(ClassProperty.TypeName)}.ClassName" => Result.Success(pipelineContext.Model.TypeName.FixTypeName().GetClassName()),
            $"{nameof(ClassProperty.TypeName)}.Namespace" => Result.Success(pipelineContext.Model.TypeName.FixTypeName().GetNamespaceWithDefault()),
            $"{nameof(ClassProperty.TypeName)}.NoGenerics" => Result.Success(pipelineContext.Model.TypeName.FixTypeName().WithoutProcessedGenerics()),
            _ => Result.Continue<string>()
        };
    }
}
