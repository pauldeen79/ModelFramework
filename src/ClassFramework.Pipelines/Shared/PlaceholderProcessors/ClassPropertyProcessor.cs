namespace ClassFramework.Pipelines.Shared.PlaceholderProcessors;

public class PropertyProcessor : IPipelinePlaceholderProcessor
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public PropertyProcessor(ICsharpExpressionCreator csharpExpressionCreator)
    {
        _csharpExpressionCreator = csharpExpressionCreator.IsNotNull(nameof(csharpExpressionCreator));
    }

    public Result<string> Process(string value, IFormatProvider formatProvider, object? context, IFormattableStringParser formattableStringParser)
    {
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));

        if (context is not PropertyContext PropertyContext)
        {
            return Result.Continue<string>();
        }

        var typeName = PropertyContext.TypeName.FixTypeName();

        return value switch
        {
            nameof(Property.Name) => Result.Success(PropertyContext.SourceModel.Name),
            $"{nameof(Property.Name)}Lower" => Result.Success(PropertyContext.SourceModel.Name.ToLower(formatProvider.ToCultureInfo())),
            $"{nameof(Property.Name)}Upper" => Result.Success(PropertyContext.SourceModel.Name.ToUpper(formatProvider.ToCultureInfo())),
            $"{nameof(Property.Name)}Pascal" => Result.Success(PropertyContext.SourceModel.Name.ToPascalCase(formatProvider.ToCultureInfo())),
            $"{nameof(Property.Name)}PascalCsharpFriendlyName" => Result.Success(PropertyContext.SourceModel.Name.ToPascalCase(formatProvider.ToCultureInfo()).GetCsharpFriendlyName()),
            "BuilderMemberName" => Result.Success(PropertyContext.SourceModel.GetBuilderMemberName(PropertyContext.Settings.AddNullChecks, PropertyContext.Settings.EnableNullableReferenceTypes, PropertyContext.Settings.ValidateArguments, PropertyContext.FormatProvider.ToCultureInfo())),
            "EntityMemberName" => Result.Success(PropertyContext.SourceModel.GetEntityMemberName(PropertyContext.Settings.AddBackingFields, PropertyContext.FormatProvider.ToCultureInfo())),
            "InitializationExpression" => Result.Success(GetInitializationExpression(PropertyContext.SourceModel, typeName, PropertyContext.Settings.CollectionTypeName, formatProvider.ToCultureInfo())),
            nameof(Property.TypeName) => Result.Success(typeName),
            $"{nameof(Property.TypeName)}.GenericArguments" => Result.Success(typeName.GetGenericArguments()),
            $"{nameof(Property.TypeName)}.GenericArgumentsWithBrackets" => Result.Success(typeName.GetGenericArguments(addBrackets: true)),
            $"{nameof(Property.TypeName)}.GenericArguments.ClassName" => Result.Success(typeName.GetGenericArguments().GetClassName()),
            $"{nameof(Property.TypeName)}.ClassName" => Result.Success(typeName.GetClassName()),
            $"{nameof(Property.TypeName)}.Namespace" => Result.Success(typeName.GetNamespaceWithDefault()),
            $"{nameof(Property.TypeName)}.NoGenerics" => Result.Success(typeName.WithoutProcessedGenerics()),
            "DefaultValue" => formattableStringParser.Parse(PropertyContext.SourceModel.GetDefaultValue(_csharpExpressionCreator, PropertyContext.Settings.EnableNullableReferenceTypes, typeName), formatProvider, context),
            _ => Result.Continue<string>()
        };
    }

    private static string GetInitializationExpression(Property property, string typeName, string collectionTypeName, CultureInfo cultureInfo)
    {
        collectionTypeName = collectionTypeName.IsNotNull(nameof(collectionTypeName));

        return typeName.IsCollectionTypeName()
            && (collectionTypeName.Length == 0 || collectionTypeName != property.TypeName.WithoutGenerics())
                ? GetCollectionFormatStringForInitialization(property, typeName, cultureInfo, collectionTypeName)
                : property.Name.ToPascalCase(cultureInfo).GetCsharpFriendlyName();
    }

    private static string GetCollectionFormatStringForInitialization(Property property, string typeName, CultureInfo cultureInfo, string collectionTypeName)
    {
        collectionTypeName = collectionTypeName.WhenNullOrEmpty(() => typeof(List<>).WithoutGenerics());

        var genericTypeName = typeName.GetGenericArguments();

        return property.IsNullable
            ? $"{property.Name.ToPascalCase(cultureInfo)} is null ? null : new {collectionTypeName}<{genericTypeName}>({property.Name.ToPascalCase(cultureInfo).GetCsharpFriendlyName()})"
            : $"new {collectionTypeName}<{genericTypeName}>({property.Name.ToPascalCase(cultureInfo).GetCsharpFriendlyName()})";
    }
}
