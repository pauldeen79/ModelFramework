namespace ClassFramework.TemplateFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClassFrameworkTemplates(this IServiceCollection services)
        => services
            .AddChildTemplate<AttributeTemplate>(typeof(Domain.Attribute))
            .AddChildTemplate<ClassConstructorTemplate>(typeof(ClassConstructor))
            .AddChildTemplate<ClassFieldTemplate>(typeof(ClassField))
            .AddChildTemplate<ClassMethodTemplate>(typeof(ClassMethod))
            .AddChildTemplate<ClassPropertyTemplate>(typeof(ClassProperty))
            .AddChildTemplate<CodeGenerationHeaderTemplate>(nameof(CodeGenerationHeaderTemplate))
            .AddChildTemplate<EnumerationTemplate>(typeof(Enumeration))
            .AddChildTemplate<TypeBaseTemplate>(typeof(TypeBase))
            .AddChildTemplate<UsingsTemplate>(nameof(UsingsTemplate));
}
