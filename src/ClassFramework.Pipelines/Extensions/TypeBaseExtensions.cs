namespace ClassFramework.Pipelines.Extensions;

public static class TypeBaseExtensions
{
    public static bool IsMemberValidForBuilderClass(
        this TypeBase parent,
        IParentTypeContainer parentTypeContainer,
        Builder.PipelineBuilderSettings settings)
    {
        parentTypeContainer = parentTypeContainer.IsNotNull(nameof(parentTypeContainer));
        settings = settings.IsNotNull(nameof(settings));

        if (!settings.ClassSettings.InheritanceSettings.EnableInheritance)
        {
            // If entity inheritance is not enabled, then simply include all members
            return true;
        }

        // If inheritance is enabled, then include the members if it's defined on the parent class (or use the custom instance comparison delegate, when provided)
        return parentTypeContainer.IsDefinedOn(parent, settings.InheritanceSettings.InheritanceComparisonDelegate);
    }

    public static string GetGenericTypeArgumentsString(this TypeBase instance)
        => instance.GenericTypeArguments.Count > 0
            ? $"<{string.Join(", ", instance.GenericTypeArguments)}>"
            : string.Empty;

    public static string GetGenericTypeArgumentConstraintsString(this TypeBase instance)
        => instance.GenericTypeArgumentConstraints.Any()
            ? string.Concat
            (
                Environment.NewLine,
                "        ",
                string.Join(string.Concat(Environment.NewLine, "        "), instance.GenericTypeArgumentConstraints)
            )
            : string.Empty;

    public static Result<string> GetCustomValueForInheritedClass(
        this TypeBase instance,
        Entity.PipelineBuilderSettings settings,
        Func<IBaseClassContainer, Result<string>> customValue)
    {
        settings = settings.IsNotNull(nameof(settings));
        customValue = customValue.IsNotNull(nameof(customValue));

        if (!settings.InheritanceSettings.EnableInheritance)
        {
            // Inheritance is not enabled
            return Result.Success(string.Empty);
        }

        var baseClassContainer = instance as IBaseClassContainer;
        if (baseClassContainer is null)
        {
            // Type cannot have a base class
            return Result.Success(string.Empty);
        }

        if (string.IsNullOrEmpty(baseClassContainer.BaseClass))
        {
            // Class is not inherited
            return Result.Success(string.Empty);
        }

        return customValue(baseClassContainer);
    }

    public static IEnumerable<ClassProperty> GetBuilderConstructorProperties(
        this TypeBase instance,
        BuilderContext context)
    {
        context = context.IsNotNull(nameof(context));

        var constructorsContainer = instance as IConstructorsContainer;
        if (constructorsContainer is null)
        {
            throw new ArgumentException("Cannot get immutable builder constructor properties for type that does not have constructors", nameof(context));
        }

        if (constructorsContainer.HasPublicParameterlessConstructor())
        {
            return instance.Properties.Where(x => x.HasSetter || x.HasInitializer);
        }

        var ctor = constructorsContainer.Constructors.FirstOrDefault(x => x.Visibility == Visibility.Public && x.Parameters.Count > 0);
        if (ctor is null)
        {
            // No public constructor, so we can't add properties to initialization.
            return Enumerable.Empty<ClassProperty>();
        }

        if (context.IsBuilderForOverrideEntity && context.Settings.InheritanceSettings.BaseClass is not null)
        {
            // Try to get property from either the base class c'tor or the class c'tor itself
            return ctor
                .Parameters
                .Select(x => instance.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase))
                    ?? context.Settings.InheritanceSettings.BaseClass!.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
                .Where(x => x is not null);
        }

        return ctor
            .Parameters
            .Select(x => instance.Properties.FirstOrDefault(y => y.Name.Equals(x.Name, StringComparison.OrdinalIgnoreCase)))
            .Where(x => x is not null);
    }

    public static IEnumerable<Result<ClassFieldBuilder>> GetBuilderClassFields(
        this TypeBase instance,
        PipelineContext<ClassBuilder, BuilderContext> context,
        IFormattableStringParser formattableStringParser)
    {
        context = context.IsNotNull(nameof(context));
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));

        if (context.Context.IsAbstractBuilder
            || !context.Context.Settings.GenerationSettings.AddNullChecks
            || context.Context.Settings.ClassSettings.ConstructorSettings.OriginalValidateArguments == ArgumentValidationType.Shared)
        {
            yield break;
        }
        
        foreach (var property in instance.Properties.Where(x => instance.IsMemberValidForBuilderClass(x, context.Context.Settings)))
        {
            var builderArgumentTypeResult = formattableStringParser.Parse
            (
                property.Metadata.GetStringValue(MetadataNames.CustomBuilderArgumentType, property.TypeName),
                context.Context.FormatProvider,
                context
            );

            if (!builderArgumentTypeResult.IsSuccessful())
            {
                yield return Result.FromExistingResult<ClassFieldBuilder>(builderArgumentTypeResult);
                yield break;
            }

            yield return Result.Success(new ClassFieldBuilder()
                .WithName($"_{property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo())}")
                .WithTypeName(builderArgumentTypeResult.Value!.FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName))
                .WithIsNullable(property.IsNullable)
                .WithIsValueType(property.IsValueType));
        }
    }

    public static IEnumerable<ClassProperty> GetPropertiesFromClassAndBaseClass(this TypeBase instance, Builder.PipelineBuilderSettings settings)
    {
        settings = settings.IsNotNull(nameof(settings));

        var properties = instance.Properties.AsEnumerable();
        if (settings.InheritanceSettings.BaseClass is not null)
        {
            properties = properties.Concat(settings.InheritanceSettings.BaseClass.Properties);
        }

        return properties.Where(x => instance.IsMemberValidForBuilderClass(x, settings));
    }
}
