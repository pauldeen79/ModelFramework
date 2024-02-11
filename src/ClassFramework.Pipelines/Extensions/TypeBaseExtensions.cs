namespace ClassFramework.Pipelines.Extensions;

public static class TypeBaseExtensions
{
    public static bool IsMemberValidForBuilderClass(
        this IType parent,
        IParentTypeContainer parentTypeContainer,
        IPipelineGenerationSettings settings)
    {
        parentTypeContainer = parentTypeContainer.IsNotNull(nameof(parentTypeContainer));
        settings = settings.IsNotNull(nameof(settings));

        if (!settings.EnableInheritance)
        {
            // If entity inheritance is not enabled, then simply include all members
            return true;
        }

        // If inheritance is enabled, then include the members if it's defined on the parent class (or use the custom instance comparison delegate, when provided)
        return parentTypeContainer.IsDefinedOn(parent, settings.InheritanceComparisonDelegate);
    }

    public static string GetGenericTypeArgumentConstraintsString(this IType instance)
        => instance.GenericTypeArgumentConstraints.Count != 0
            ? string.Concat
            (
                Environment.NewLine,
                "        ",
                string.Join(string.Concat(Environment.NewLine, "        "), instance.GenericTypeArgumentConstraints)
            )
            : string.Empty;

    public static Result<string> GetCustomValueForInheritedClass(
        this IType instance,
        bool enableInheritance,
        Func<IBaseClassContainer, Result<string>> customValue)
    {
        customValue = customValue.IsNotNull(nameof(customValue));

        if (!enableInheritance)
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

    public static IEnumerable<Property> GetBuilderConstructorProperties(
        this IType instance,
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
            return Enumerable.Empty<Property>();
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

    public static IEnumerable<Result<FieldBuilder>> GetBuilderClassFields(
        this IType instance,
        PipelineContext<IConcreteTypeBuilder, BuilderContext> context,
        IFormattableStringParser formattableStringParser)
    {
        context = context.IsNotNull(nameof(context));
        formattableStringParser = formattableStringParser.IsNotNull(nameof(formattableStringParser));

        if (!context.Context.HasBackingFields())
        {
            yield break;
        }

        foreach (var property in instance.Properties.Where(x =>
            instance.IsMemberValidForBuilderClass(x, context.Context.Settings)
            && x.HasBackingFieldOnBuilder(context.Context.Settings.EntitySettings.NullCheckSettings.AddNullChecks, context.Context.Settings.TypeSettings.EnableNullableReferenceTypes, context.Context.Settings.EntitySettings.ConstructorSettings.OriginalValidateArguments, context.Context.Settings.EntitySettings.GenerationSettings.AddBackingFields)))
        {
            var builderArgumentTypeResult = property.GetBuilderArgumentType(context, formattableStringParser);

            if (!builderArgumentTypeResult.IsSuccessful())
            {
                yield return Result.FromExistingResult<FieldBuilder>(builderArgumentTypeResult);
                yield break;
            }

            yield return Result.Success(new FieldBuilder()
                .WithName($"_{property.Name.ToPascalCase(context.Context.FormatProvider.ToCultureInfo())}")
                .WithTypeName(builderArgumentTypeResult.Value!.FixCollectionTypeName(context.Context.Settings.TypeSettings.NewCollectionTypeName).FixNullableTypeName(property))
                .WithIsNullable(property.IsNullable)
                .WithIsValueType(property.IsValueType));
        }
    }

    public static IEnumerable<Property> GetPropertiesFromClassAndBaseClass(
        this IType instance,
        Builder.PipelineSettings settings)
    {
        settings = settings.IsNotNull(nameof(settings));

        var properties = instance.Properties.AsEnumerable();
        if (settings.InheritanceSettings.BaseClass is not null)
        {
            properties = properties.Concat(settings.InheritanceSettings.BaseClass.Properties);
        }

        return properties.Where(x => instance.IsMemberValidForBuilderClass(x, settings));
    }

    public static string GetEntityBaseClass(this IType instance, bool enableInheritance, Class? baseClass)
        => enableInheritance
        && baseClass is not null
            ? baseClass.GetFullName()
            : instance.GetCustomValueForInheritedClass(enableInheritance, cls => Result.Success(cls.BaseClass!)).Value!; // we're always returning Success here, so we can shortcut the validation of the result by getting .Value

    public static string WithoutInterfacePrefix(this IType instance)
        => instance is Domain.Types.Interface
            && instance.Name.StartsWith("I")
            && instance.Name.Length >= 2
            && instance.Name.Substring(1, 1).Equals(instance.Name.Substring(1, 1).ToUpperInvariant(), StringComparison.Ordinal)
                ? instance.Name.Substring(1)
                : instance.Name;
}
