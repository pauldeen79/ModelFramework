namespace ClassFramework.Pipelines.Builder.PlaceholderProcessors;

public class ParentClassPropertyChildContextProcessor : IPlaceholderProcessor
{
    public int Order => 30;

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));

        if (context is not ParentChildContext<ClassProperty> parentChildContext)
        {
            return Result<string>.Continue();
        }

        var typeName = parentChildContext.ParentContext.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate is not null
            ? parentChildContext.ParentContext.Context.Settings.TypeSettings.FormatInstanceTypeNameDelegate!.Invoke(parentChildContext.ParentContext.Context.SourceModel, true)
            : parentChildContext.ChildContext.TypeName;

        return value switch
        {
            "NullCheck.Source" => Result<string>.Success(parentChildContext.ParentContext.Context.Settings.GenerationSettings.AddNullChecks
                ? $"if (source.{parentChildContext.ChildContext.Name} is not null) "
                : string.Empty),
            "NullableRequiredSuffix" => Result<string>.Success(parentChildContext.ChildContext.IsNullable || !parentChildContext.ParentContext.Context.Settings.GenerationSettings.EnableNullableReferenceTypes
                ? string.Empty
                : "!"),
            "BuildersNamespace" => formattableStringParser.Parse(parentChildContext.ParentContext.Context.Settings.NameSettings.BuilderNamespaceFormatString, parentChildContext.ParentContext.Context.FormatProvider, parentChildContext.ParentContext.Context),

            // overrides of property typename (needed in case the FormatInstanceTypeNameDelegate is filled)
            nameof(ClassProperty.TypeName) => Result<string>.Success(typeName.FixTypeName()),
            $"{nameof(ClassProperty.TypeName)}.GenericArguments" => Result<string>.Success(typeName.FixTypeName().GetGenericArguments()),
            $"{nameof(ClassProperty.TypeName)}.GenericArgumentsWithBrackets" => Result<string>.Success(typeName.FixTypeName().GetGenericArguments(addBrackets: true)),
            $"{nameof(ClassProperty.TypeName)}.GenericArguments.ClassName" => Result<string>.Success(typeName.FixTypeName().GetGenericArguments().GetClassName()),
            $"{nameof(ClassProperty.TypeName)}.ClassName" => Result<string>.Success(typeName.FixTypeName().GetClassName()),
            $"{nameof(ClassProperty.TypeName)}.Namespace" => Result<string>.Success(typeName.FixTypeName().GetNamespaceWithDefault()),
            $"{nameof(ClassProperty.TypeName)}.NoGenerics" => Result<string>.Success(typeName.FixTypeName().WithoutProcessedGenerics()),

            // default property stuff, for which typenames are already performed above.
            _ => ClassPropertyProcessor.Process(value, formatProvider, parentChildContext.ChildContext)
        };
    }
}
