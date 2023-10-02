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
