﻿namespace ClassFramework.Pipelines.Shared.PlaceholderProcessors;

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

        if (context is not PropertyContext propertyContext)
        {
            return Result.Continue<string>();
        }

        var typeName = propertyContext.TypeName.FixTypeName();

        return value switch
        {
            nameof(Property.Name) => Result.Success(propertyContext.SourceModel.Name),
            $"{nameof(Property.Name)}Lower" => Result.Success(propertyContext.SourceModel.Name.ToLower(formatProvider.ToCultureInfo())),
            $"{nameof(Property.Name)}Upper" => Result.Success(propertyContext.SourceModel.Name.ToUpper(formatProvider.ToCultureInfo())),
            $"{nameof(Property.Name)}Pascal" => Result.Success(propertyContext.SourceModel.Name.ToPascalCase(formatProvider.ToCultureInfo())),
            $"{nameof(Property.Name)}PascalCsharpFriendlyName" => Result.Success(propertyContext.SourceModel.Name.ToPascalCase(formatProvider.ToCultureInfo()).GetCsharpFriendlyName()),
            "BuilderMemberName" => Result.Success(propertyContext.SourceModel.GetBuilderMemberName(propertyContext.Settings.AddNullChecks, propertyContext.Settings.EnableNullableReferenceTypes, propertyContext.Settings.ValidateArguments, propertyContext.Settings.AddBackingFields, propertyContext.FormatProvider.ToCultureInfo())),
            "EntityMemberName" => Result.Success(propertyContext.SourceModel.GetEntityMemberName(propertyContext.Settings.AddBackingFields, propertyContext.FormatProvider.ToCultureInfo())),
            "InitializationExpression" => Result.Success(GetInitializationExpression(propertyContext.SourceModel, typeName, propertyContext.Settings.CollectionTypeName, formatProvider.ToCultureInfo(), propertyContext.Settings.AddNullChecks, propertyContext.Settings.ValidateArguments, propertyContext.Settings.EnableNullableReferenceTypes)),
            nameof(Property.TypeName) => Result.Success(typeName),
            $"{nameof(Property.TypeName)}.GenericArguments" => Result.Success(typeName.GetGenericArguments()),
            $"{nameof(Property.TypeName)}.GenericArgumentsWithBrackets" => Result.Success(typeName.GetGenericArguments(addBrackets: true)),
            $"{nameof(Property.TypeName)}.GenericArguments.ClassName" => Result.Success(typeName.GetGenericArguments().GetClassName()),
            $"{nameof(Property.TypeName)}.ClassName" => Result.Success(typeName.GetClassName()),
            $"{nameof(Property.TypeName)}.Namespace" => Result.Success(typeName.GetNamespaceWithDefault()),
            $"{nameof(Property.TypeName)}.NoGenerics" => Result.Success(typeName.WithoutProcessedGenerics()),
            "DefaultValue" => formattableStringParser.Parse(propertyContext.SourceModel.GetDefaultValue(_csharpExpressionCreator, propertyContext.Settings.EnableNullableReferenceTypes, typeName), formatProvider, context),
            _ => Result.Continue<string>()
        };
    }

    private static string GetInitializationExpression(Property property, string typeName, string collectionTypeName, CultureInfo cultureInfo, bool addNullChecks, ArgumentValidationType validateArguments, bool enableNullableReferenceTypes)
    {
        collectionTypeName = collectionTypeName.IsNotNull(nameof(collectionTypeName));

        return typeName.IsCollectionTypeName()
            && (collectionTypeName.Length == 0 || collectionTypeName != property.TypeName.WithoutGenerics())
                ? GetCollectionFormatStringForInitialization(property, typeName, cultureInfo, collectionTypeName, addNullChecks, validateArguments, enableNullableReferenceTypes)
                : property.Name.ToPascalCase(cultureInfo).GetCsharpFriendlyName();
    }

    private static string GetCollectionFormatStringForInitialization(Property property, string typeName, CultureInfo cultureInfo, string collectionTypeName, bool addNullChecks, ArgumentValidationType validateArguments, bool enableNullableReferenceTypes)
    {
        collectionTypeName = collectionTypeName.WhenNullOrEmpty(() => typeof(List<>).WithoutGenerics());

        var genericTypeName = typeName.GetGenericArguments();
        var nullSuffix = enableNullableReferenceTypes && !property.IsNullable
            ? "!"
            : string.Empty;

        return property.IsNullable || (addNullChecks && validateArguments != ArgumentValidationType.None)
            ? $"{property.Name.ToPascalCase(cultureInfo)} is null ? null{nullSuffix} : new {collectionTypeName}<{genericTypeName}>({property.Name.ToPascalCase(cultureInfo).GetCsharpFriendlyName()})"
            : $"new {collectionTypeName}<{genericTypeName}>({property.Name.ToPascalCase(cultureInfo).GetCsharpFriendlyName()})";
    }
}
