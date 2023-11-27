namespace ClassFramework.TemplateFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClassFrameworkTemplates(this IServiceCollection services)
        => services
            .AddChildTemplate<CodeGenerationHeaderTemplate>(nameof(CodeGenerationHeaderTemplate))
            .AddChildTemplate<UsingsTemplate>(nameof(UsingsTemplate))
            .AddChildTemplate<TypeBaseTemplate>(typeof(TypeBase))
            .AddChildTemplate<AttributeTemplate>(typeof(Domain.Attribute));
}
