﻿namespace ClassFramework.Pipelines.Extensions;

public static class ClassPropertyExtensions
{
    public static string GetDefaultValue(this ClassProperty property, ICsharpExpressionCreator csharpExpressionCreator, bool enableNullableReferenceTypes, string typeName)
    {
        csharpExpressionCreator = csharpExpressionCreator.IsNotNull(nameof(csharpExpressionCreator));

        var md = property.Metadata.FirstOrDefault(x => x.Name == MetadataNames.CustomBuilderDefaultValue);
        if (md is not null && md.Value is not null)
        {
            if (md.Value is Literal literal && literal.Value is not null)
            {
                return literal.Value;
            }

            return csharpExpressionCreator.Create(md.Value);
        }

        return typeName.GetDefaultValue(property.IsNullable, property.IsValueType, enableNullableReferenceTypes);
    }

    public static string GetNullCheckSuffix(this ClassProperty property, string name, bool addNullChecks)
    {
        if (!addNullChecks || property.IsNullable || property.IsValueType)
        {
            return string.Empty;
        }

        return $" ?? throw new {typeof(ArgumentNullException).FullName}(nameof({name}))";
    }

    public static string GetBuilderMemberName(this ClassProperty property, bool addNullChecks, bool enableNullableReferenceTypes, ArgumentValidationType argumentValidation, CultureInfo cultureInfo)
    {
        cultureInfo = cultureInfo.IsNotNull(nameof(cultureInfo));

        if (property.HasBackingFieldOnBuilder(addNullChecks, enableNullableReferenceTypes, argumentValidation))
        {
            return $"_{property.Name.ToPascalCase(cultureInfo)}";
        }

        return property.Name;
    }

    public static string GetEntityMemberName(this ClassProperty property, bool addBackingFields, CultureInfo cultureInfo)
    {
        cultureInfo = cultureInfo.IsNotNull(nameof(cultureInfo));

        if (addBackingFields && !property.TypeName.FixTypeName().IsCollectionTypeName())
        {
            return $"_{property.Name.ToPascalCase(cultureInfo)}";
        }

        return property.Name;
    }

    // For now, only add backing fields for non nullable fields.
    // Nullable fields can simply have auto properties, as null checks are not needed
    public static bool HasBackingFieldOnBuilder(this ClassProperty property, bool addNullChecks, bool enableNullableReferenceTypes, ArgumentValidationType argumentValidation)
        => addNullChecks
        && !property.IsValueType
        && !property.IsNullable(enableNullableReferenceTypes)
        && argumentValidation != ArgumentValidationType.Shared;

    public static Result<string> GetBuilderClassConstructorInitializer<TModel>(
        this ClassProperty property,
        PipelineContext<TModel, BuilderContext> context,
        IFormattableStringParser formattableStringParser,
        string typeName)
    {
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
        context = context.IsNotNull(nameof(context));
        typeName = typeName.IsNotNull(nameof(typeName));

        var builderArgumentTypeResult = formattableStringParser.Parse
        (
            property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, context.Context.MapTypeName(typeName)),
            context.Context.FormatProvider,
            new ParentChildContext<TModel, BuilderContext, ClassProperty>(context, property, context.Context.Settings)
        );

        if (!builderArgumentTypeResult.IsSuccessful())
        {
            return builderArgumentTypeResult;
        }

        return Result.Success(builderArgumentTypeResult.Value!
            .FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName)
            .GetCollectionInitializeStatement()
            .GetCsharpFriendlyTypeName());
    }

    public static ClassProperty EnsureParentTypeFullName(this ClassProperty property, Class parentClass)
        => new ClassPropertyBuilder(property)
            .WithParentTypeFullName(property.ParentTypeFullName.WhenNullOrEmpty(() => parentClass.IsNotNull(nameof(parentClass)).GetFullName().WithoutGenerics()))
            .Build();

    public static string GetSuffix(this ClassProperty source, bool enableNullableReferenceTypes)
        => !source.IsNullable(enableNullableReferenceTypes) && !source.IsValueType
            ? "?"
            : string.Empty;

    public static string GetInitializationExpression(this ClassProperty property, string collectionTypeName, CultureInfo cultureInfo)
    {
        collectionTypeName = collectionTypeName.IsNotNull(nameof(collectionTypeName));

        return property.TypeName.FixTypeName().IsCollectionTypeName()
            && (collectionTypeName.Length == 0 || collectionTypeName != property.TypeName.WithoutGenerics())
                ? GetCollectionFormatStringForInitialization(property, cultureInfo, collectionTypeName)
                : property.Name.ToPascalCase(cultureInfo).GetCsharpFriendlyName();
    }

    private static string GetCollectionFormatStringForInitialization(ClassProperty property, CultureInfo cultureInfo, string collectionTypeName)
    {
        collectionTypeName = collectionTypeName.WhenNullOrEmpty(() => typeof(List<>).WithoutGenerics());

        return property.IsNullable
            ? $"{property.Name.ToPascalCase(cultureInfo)} is null ? null : new {collectionTypeName}<{property.TypeName.GetGenericArguments()}>({property.Name.ToPascalCase(cultureInfo).GetCsharpFriendlyName()})"
            : $"new {collectionTypeName}<{property.TypeName.GetGenericArguments()}>({property.Name.ToPascalCase(cultureInfo).GetCsharpFriendlyName()})";
    }
}
