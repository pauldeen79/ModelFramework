namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class PipelinesBuilders : ClassFrameworkCSharpClassBase
{
    public override string Path => Constants.Paths.PipelinesBuilders;
    protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.DomainOnly;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetPipelinesModels(),
            Constants.Namespaces.Pipelines,
            Constants.Namespaces.PipelinesBuilders);
}
