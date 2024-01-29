namespace ClassFramework.Pipelines.Extensions;

public static class PropertyExtensions
{
    public static string GetDefaultValue(this Property property, ICsharpExpressionCreator csharpExpressionCreator, bool enableNullableReferenceTypes, string typeName)
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

    public static string GetNullCheckSuffix(this Property property, string name, bool addNullChecks)
    {
        if (!addNullChecks || property.IsNullable || property.IsValueType)
        {
            return string.Empty;
        }

        return $" ?? throw new {typeof(ArgumentNullException).FullName}(nameof({name}))";
    }

    public static string GetBuilderMemberName(this Property property, bool addNullChecks, bool enableNullableReferenceTypes, ArgumentValidationType argumentValidation, bool addBackingFields, CultureInfo cultureInfo)
    {
        cultureInfo = cultureInfo.IsNotNull(nameof(cultureInfo));

        if (property.HasBackingFieldOnBuilder(addNullChecks, enableNullableReferenceTypes, argumentValidation, addBackingFields))
        {
            return $"_{property.Name.ToPascalCase(cultureInfo)}";
        }

        return property.Name;
    }

    public static string GetEntityMemberName(this Property property, bool addBackingFields, CultureInfo cultureInfo)
    {
        cultureInfo = cultureInfo.IsNotNull(nameof(cultureInfo));

        if (addBackingFields)
        {
            return $"_{property.Name.ToPascalCase(cultureInfo)}";
        }

        return property.Name;
    }

    // For now, only add backing fields for non nullable fields.
    // Nullable fields can simply have auto properties, as null checks are not needed
    public static bool HasBackingFieldOnBuilder(this Property property, bool addNullChecks, bool enableNullableReferenceTypes, ArgumentValidationType argumentValidation, bool addBackingFields)
        => (addNullChecks
        && !property.IsValueType
        && !property.IsNullable(enableNullableReferenceTypes)
        && argumentValidation != ArgumentValidationType.Shared) || addBackingFields;

    public static Result<string> GetBuilderConstructorInitializer<TModel>(
        this Property property,
        PipelineContext<TModel, BuilderContext> context,
        IFormattableStringParser formattableStringParser)
    {
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));
        context = context.IsNotNull(nameof(context));

        var builderArgumentTypeResult = GetBuilderArgumentType(property, context, formattableStringParser);

        if (!builderArgumentTypeResult.IsSuccessful())
        {
            return builderArgumentTypeResult;
        }

        return Result.Success(builderArgumentTypeResult.Value!
            .FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName)
            .GetCollectionInitializeStatement()
            .GetCsharpFriendlyTypeName());
    }

    public static Result<string> GetBuilderArgumentType<TModel>(this Property property, PipelineContext<TModel, BuilderContext> context, IFormattableStringParser formattableStringParser)
    {
        context = context.IsNotNull(nameof(context));
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));

        var metadata = property.Metadata.WithMappingMetadata(property.TypeName.GetCollectionItemType().WhenNullOrEmpty(property.TypeName), context.Context.Settings.TypeSettings);
        var ns = metadata.GetStringValue(MetadataNames.CustomBuilderNamespace);

        if (!string.IsNullOrEmpty(ns))
        {
            var newTypeName = metadata.GetStringValue(MetadataNames.CustomBuilderName, "{TypeName}");
            var newFullName = $"{ns}.{newTypeName}";
            if (property.TypeName.IsCollectionTypeName())
            {
                var idx = property.TypeName.IndexOf('<');
                if (idx > -1)
                {
                    newFullName = $"{property.TypeName.Substring(0, idx)}<{newFullName.Replace("{TypeName.ClassName}", "{TypeName.GenericArguments.ClassName}")}>";
                }
            }

            return formattableStringParser.Parse
            (
                newFullName,
                context.Context.FormatProvider,
                new ParentChildContext<PipelineContext<TModel, BuilderContext>, Property>(context, property, context.Context.Settings)
            );
        }

        return formattableStringParser.Parse
        (
            metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, context.Context.MapTypeName(property.TypeName)),
            context.Context.FormatProvider,
            new ParentChildContext<PipelineContext<TModel, BuilderContext>, Property>(context, property, context.Context.Settings)
        );
    }

    public static string GetSuffix(this Property source, bool enableNullableReferenceTypes)
        => !source.IsNullable(enableNullableReferenceTypes) && !source.IsValueType && !source.TypeName.IsCollectionTypeName()
            ? "?"
            : string.Empty;
}
