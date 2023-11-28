namespace ClassFramework.TemplateFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClassFrameworkTemplates(this IServiceCollection services)
        => services
            .AddScoped<CsharpClassGenerator>()
            .AddChildTemplate<AttributeTemplate>(typeof(AttributeViewModel))
            .AddChildTemplate<ClassConstructorTemplate>(typeof(ClassConstructorViewModel))
            .AddChildTemplate<ClassFieldTemplate>(typeof(ClassFieldViewModel))
            .AddChildTemplate<ClassMethodTemplate>(typeof(ClassMethodViewModel))
            .AddChildTemplate<ClassPropertyTemplate>(typeof(ClassPropertyViewModel))
            .AddChildTemplate<CodeGenerationHeaderTemplate>(typeof(CodeGenerationHeaderViewModel))
            .AddChildTemplate<CodeStatementTemplate>(typeof(CodeStatementViewModel))
            .AddChildTemplate<EnumerationTemplate>(typeof(EnumerationViewModel))
            .AddChildTemplate<NewLineTemplate>(typeof(NewLineViewModel))
            .AddChildTemplate<ParameterTemplate>(typeof(ParameterViewModel))
            .AddChildTemplate<SpaceAndCommaTemplate>(typeof(SpaceAndCommaViewModel))
            .AddChildTemplate<TypeBaseTemplate>(typeof(TypeBaseViewModel))
            .AddChildTemplate<UsingsTemplate>(typeof(UsingsViewModel))
            .AddChildTemplate<StringCodeStatementTemplate>(typeof(StringCodeStatement));
}
