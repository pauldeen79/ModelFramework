﻿namespace ModelFramework.Objects.Extensions;

public static class ClassPropertyExtensions
{
    internal static string CreateImmutableBuilderInitializationCode(this IClassProperty property,
                                                                    ImmutableBuilderClassSettings settings)
        => string.Format
        (
            property.Metadata.GetStringValue
            (
                MetadataNames.CustomBuilderConstructorInitializeExpression,
                () => property.TypeName.IsCollectionTypeName()
                    ? CreateCollectionInitialization(property, settings)
                    : CreateSingleInitialization(property, settings)
            ),
            property.Name,                                                                              // 0
            property.Name.ToPascalCase(),                                                               // 1
            property.TypeName.GetCsharpFriendlyTypeName(),                                              // 2
            property.TypeName.GetGenericArguments().GetCsharpFriendlyTypeName(),                        // 3
            settings.ConstructorSettings.AddNullChecks && !property.IsValueType && property.IsNullable  // 4
                ? $"if (source.{property.Name} != null) "
                : "",
            settings.TypeSettings.FormatInstanceTypeNameDelegate != null                                // 5
                ? settings.TypeSettings.FormatInstanceTypeNameDelegate.Invoke(new ClassBuilder().WithName(property.TypeName.GetClassName()).WithNamespace(property.TypeName.GetNamespaceWithDefault()).Build(), true).GetClassName().WhenNullOrEmpty(() => property.TypeName.GetClassName())
                : property.TypeName.GetClassName(),
            property.TypeName.GetGenericArguments().GetClassName(),                                     // 6
            settings.NameSettings.BuildersNamespace,                                                    // 7
            property.TypeName.WithoutProcessedGenerics().GetCsharpFriendlyTypeName(),                   // 8
            string.IsNullOrEmpty(property.TypeName.GetGenericArguments())                               // 9
                ? string.Empty
                : $"<{property.TypeName.GetGenericArguments().GetCsharpFriendlyTypeName()}>",
            settings.TypeSettings.FormatInstanceTypeNameDelegate != null                                // 10
                ? settings.TypeSettings.FormatInstanceTypeNameDelegate.Invoke(new ClassBuilder().WithName(property.TypeName.WithoutProcessedGenerics().GetClassName()).WithNamespace(property.TypeName.GetNamespaceWithDefault()).Build(), true).GetClassName().WhenNullOrEmpty(() => property.TypeName.WithoutProcessedGenerics().GetClassName())
                : property.TypeName.WithoutProcessedGenerics().GetClassName()
        );

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

    internal static string GetDefaultValue(this IClassProperty property, bool enableNullableReferenceTypes)
    {
        var md = property.Metadata.FirstOrDefault(x => x.Name == MetadataNames.CustomBuilderDefaultValue);
        if (md != null && md.Value != null)
        {
            if (md.Value is Literal literal && literal.Value != null)
            {
                return literal.Value;
            }
            return md.Value.CsharpFormat();
        }

        return property.TypeName.GetDefaultValue(property.IsNullable, property.IsValueType, enableNullableReferenceTypes);
    }

    internal static IClassProperty EnsureParentTypeFullName(this IClassProperty property, IClass parentClass)
        => new ClassPropertyBuilder(property)
            .WithParentTypeFullName(property.ParentTypeFullName.WhenNullOrEmpty(() => parentClass.GetFullName().WithoutGenerics()))
            .Build();

    private static string CreateSingleInitialization(IClassProperty property, ImmutableBuilderClassSettings settings)
    {
        var name = settings.ConstructorSettings.AddNullChecks && settings.ClassSettings.ConstructorSettings.OriginalValidateArguments != ArgumentValidationType.Shared
            ? $"_{property.Name.ToPascalCase()}"
            : property.Name;
        
        return settings.GenerationSettings.UseLazyInitialization
            ? $"_{property.Name.ToPascalCase()}Delegate = new {property.GetNewExpression(settings)}(() => source.{property.Name})"
            : $"{name} = source.{property.Name}";
    }

    private static string CreateCollectionInitialization(IClassProperty property, ImmutableBuilderClassSettings settings)
    {
        if (settings.TypeSettings.NewCollectionTypeName == typeof(IEnumerable<>).WithoutGenerics())
        {
            return settings.ConstructorSettings.AddNullChecks && !property.IsValueType && property.IsNullable
                ? $"if (source.{property.Name} != null) {property.Name} = source.{property.Name}"
                : $"{property.Name} = source.{property.Name}";
        }

        return settings.ConstructorSettings.AddNullChecks && !property.IsValueType && property.IsNullable
            ? $"if (source.{property.Name} != null) {property.Name}.AddRange(source.{property.Name})"
            : $"{property.Name}.AddRange(source.{property.Name})";
    }

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
