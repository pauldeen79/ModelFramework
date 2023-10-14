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
            foreach (var i in interfaces.Where(x => x.FullName is not null && !x.Name.EndsWith("Base")))
            {
                typeBaseBuilder.AddInterfaces(i.FullName!.Replace($"{CodeGenerationRootNamespace}.Models.Abstractions.", $"{RootNamespace}.{BuildersName}.Abstractions.", StringComparison.Ordinal) + BuilderName);
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
            typeBaseBuilder.AddInterfaces(i.FullName!
                .Replace($"{CodeGenerationRootNamespace}.Models.Abstractions.", $"{RootNamespace}.Abstractions.", StringComparison.Ordinal)
                .Replace($"{CodeGenerationRootNamespace}.Models.", $"{RootNamespace}.", StringComparison.Ordinal));
        }
    }
}
