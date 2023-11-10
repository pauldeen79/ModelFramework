namespace ClassFramework.Pipelines.Shared.PlaceholderProcessors;

public class ClassPropertyProcessor : IPipelinePlaceholderProcessor
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public ClassPropertyProcessor(ICsharpExpressionCreator csharpExpressionCreator)
    {
        _csharpExpressionCreator = csharpExpressionCreator.IsNotNull(nameof(csharpExpressionCreator));
    }

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));

        if (context is not ClassPropertyContext classPropertyContext)
        {
            return Result.Continue<string>();
        }

        return value switch
        {
            nameof(ClassProperty.Name) => Result.Success(classPropertyContext.SourceModel.Name),
            $"{nameof(ClassProperty.Name)}Lower" => Result.Success(classPropertyContext.SourceModel.Name.ToLower(formatProvider.ToCultureInfo())),
            $"{nameof(ClassProperty.Name)}Upper" => Result.Success(classPropertyContext.SourceModel.Name.ToUpper(formatProvider.ToCultureInfo())),
            $"{nameof(ClassProperty.Name)}Pascal" => Result.Success(classPropertyContext.SourceModel.Name.ToPascalCase(formatProvider.ToCultureInfo())),
            $"{nameof(ClassProperty.Name)}PascalCsharpFriendlyName" => Result.Success(classPropertyContext.SourceModel.Name.ToPascalCase(formatProvider.ToCultureInfo()).GetCsharpFriendlyName()),
            nameof(ClassProperty.TypeName) => Result.Success(classPropertyContext.SourceModel.TypeName.FixTypeName()),
            $"{nameof(ClassProperty.TypeName)}.GenericArguments" => Result.Success(classPropertyContext.SourceModel.TypeName.FixTypeName().GetGenericArguments()),
            $"{nameof(ClassProperty.TypeName)}.GenericArgumentsWithBrackets" => Result.Success(classPropertyContext.SourceModel.TypeName.FixTypeName().GetGenericArguments(addBrackets: true)),
            $"{nameof(ClassProperty.TypeName)}.GenericArguments.ClassName" => Result.Success(classPropertyContext.SourceModel.TypeName.FixTypeName().GetGenericArguments().GetClassName()),
            $"{nameof(ClassProperty.TypeName)}.ClassName" => Result.Success(classPropertyContext.SourceModel.TypeName.FixTypeName().GetClassName()),
            $"{nameof(ClassProperty.TypeName)}.Namespace" => Result.Success(classPropertyContext.SourceModel.TypeName.FixTypeName().GetNamespaceWithDefault()),
            $"{nameof(ClassProperty.TypeName)}.NoGenerics" => Result.Success(classPropertyContext.SourceModel.TypeName.FixTypeName().WithoutProcessedGenerics()),
            "DefaultValue" => formattableStringParser.Parse(classPropertyContext.SourceModel.GetDefaultValue(_csharpExpressionCreator, classPropertyContext.Settings.EnableNullableReferenceTypes), formatProvider, context),
            _ => Result.Continue<string>()
        };
    }
}
