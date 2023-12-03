namespace ClassFramework.TemplateFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClassFrameworkTemplates(this IServiceCollection services)
        => services
            .AddScoped<CsharpClassGenerator>()
            .AddScoped<IViewModelFactory, ViewModelFactory>()
            .AddScoped<IViewModelFactoryComponent, AttributeViewModelFactoryComponent>()
            .AddScoped<IViewModelFactoryComponent, ClassConstructorViewModelFactoryComponent>()
            .AddScoped<IViewModelFactoryComponent, ClassFieldViewModelFactoryComponent>()
            .AddScoped<IViewModelFactoryComponent, ClassMethodViewModelFactoryComponent>()
            .AddScoped<IViewModelFactoryComponent, ClassPropertyViewModelFactoryComponent>()
            .AddScoped<IViewModelFactoryComponent, CodeGenerationHeaderViewModelFactoryComponent>()
            .AddScoped<IViewModelFactoryComponent, CodeStatementViewModelFactoryComponent>()
            .AddScoped<IViewModelFactoryComponent, EnumerationViewModelFactoryComponent>()
            .AddScoped<IViewModelFactoryComponent, NewLineViewModelFactoryComponent>()
            .AddScoped<IViewModelFactoryComponent, ParameterViewModelFactoryComponent>()
            .AddScoped<IViewModelFactoryComponent, SpaceAndCommaViewModelLocatorComponent>()
            .AddScoped<IViewModelFactoryComponent, TypeBaseViewModelFactoryComponent>()
            .AddScoped<IViewModelFactoryComponent, UsingsViewModelFactoryComponent>()
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
