namespace ClassFramework.Pipelines.Builder.PlaceholderProcessors;

public class ParentClassPropertyChildContextProcessor : IPlaceholderProcessor
{
    public int Order => 30;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));

        if (context is not ParentChildContext<ClassProperty> parentChildContext)
        {
            return Result.Continue<string>();
        }

        return value switch
        {
            "NullCheck.Source" => Result<string>.Success(parentChildContext.ParentContext.Context.Settings.GenerationSettings.AddNullChecks
                ? $"if (source.{parentChildContext.ChildContext.Name} is not null) "
                : string.Empty),
            "NullableRequiredSuffix" => Result<string>.Success(parentChildContext.ChildContext.IsNullable || !parentChildContext.ParentContext.Context.Settings.GenerationSettings.EnableNullableReferenceTypes
                ? string.Empty
                : "!"),
            "NullableSuffix" => Result<string>.Success(parentChildContext.ChildContext.IsNullable && (parentChildContext.ChildContext.IsValueType || parentChildContext.ParentContext.Context.Settings.GenerationSettings.EnableNullableReferenceTypes)
                ? "?"
                : string.Empty),
            "BuildersNamespace" => formattableStringParser.Parse(parentChildContext.ParentContext.Context.Settings.NameSettings.BuilderNamespaceFormatString, parentChildContext.ParentContext.Context.FormatProvider, parentChildContext.ParentContext.Context),
            nameof(ClassProperty.Name) => Result<string>.Success(parentChildContext.ChildContext.Name),
            $"{nameof(ClassProperty.Name)}Lower" => Result<string>.Success(parentChildContext.ChildContext.Name.ToLower(formatProvider.ToCultureInfo())),
            $"{nameof(ClassProperty.Name)}Upper" => Result<string>.Success(parentChildContext.ChildContext.Name.ToUpper(formatProvider.ToCultureInfo())),
            $"{nameof(ClassProperty.Name)}Pascal" => Result<string>.Success(parentChildContext.ChildContext.Name.ToPascalCase(formatProvider.ToCultureInfo())),
            nameof(ClassProperty.TypeName) => Result<string>.Success(parentChildContext.ChildContext.TypeName.FixTypeName()),
            $"{nameof(ClassProperty.TypeName)}.GenericArguments" => Result<string>.Success(parentChildContext.ChildContext.TypeName.FixTypeName().GetGenericArguments()),
            $"{nameof(ClassProperty.TypeName)}.GenericArgumentsWithBrackets" => Result<string>.Success(parentChildContext.ChildContext.TypeName.FixTypeName().GetGenericArguments(addBrackets: true)),
            $"{nameof(ClassProperty.TypeName)}.GenericArguments.ClassName" => Result<string>.Success(parentChildContext.ChildContext.TypeName.FixTypeName().GetGenericArguments().GetClassName()),
            $"{nameof(ClassProperty.TypeName)}.ClassName" => Result<string>.Success(parentChildContext.ChildContext.TypeName.FixTypeName().GetClassName()),
            $"{nameof(ClassProperty.TypeName)}.Namespace" => Result<string>.Success(parentChildContext.ChildContext.TypeName.FixTypeName().GetNamespaceWithDefault()),
            $"{nameof(ClassProperty.TypeName)}.NoGenerics" => Result<string>.Success(parentChildContext.ChildContext.TypeName.FixTypeName().WithoutProcessedGenerics()),
            $"Class.{nameof(Class.Name)}" => Result<string>.Success(parentChildContext.ParentContext.Context.SourceModel.Name),
            $"Class.{nameof(Class.Name)}Lower" => Result<string>.Success(parentChildContext.ParentContext.Context.SourceModel.Name.ToLower(formatProvider.ToCultureInfo())),
            $"Class.{nameof(Class.Name)}Upper" => Result<string>.Success(parentChildContext.ParentContext.Context.SourceModel.Name.ToUpper(formatProvider.ToCultureInfo())),
            $"Class.{nameof(Class.Name)}Pascal" => Result<string>.Success(parentChildContext.ParentContext.Context.SourceModel.Name.ToPascalCase(formatProvider.ToCultureInfo())),
            $"Class.{nameof(Class.Namespace)}" => Result<string>.Success(parentChildContext.ParentContext.Context.SourceModel.Namespace),
            $"Class.FullName" => Result<string>.Success(parentChildContext.ParentContext.Context.SourceModel.GetFullName()),
            _ => Result.Continue<string>()
        };
    }
}
