namespace ClassFramework.TemplateFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClassFrameworkTemplates(this IServiceCollection services)
        => services
            // Add support for viewmodels in TemplateFramework
            .AddScoped<ITemplateParameterConverter, ViewModelTemplateParameterConverter>()
            .AddScoped<ITemplateProviderComponent, ViewModelTemplateProviderComponent>()

            .AddScoped<CsharpClassGenerator>()
            .AddScoped<IViewModel, AttributeViewModel>()
            .AddScoped<IViewModel, ClassConstructorViewModel>()
            .AddScoped<IViewModel, ClassFieldViewModel>()
            .AddScoped<IViewModel, ClassMethodViewModel>()
            .AddScoped<IViewModel, ClassPropertyViewModel>()
            .AddScoped<IViewModel, CodeGenerationHeaderViewModel>()
            .AddScoped<IViewModel, EnumerationMemberViewModel>()
            .AddScoped<IViewModel, EnumerationViewModel>()
            .AddScoped<IViewModel, NewLineViewModel>()
            .AddScoped<IViewModel, ParameterViewModel>()
            .AddScoped<IViewModel, PropertyCodeBodyViewModel>()
            .AddScoped<IViewModel, SpaceAndCommaViewModel>()
            .AddScoped<IViewModel, TypeBaseViewModel>()
            .AddScoped<IViewModel, UsingsViewModel>()
            .AddScoped<IViewModel, StringCodeStatementViewModel>()
            .AddChildTemplate<AttributeTemplate>(typeof(AttributeViewModel))
            .AddChildTemplate<ClassConstructorTemplate>(typeof(ClassConstructorViewModel))
            .AddChildTemplate<ClassFieldTemplate>(typeof(ClassFieldViewModel))
            .AddChildTemplate<ClassMethodTemplate>(typeof(ClassMethodViewModel))
            .AddChildTemplate<ClassPropertyTemplate>(typeof(ClassPropertyViewModel))
            .AddChildTemplate<CodeGenerationHeaderTemplate>(typeof(CodeGenerationHeaderViewModel))
            .AddChildTemplate<EnumerationMemberTemplate>(typeof(EnumerationMemberViewModel))
            .AddChildTemplate<EnumerationTemplate>(typeof(EnumerationViewModel))
            .AddChildTemplate<NewLineTemplate>(typeof(NewLineViewModel))
            .AddChildTemplate<ParameterTemplate>(typeof(ParameterViewModel))
            .AddChildTemplate<PropertyCodeBodyTemplate>(typeof(PropertyCodeBodyViewModel))
            .AddChildTemplate<SpaceAndCommaTemplate>(typeof(SpaceAndCommaViewModel))
            .AddChildTemplate<TypeBaseTemplate>(typeof(TypeBaseViewModel))
            .AddChildTemplate<UsingsTemplate>(typeof(UsingsViewModel))
            .AddChildTemplate<StringCodeStatementTemplate>(typeof(StringCodeStatementViewModel));
}
