namespace ClassFramework.TemplateFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClassFrameworkTemplates(this IServiceCollection services)
        => services
            .AddChildTemplate<CodeGenerationHeaderTemplate>("CodeGenerationHeader")
            .AddChildTemplate<UsingsTemplate>("Usings")
            .AddChildTemplate<TypeBaseTemplate>(typeof(TypeBase))
            .AddChildTemplate<AttributeTemplate>(typeof(Domain.Attribute));
}
