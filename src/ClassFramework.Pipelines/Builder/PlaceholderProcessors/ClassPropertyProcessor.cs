namespace ClassFramework.Pipelines.Builder.PlaceholderProcessors;

public class ClassPropertyProcessor : IPlaceholderProcessor
{
    public int Order => 20;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        if (context is not PipelineContext<ClassProperty, BuilderContext> pipelineContext)
        {
            return Result<string>.Continue();
        }

        var model = pipelineContext.Model;

        return Process(value, formatProvider, model);
    }

    internal static Result<string> Process(string value, IFormatProvider formatProvider, ClassProperty model)
        => value switch
        {
            nameof(ClassProperty.Name) => Result<string>.Success(model.Name),
            $"{nameof(ClassProperty.Name)}Lower" => Result<string>.Success(model.Name.ToLower(formatProvider.ToCultureInfo())),
            $"{nameof(ClassProperty.Name)}Upper" => Result<string>.Success(model.Name.ToUpper(formatProvider.ToCultureInfo())),
            $"{nameof(ClassProperty.Name)}Pascal" => Result<string>.Success(model.Name.ToPascalCase(formatProvider.ToCultureInfo())),
            nameof(ClassProperty.TypeName) => Result<string>.Success(model.TypeName.FixTypeName()),
            $"{nameof(ClassProperty.TypeName)}.GenericArguments" => Result<string>.Success(model.TypeName.FixTypeName().GetGenericArguments()),
            $"{nameof(ClassProperty.TypeName)}.GenericArgumentsWithBrackets" => Result<string>.Success(model.TypeName.FixTypeName().GetGenericArguments(addBrackets: true)),
            $"{nameof(ClassProperty.TypeName)}.GenericArguments.ClassName" => Result<string>.Success(model.TypeName.FixTypeName().GetGenericArguments().GetClassName()),
            $"{nameof(ClassProperty.TypeName)}.ClassName" => Result<string>.Success(model.TypeName.FixTypeName().GetClassName()),
            $"{nameof(ClassProperty.TypeName)}.Namespace" => Result<string>.Success(model.TypeName.FixTypeName().GetNamespaceWithDefault()),
            $"{nameof(ClassProperty.TypeName)}.NoGenerics" => Result<string>.Success(model.TypeName.FixTypeName().WithoutProcessedGenerics()),
            "NullableSuffix" => Result<string>.Success(model.IsNullable
                ? "?"
                : string.Empty),
            _ => Result<string>.Continue()
        };
}
