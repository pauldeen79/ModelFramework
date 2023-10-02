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
    }

    private static void AddBuilderOverloads(ModelFramework.Objects.Builders.ClassPropertyBuilder property, string typeName, string propertyName)
    {
        if (propertyName == nameof(IMetadataContainer.Metadata) && $"I{typeName.GetGenericArguments().GetClassName()}" == nameof(IMetadata))
        {
            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .AddParameter("name", typeof(string))
                .AddParameter("value", typeof(object), true)
                .WithInitializeExpression("Add{4}(new ClassFramework.Domain.Builders.MetadataBuilder().WithName(name).WithValue(value));")
                .Build());
        }

        if (propertyName == nameof(IParametersContainer.Parameters) && $"I{typeName.GetGenericArguments().GetClassName()}" == nameof(IParameter))
        {
            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("AddParameter") //if we omit this, then the method name would be AddParameters
                .AddParameter("name", typeof(string))
                .AddParameter("type", typeof(Type))
                .WithInitializeExpression("Add{4}(new ClassFramework.Domain.Builders.ParameterBuilder().WithName(name).WithType(type));")
                .Build());

            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("AddParameter") //if we omit this, then the method name would be AddParameters
                .AddParameter("name", typeof(string))
                .AddParameter("type", typeof(Type))
                .AddParameter("isNullable", typeof(bool))
                .WithInitializeExpression("Add{4}(new ClassFramework.Domain.Builders.ParameterBuilder().WithName(name).WithType(type).WithIsNullable(isNullable));")
                .Build());

            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("AddParameter") //if we omit this, then the method name would be AddParameters
                .AddParameter("name", typeof(string))
                .AddParameter("typeName", typeof(string))
                .WithInitializeExpression("Add{4}(new ClassFramework.Domain.Builders.ParameterBuilder().WithName(name).WithTypeName(typeName));")
                .Build());

            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("AddParameter") //if we omit this, then the method name would be AddParameters
                .AddParameter("name", typeof(string))
                .AddParameter("typeName", typeof(string))
                .AddParameter("isNullable", typeof(bool))
                .WithInitializeExpression("Add{4}(new ClassFramework.Domain.Builders.ParameterBuilder().WithName(name).WithTypeName(typeName).WithIsNullable(isNullable));")
                .Build());
        }

        if (propertyName == nameof(ICodeStatementsContainer.CodeStatements) && $"I{typeName.GetGenericArguments().GetClassName()}" == nameof(ICodeStatementBase))
        {
            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("AddStringCodeStatements") //if we omit this, then the method name would be AddCodeStatements
                .AddParameters(new ModelFramework.Objects.Builders.ParameterBuilder().WithName("statements").WithType(typeof(string[])).WithIsParamArray())
                .WithInitializeExpression("Add{4}(ClassFramework.Domain.Builders.Extensions.EnumerableOfStringExtensions.ToStringCodeStatementBuilders(statements));")
                .Build());
            property.AddBuilderOverload(new ModelFramework.Objects.Builders.OverloadBuilder()
                .WithMethodName("AddStringCodeStatements") //if we omit this, then the method name would be AddCodeStatements
                .AddParameter("statements", typeof(IEnumerable<string>))
                .WithInitializeExpression("AddStringCodeStatements(statements.ToArray());")
                .Build());
        }
    }

    protected override void Visit<TBuilder, TEntity>(ModelFramework.Objects.Builders.TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
    {
        Guard.IsNotNull(typeBaseBuilder);
        
        Type? sourceModel = null;
        Type[] interfaces;
        
        if (typeBaseBuilder.Name.EndsWith("Builder"))
        {
            sourceModel = Array.Find(GetType().Assembly.GetTypes(), x => x.Name == $"I{typeBaseBuilder.Name.ReplaceSuffix("Builder", string.Empty, StringComparison.Ordinal)}");
            if (sourceModel is null)
            {
                return;
            }

            interfaces = sourceModel.GetInterfaces();
            foreach (var i in interfaces.Where(x => x.FullName is not null && !x.Name.EndsWith("Base")))
            {
                typeBaseBuilder.AddInterfaces(i.FullName!.Replace("ClassFramework.CodeGeneration.Models.Abstractions.", $"{Constants.Namespaces.DomainBuilders}.Abstractions.", StringComparison.Ordinal) + "Builder");
            }

            return;
        }

        sourceModel = Array.Find(GetType().Assembly.GetTypes(), x => x.Name == $"I{typeBaseBuilder.Name}");
        if (sourceModel is null)
        {
            return;
        }

        interfaces = sourceModel.GetInterfaces();
        foreach (var i in interfaces.Where(x => x.FullName is not null && !x.Name.EndsWith("Base")))
        {
            typeBaseBuilder.AddInterfaces(i.FullName!.Replace("ClassFramework.CodeGeneration.Models.Abstractions.", $"{Constants.Namespaces.Domain}.Abstractions.", StringComparison.Ordinal));
        }
    }
}
