namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract class ClassFrameworkCSharpClassBase : CSharpClassBase
{
    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string DefaultFileName => string.Empty; // not used because we're using multiple files, but it's abstract so we need to fill it

    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type RecordConcreteCollectionType => typeof(ReadOnlyValueCollection<>);
    protected override string FileNameSuffix => Constants.TemplateGenerated;
    protected override string ProjectName => Constants.ProjectName;
    protected override bool UseLazyInitialization => false; // we don't want lazy stuff in models, just getters and setters
    protected override bool AddBackingFieldsForCollectionProperties => false; // we just want static stuff - else you need to choose builders or models instead of entities
    protected override bool AddPrivateSetters => false; // we just want static stuff - else you need to choose builders or models instead of entities
    protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.Shared;
    protected override bool ConvertStringToStringBuilderOnBuilders => false; // we don't want string builders, just strings

    protected override void FixImmutableBuilderProperty(ModelFramework.Objects.Builders.ClassPropertyBuilder property, string typeName)
    {
        Guard.IsNotNull(property);
        Guard.IsNotNull(typeName);

        base.FixImmutableBuilderProperty(property, typeName);

        if ((typeName.IsBooleanTypeName() || typeName.IsNullableBooleanTypeName())
            && property.Name.In(nameof(IClassProperty.HasGetter), nameof(IClassProperty.HasSetter)))
        {
            property.SetDefaultValueForBuilderClassConstructor(new ModelFramework.Common.Literal("true"));
        }

        AddBuilderOverloads(property, typeName, property.Name);

        if (property.Name == nameof(IVisibilityContainer.Visibility))
        {
            property.SetDefaultValueForBuilderClassConstructor
            (
                new ModelFramework.Common.Literal
                (
                    MapCodeGenerationNamespacesToDomain($"I{typeName}" == nameof(IClassField)
                        ? $"{typeof(Visibility).FullName}.{Visibility.Private}"
                        : $"{typeof(Visibility).FullName}.{Visibility.Public}")
                )
            );
        }

        if (property.Name == nameof(IClassProperty.HasSetter))
        {
            property.SetBuilderWithExpression(@"{0} = {2};
if ({2})
{5}
    HasInitializer = false;
{6}");
        }

        if (property.Name == nameof(IClassProperty.HasInitializer))
        {
            property.SetBuilderWithExpression(@"{0} = {2};
if ({2})
{5}
    HasSetter = false;
{6}");
        }
    }

    private static void AddBuilderOverloads(ModelFramework.Objects.Builders.ClassPropertyBuilder property, string typeName, string propertyName)
    {
        if (propertyName == nameof(ITypeContainer.TypeName) && typeName.IsStringTypeName())
        {
            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("WithType") //if we omit this, then the method name would be WithTypeName
                .AddParameter("type", typeof(Type))
                .WithInitializeExpression("{2} = type.AssemblyQualifiedName.FixTypeName(); IsValueType = type.IsValueType || type.IsEnum;")
                .Build());
        }

        if (propertyName == nameof(IMetadataContainer.Metadata) && typeName.GetGenericArguments().GetClassName() == nameof(IMetadata))
        {
            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .AddParameter("name", typeof(string))
                .AddParameter("value", typeof(object), true)
                .WithInitializeExpression("Add{4}(new ModelFramework.Common.Builders.MetadataBuilder().WithName(name).WithValue(value));")
                .Build());
        }

        if (propertyName == nameof(IParametersContainer.Parameters) && typeName.GetGenericArguments().GetClassName() == nameof(IParameter))
        {
            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("AddParameter") //if we omit this, then the method name would be AddParameters
                .AddParameter("name", typeof(string))
                .AddParameter("type", typeof(Type))
                .WithInitializeExpression("Add{4}(new ModelFramework.Objects.Builders.ParameterBuilder().WithName(name).WithType(type));")
                .Build());

            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("AddParameter") //if we omit this, then the method name would be AddParameters
                .AddParameter("name", typeof(string))
                .AddParameter("type", typeof(Type))
                .AddParameter("isNullable", typeof(bool))
                .WithInitializeExpression("Add{4}(new ModelFramework.Objects.Builders.ParameterBuilder().WithName(name).WithType(type).WithIsNullable(isNullable));")
                .Build());

            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("AddParameter") //if we omit this, then the method name would be AddParameters
                .AddParameter("name", typeof(string))
                .AddParameter("typeName", typeof(string))
                .WithInitializeExpression("Add{4}(new ModelFramework.Objects.Builders.ParameterBuilder().WithName(name).WithTypeName(typeName));")
                .Build());

            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("AddParameter") //if we omit this, then the method name would be AddParameters
                .AddParameter("name", typeof(string))
                .AddParameter("typeName", typeof(string))
                .AddParameter("isNullable", typeof(bool))
                .WithInitializeExpression("Add{4}(new ModelFramework.Objects.Builders.ParameterBuilder().WithName(name).WithTypeName(typeName).WithIsNullable(isNullable));")
                .Build());
        }

        if (propertyName == nameof(ICodeStatementsContainer.CodeStatements) && typeName.GetGenericArguments().GetClassName() == nameof(ICodeStatementBase))
        {
            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("AddLiteralCodeStatements") //if we omit this, then the method name would be AddCodeStatements
                .AddParameters(new ModelFramework.Objects.Builders.ParameterBuilder().WithName("statements").WithType(typeof(string[])).WithIsParamArray())
                .WithInitializeExpression("Add{4}(ModelFramework.Objects.Extensions.EnumerableOfStringExtensions.ToLiteralCodeStatementBuilders(statements));")
                .Build());
            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("AddLiteralCodeStatements") //if we omit this, then the method name would be AddCodeStatements
                .AddParameter("statements", typeof(IEnumerable<string>))
                .WithInitializeExpression("AddLiteralCodeStatements(statements.ToArray());")
                .Build());
        }
    }
}
