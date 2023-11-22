namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public abstract class ClassFrameworkCSharpClassBase : CSharpClassBase
{
    public override bool RecurseOnDeleteGeneratedFiles => false;
    public override string DefaultFileName => string.Empty; // not used because we're using multiple files, but it's abstract so we need to fill it

    protected override bool CreateCodeGenerationHeader => true;
    protected override bool EnableNullableContext => true;
    protected override Type RecordCollectionType => typeof(IReadOnlyCollection<>);
    protected override Type RecordConcreteCollectionType => typeof(List<>);
    protected override string FileNameSuffix => Constants.TemplateGenerated;
    protected override string ProjectName => Constants.ProjectName;
    protected override bool UseLazyInitialization => false; // we don't want lazy stuff, just getters and setters
    protected override bool AddBackingFieldsForCollectionProperties => true; // we just want static stuff - else you need to choose builders or models instead of entities
    protected override string? CollectionPropertyGetStatement => "return _[NamePascal]?.AsReadOnly()!;";
    protected override bool AddPrivateSetters => false; // we just want static stuff - else you need to choose builders or models instead of entities
    protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.DomainOnly;
    protected override bool ConvertStringToStringBuilderOnBuilders => false; // we don't want string builders, just strings
    protected override bool AddNullChecks => true;

    protected override void Visit<TBuilder, TEntity>(ModelFramework.Objects.Builders.TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
    {
        Guard.IsNotNull(typeBaseBuilder);

        Type? sourceModel = null;
        Type[] interfaces;

        if (typeBaseBuilder.Name.EndsWith(BuilderName))
        {
            if (typeBaseBuilder is ModelFramework.Objects.Builders.ClassBuilder classBuilder)
            {
                classBuilder.AddMethods(new ModelFramework.Objects.Builders.ClassMethodBuilder().WithName("SetDefaultValues").WithPartial().WithVisibility(ModelFramework.Objects.Contracts.Visibility.Private));
                classBuilder.Constructors.First(x => x.Parameters.Count == 0).CodeStatements.Add(new ModelFramework.Objects.CodeStatements.Builders.LiteralCodeStatementBuilder("SetDefaultValues();"));
            }

            sourceModel = Array.Find(GetType().Assembly.GetTypes(), x => x.Name == $"I{typeBaseBuilder.Name.ReplaceSuffix(BuilderName, string.Empty, StringComparison.Ordinal)}");
            if (sourceModel is null)
            {
                return;
            }

            interfaces = sourceModel.GetInterfaces();
            foreach (var i in interfaces.Where(x => x.FullName is not null && x.FullName.Contains($"{CodeGenerationRootNamespace}.Models.Abstractions.", StringComparison.Ordinal)))
            {
                typeBaseBuilder.AddInterfaces(i.FullName!.Replace($"{CodeGenerationRootNamespace}.Models.Abstractions.", $"{RootNamespace}.{BuildersName}.Abstractions.", StringComparison.Ordinal) + BuilderName);
            }

            return;
        }

        ConvertBuilderFactoriesToToBuilderMethod(typeBaseBuilder);

        sourceModel = Array.Find(GetType().Assembly.GetTypes(), x => x.Name == $"I{typeBaseBuilder.Name}");
        if (sourceModel is null)
        {
            return;
        }

        interfaces = sourceModel.GetInterfaces();
        foreach (var i in interfaces.Where(x => x.FullName is not null && x.FullName.Contains($"{CodeGenerationRootNamespace}.Models.Abstractions.", StringComparison.Ordinal)))
        {
            typeBaseBuilder.AddInterfaces(i.FullName!
                .Replace($"{CodeGenerationRootNamespace}.Models.Abstractions.", $"{RootNamespace}.Abstractions.", StringComparison.Ordinal)
                .Replace($"{CodeGenerationRootNamespace}.Models.", $"{RootNamespace}.", StringComparison.Ordinal));
        }
    }

    protected ModelFramework.Objects.Contracts.ITypeBase[] GetPipelinesModels()
        => MapCodeGenerationModelsToDomain(
            GetType().Assembly.GetTypes()
                .Where(x => x.IsInterface && x.Namespace == $"{CodeGenerationRootNamespace}.Models.Pipelines" && !GetCustomBuilderTypes().Contains(x.GetEntityClassName())));

    private void ConvertBuilderFactoriesToToBuilderMethod<TBuilder, TEntity>(ModelFramework.Objects.Builders.TypeBaseBuilder<TBuilder, TEntity> typeBaseBuilder)
        where TBuilder : ModelFramework.Objects.Builders.TypeBaseBuilder<TBuilder, TEntity>
        where TEntity : ModelFramework.Objects.Contracts.ITypeBase
    {
        foreach (var property in typeBaseBuilder.Properties)
        {
            if (!string.IsNullOrEmpty(GetEntityClassName(property.TypeName)))
            {
                var value = property.IsNullable
                    ? "if (source.{0} is not null) {0} = source.{0}.ToBuilder()"
                    : "{0} = source.{0}.ToBuilder()";

                property.Metadata.Replace(ModelFramework.Objects.MetadataNames.CustomBuilderConstructorInitializeExpression, value);
            }

            if (!string.IsNullOrEmpty(GetEntityClassName(property.TypeName.GetGenericArguments())))
            {
                var value = property.IsNullable
                    ? "if (source.{0} is not null) {0}.AddRange(source.{0}.Select(x => x.ToBuilder()))"
                    : "{0}.AddRange(source.{0}.Select(x => x.ToBuilder()))";

                property.Metadata.Replace(ModelFramework.Objects.MetadataNames.CustomBuilderConstructorInitializeExpression, value);
            }
        }
    }
}
