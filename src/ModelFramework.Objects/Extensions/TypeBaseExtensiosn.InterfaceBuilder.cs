namespace ModelFramework.Objects.Extensions;

public static partial class TypeBaseExtensiosn
{
    public static IInterface ToInterfaceClass(this ITypeBase instance, InterfaceSettings settings)
        => instance.ToInterfaceBuilder(settings).Build();

    public static InterfaceBuilder ToInterfaceBuilder(this ITypeBase instance, InterfaceSettings settings)
        => new InterfaceBuilder()
            .WithName("I" + instance.Name)
            .WithNamespace(instance.Namespace)
            .WithVisibility(instance.Visibility)
            .WithPartial(instance.Partial)
            .AddInterfaces(instance.Interfaces.Where(i => i != "I" + instance.Name && i != instance.Namespace + ".I" + instance.Name))
            .AddProperties
            (
                settings.PropertyFilter == null
                    ? instance.Properties.Select(p => ChangeProperty(p, settings.ApplyGenericTypes, settings.ChangePropertiesToReadOnly))
                                         .Select(x => new ClassPropertyBuilder(x))
                    : instance.Properties.Where(settings.PropertyFilter)
                                         .Select(p => ChangeProperty(p, settings.ApplyGenericTypes, settings.ChangePropertiesToReadOnly))
                                         .Select(x => new ClassPropertyBuilder(x))
            )
            .AddMethods
            (
                settings.MethodFilter == null
                    ? instance.Methods
                        .Where(m => !m.Static
                            && !m.Override
                            && !m.Operator
                            && !m.IsInterfaceMethod()
                            && string.IsNullOrEmpty(m.ExplicitInterfaceName)
                            && m.Visibility == Visibility.Public)
                        .Select(m => ChangeArgumentsAndReturnType(m, settings.ApplyGenericTypes))
                        .Select(x => new ClassMethodBuilder(x))
                    : instance.Methods.Where(settings.MethodFilter)
                                      .Select(m => ChangeArgumentsAndReturnType(m, settings.ApplyGenericTypes))
                                      .Select(x => new ClassMethodBuilder(x))
            )
            .AddMetadata
            (
                settings.MetadataFilter == null
                    ? instance.Metadata.Select(x => new MetadataBuilder(x))
                    : instance.Metadata.Where(settings.MetadataFilter)
                                       .Select(x => new MetadataBuilder(x))
            )
            .AddAttributes
            (
                settings.AttributeFilter == null
                    ? instance.Attributes.Select(x => new AttributeBuilder(x))
                    : instance.Attributes.Where(settings.AttributeFilter)
                                         .Select(x => new AttributeBuilder(x))
            )
            .AddGenericTypeArguments(GenerateGenericTypeArguments(settings.ApplyGenericTypes));

    private static IClassProperty ChangeProperty(IClassProperty property,
                                                 IDictionary<string, string>? applyGenericTypes,
                                                 bool changePropertiesToReadOnly)
        => !changePropertiesToReadOnly
            ? property
            : new ClassProperty
            (
                property.HasGetter,
                false,
                false,
                property.GetterVisibility,
                property.SetterVisibility,
                property.InitializerVisibility,
                property.GetterCodeStatements,
                property.SetterCodeStatements,
                Enumerable.Empty<ICodeStatement>(),
                property.Metadata,
                property.Static,
                property.Virtual,
                property.Abstract,
                property.Protected,
                property.Override,
                property.Visibility,
                property.Name,
                property.Attributes,
                ReplaceGenericType(property.TypeName, applyGenericTypes),
                property.IsNullable,
                property.IsValueType,
                property.ExplicitInterfaceName,
                property.ParentTypeFullName
            );

    private static IClassMethod ChangeArgumentsAndReturnType(IClassMethod method, IDictionary<string, string>? applyGenericTypes)
    {
        if (applyGenericTypes == null || applyGenericTypes.Count == 0)
        {
            return method;
        }

        return new ClassMethod
        (
            method.Partial,
            method.ExtensionMethod,
            method.Operator,
            method.GenericTypeArguments,
            method.GenericTypeArgumentConstraints,
            method.Metadata,
            method.Static,
            method.Virtual,
            method.Abstract,
            method.Protected,
            method.Override,
            method.Visibility,
            method.Name,
            method.Attributes,
            method.CodeStatements,
            method.Parameters.Select(p => ChangeParameter(p, applyGenericTypes)),
            ReplaceGenericType(method.TypeName, applyGenericTypes),
            method.IsNullable,
            method.IsValueType,
            method.ExplicitInterfaceName,
            method.ParentTypeFullName
        );
    }

    private static IParameter ChangeParameter(IParameter parameter, IDictionary<string, string>? applyGenericTypes)
    {
        if (applyGenericTypes == null || applyGenericTypes.Count == 0)
        {
            return parameter;
        }

        return new Parameter
        (
            parameter.IsParamArray,
            parameter.IsOut,
            parameter.IsRef,
            ReplaceGenericType(parameter.TypeName, applyGenericTypes),
            parameter.IsNullable,
            parameter.IsValueType,
            parameter.Attributes,
            parameter.Metadata,
            parameter.Name,
            parameter.DefaultValue
        );
    }

    private static string ReplaceGenericType(string typeName, IDictionary<string, string>? applyGenericTypes)
    {
        if (applyGenericTypes == null || applyGenericTypes.Count == 0)
        {
            return typeName;
        }

        if (!applyGenericTypes.ContainsKey(typeName))
        {
            return typeName;
        }

        return applyGenericTypes[typeName];
    }

    private static List<string> GenerateGenericTypeArguments(IDictionary<string, string>? applyGenericTypes)
        => applyGenericTypes != null && applyGenericTypes.Count > 0
            ? applyGenericTypes.Values.ToList()
            : new List<string>();
}
