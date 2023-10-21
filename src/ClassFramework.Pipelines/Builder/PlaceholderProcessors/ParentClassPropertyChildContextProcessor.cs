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
            "NullCheck.Source" => Result.Success(parentChildContext.ParentContext.Context.Settings.GenerationSettings.AddNullChecks
                ? $"if (source.{parentChildContext.ChildContext.Name} is not null) "
                : string.Empty),
            "NullCheck.Argument" => Result.Success(parentChildContext.ParentContext.Context.Settings.GenerationSettings.AddNullChecks
                ? $"if ({parentChildContext.ChildContext.Name.ToPascalCase(formatProvider.ToCultureInfo())} is null) throw new {typeof(ArgumentNullException).FullName}(nameof({parentChildContext.ChildContext.Name.ToPascalCase(formatProvider.ToCultureInfo())}));"
                : string.Empty),
            "NullableRequiredSuffix" => Result.Success(parentChildContext.ChildContext.IsNullable || !parentChildContext.ParentContext.Context.Settings.GenerationSettings.EnableNullableReferenceTypes
                ? string.Empty
                : "!"),
            "NullableSuffix" => Result.Success(parentChildContext.ChildContext.IsNullable && (parentChildContext.ChildContext.IsValueType || parentChildContext.ParentContext.Context.Settings.GenerationSettings.EnableNullableReferenceTypes)
                ? "?"
                : string.Empty),
            "BuildersNamespace" => formattableStringParser.Parse(parentChildContext.ParentContext.Context.Settings.NameSettings.BuilderNamespaceFormatString, parentChildContext.ParentContext.Context.FormatProvider, parentChildContext.ParentContext.Context),
            nameof(ClassProperty.Name) => Result.Success(parentChildContext.ChildContext.Name),
            $"{nameof(ClassProperty.Name)}Lower" => Result.Success(parentChildContext.ChildContext.Name.ToLower(formatProvider.ToCultureInfo())),
            $"{nameof(ClassProperty.Name)}Upper" => Result.Success(parentChildContext.ChildContext.Name.ToUpper(formatProvider.ToCultureInfo())),
            $"{nameof(ClassProperty.Name)}Pascal" => Result.Success(parentChildContext.ChildContext.Name.ToPascalCase(formatProvider.ToCultureInfo())),
            nameof(ClassProperty.TypeName) => Result.Success(parentChildContext.ChildContext.TypeName.FixTypeName()),
            $"{nameof(ClassProperty.TypeName)}.GenericArguments" => Result.Success(parentChildContext.ChildContext.TypeName.FixTypeName().GetGenericArguments()),
            $"{nameof(ClassProperty.TypeName)}.GenericArgumentsWithBrackets" => Result.Success(parentChildContext.ChildContext.TypeName.FixTypeName().GetGenericArguments(addBrackets: true)),
            $"{nameof(ClassProperty.TypeName)}.GenericArguments.ClassName" => Result.Success(parentChildContext.ChildContext.TypeName.FixTypeName().GetGenericArguments().GetClassName()),
            $"{nameof(ClassProperty.TypeName)}.ClassName" => Result.Success(parentChildContext.ChildContext.TypeName.FixTypeName().GetClassName()),
            $"{nameof(ClassProperty.TypeName)}.Namespace" => Result.Success(parentChildContext.ChildContext.TypeName.FixTypeName().GetNamespaceWithDefault()),
            $"{nameof(ClassProperty.TypeName)}.NoGenerics" => Result.Success(parentChildContext.ChildContext.TypeName.FixTypeName().WithoutProcessedGenerics()),
            $"Class.{nameof(Class.Name)}" => Result.Success(parentChildContext.ParentContext.Context.SourceModel.Name),
            $"Class.{nameof(Class.Name)}Lower" => Result.Success(parentChildContext.ParentContext.Context.SourceModel.Name.ToLower(formatProvider.ToCultureInfo())),
            $"Class.{nameof(Class.Name)}Upper" => Result.Success(parentChildContext.ParentContext.Context.SourceModel.Name.ToUpper(formatProvider.ToCultureInfo())),
            $"Class.{nameof(Class.Name)}Pascal" => Result.Success(parentChildContext.ParentContext.Context.SourceModel.Name.ToPascalCase(formatProvider.ToCultureInfo())),
            $"Class.{nameof(Class.Namespace)}" => Result.Success(parentChildContext.ParentContext.Context.SourceModel.Namespace),
            $"Class.FullName" => Result.Success(parentChildContext.ParentContext.Context.SourceModel.GetFullName()),
            _ => Result.Continue<string>()
        };
    }
}
