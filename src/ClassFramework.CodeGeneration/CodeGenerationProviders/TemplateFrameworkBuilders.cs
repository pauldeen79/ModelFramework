namespace ClassFramework.CodeGeneration.CodeGenerationProviders;

[ExcludeFromCodeCoverage]
public class TemplateFrameworkBuilders : ClassFrameworkCSharpClassBase
{
    public override string Path => Constants.Paths.TemplateFrameworkBuilders;
    protected override ArgumentValidationType ValidateArgumentsInConstructor => ArgumentValidationType.DomainOnly;

    public override object CreateModel()
        => GetImmutableBuilderClasses(
            GetTemplateFrameworkModels(),
            Constants.Namespaces.TemplateFramework,
            Constants.Namespaces.TemplateFrameworkBuilders);
}
