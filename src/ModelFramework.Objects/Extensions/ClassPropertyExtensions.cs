namespace ModelFramework.Objects.Extensions;

public static class ClassPropertyExtensions
{
    public static string CreateImmutableBuilderInitializationCode(this IClassProperty property, ImmutableBuilderClassSettings settings)
        => string.Format
        (
            property.Metadata.GetStringValue
            (
                MetadataNames.CustomBuilderConstructorInitializeExpression,
                () => property.TypeName.IsCollectionTypeName()
                    ? CreateCollectionInitialization(property, settings)
                    : CreateSingleInitialization(property, settings)
            ),
            property.Name,
            property.Name.ToPascalCase(),
            property.TypeName.FixTypeName().GetCsharpFriendlyTypeName(),
            property.TypeName.GetGenericArguments().GetCsharpFriendlyTypeName(),
            settings.ConstructorSettings.AddNullChecks
                ? $"if (source.{property.Name} != null) "
                : ""
        );

    private static string CreateSingleInitialization(IClassProperty property, ImmutableBuilderClassSettings settings)
        => settings.UseLazyInitialization
            ? $"_{property.Name.ToPascalCase()}Delegate = new {property.GetNewExpression(settings)}(() => source.{property.Name})"
            : $"{property.Name} = source.{property.Name}";

    private static string CreateCollectionInitialization(IClassProperty property, ImmutableBuilderClassSettings settings)
        => settings.ConstructorSettings.AddNullChecks
            ? $"if (source.{property.Name} != null) {property.Name}.AddRange(source.{property.Name})"
            : $"{property.Name}.AddRange(source.{property.Name})";

    internal static string GetDefaultValue(this IClassProperty property)
    {
        var md = property.Metadata.FirstOrDefault(x => x.Name == MetadataNames.CustomImmutableBuilderDefaultValue);
        if (md != null && md.Value != null)
        {
            if (md.Value is Literal literal && literal.Value != null)
            {
                return literal.Value;
            }
            return md.Value.CsharpFormat();
        }

        if (property.TypeName.IsStringTypeName() && !property.IsNullable)
        {
            return "string.Empty";
        }

        if (property.TypeName.IsObjectTypeName() && !property.IsNullable)
        {
            return "new object()";
        }

        return "default";
    }

    public static IEnumerable<IMetadata> GetImmutableCollectionMetadata(this IClassProperty property, string newCollectionTypeName)
        => property.TypeName.IsCollectionTypeName()
            ? new[]
              {
                    new Metadata(MetadataNames.CustomImmutableArgumentType, property.TypeName.FixCollectionTypeName(newCollectionTypeName).GetCsharpFriendlyTypeName()),
              }
            : Array.Empty<IMetadata>();

    public static IEnumerable<IMetadata> GetBuilderCollectionMetadata(this IClassProperty property, string newCollectionTypeName)
        => property.TypeName.IsCollectionTypeName()
            ? new[]
              {
                    new Metadata(MetadataNames.CustomBuilderArgumentType, property.TypeName.FixCollectionTypeName(newCollectionTypeName))
              }
            : Array.Empty<IMetadata>();

    public static IEnumerable<IMetadata> GetObservableCollectionMetadata(this IClassProperty property, string newCollectionTypeName)
        => property.TypeName.IsCollectionTypeName()
            ? new[]
              {
                    new Metadata(MetadataNames.CustomObservableArgumentType, property.TypeName.FixCollectionTypeName(newCollectionTypeName))
              }
            : Array.Empty<IMetadata>();

    public static IEnumerable<ICodeStatement> FixObservablePropertyGetterBody(this IClassProperty property)
        => !property.GetterCodeStatements.Any() && !property.SetterCodeStatements.Any()
            ? new[] { string.Format("return _{0};", property.Name.ToPascalCase()) }.ToLiteralCodeStatements()
            : property.GetterCodeStatements;

    public static IEnumerable<ICodeStatement> FixObservablePropertySetterBody(this IClassProperty property)
        => !property.GetterCodeStatements.Any() && !property.SetterCodeStatements.Any()
            ? new[] { string.Format("_{0} = value;", property.Name.ToPascalCase()) + Environment.NewLine + string.Format(@"if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(""{0}""));", property.Name) }.ToLiteralCodeStatements()
            : property.SetterCodeStatements;

    public static string GetGetterModifiers(this IClassProperty property)
        => property.GetSubModifiers(property.GetterVisibility, MetadataNames.PropertyGetterModifiers);

    public static string GetSetterModifiers(this IClassProperty property)
        => property.GetSubModifiers(property.SetterVisibility, MetadataNames.PropertySetterModifiers);

    public static string GetInitializerModifiers(this IClassProperty property)
        => property.GetSubModifiers(property.InitializerVisibility, MetadataNames.PropertyInitializerModifiers);

    private static string GetSubModifiers(this IClassProperty property, Visibility? subVisibility, string customModifiersMetadatName)
    {
        var customModifiers = property.Metadata.GetStringValue(customModifiersMetadatName);
        if (!string.IsNullOrEmpty(customModifiers)
            && customModifiers != property.Visibility.ToString().ToLower(CultureInfo.InvariantCulture))
        {
            return customModifiers + " ";
        }
        var builder = new StringBuilder();

        if (subVisibility != null && subVisibility != property.Visibility)
        {
            builder.Append(subVisibility.ToString().ToLower(CultureInfo.InvariantCulture));
        }

        if (builder.Length > 0)
        {
            builder.Append(" ");
        }

        return builder.ToString();
    }
}
