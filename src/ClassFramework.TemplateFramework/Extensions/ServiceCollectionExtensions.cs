namespace ClassFramework.TemplateFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClassFrameworkTemplates(this IServiceCollection services)
        => services
            // Add support for viewmodels in TemplateFramework
            .AddTransient<ITemplateParameterConverter, ViewModelTemplateParameterConverter>()
            .AddTransient<ITemplateProviderComponent, ViewModelTemplateProviderComponent>()

            .AddTransient<CsharpClassGenerator>()
            .AddTransient<IViewModel, AttributeViewModel>()
            .AddTransient<IViewModel, ClassConstructorViewModel>()
            .AddTransient<IViewModel, ClassFieldViewModel>()
            .AddTransient<IViewModel, ClassMethodViewModel>()
            .AddTransient<IViewModel, ClassPropertyViewModel>()
            .AddTransient<IViewModel, CodeGenerationHeaderViewModel>()
            .AddTransient<IViewModel, EnumerationMemberViewModel>()
            .AddTransient<IViewModel, EnumerationViewModel>()
            .AddTransient<IViewModel, NewLineViewModel>()
            .AddTransient<IViewModel, ParameterViewModel>()
            .AddTransient<IViewModel, PropertyCodeBodyViewModel>()
            .AddTransient<IViewModel, SpaceAndCommaViewModel>()
            .AddTransient<IViewModel, TypeBaseViewModel>()
            .AddTransient<IViewModel, UsingsViewModel>()
            .AddTransient<IViewModel, StringCodeStatementViewModel>()
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
