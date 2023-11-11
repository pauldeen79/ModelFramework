namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class PipelinesBuilders : ClassFrameworkCSharpClassBase
{
    public override string Path => Constants.Paths.PipelinesBuilders;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetPipelinesModels(),
            Constants.Namespaces.Pipelines,
            Constants.Namespaces.PipelinesBuilders);
}
