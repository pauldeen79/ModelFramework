namespace ClassFramework.TemplateFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClassFrameworkTemplates(this IServiceCollection services)
        => services
            .AddScoped<CsharpClassGenerator>()
            .AddScoped<IViewModelFactory, ViewModelFactory>()
            .AddScoped<IViewModelCreator, AttributeViewModelCreator>()
            .AddScoped<IViewModelCreator, ClassConstructorViewModelCreator>()
            .AddScoped<IViewModelCreator, ClassFieldViewModelCreator>()
            .AddScoped<IViewModelCreator, ClassMethodViewModelCreator>()
            .AddScoped<IViewModelCreator, ClassPropertyViewModelCreator>()
            .AddScoped<IViewModelCreator, CodeGenerationHeaderViewModelCreator>()
            .AddScoped<IViewModelCreator, CodeStatementViewModelCreator>()
            .AddScoped<IViewModelCreator, EnumerationViewModelCreator>()
            .AddScoped<IViewModelCreator, NewLineViewModelCreator>()
            .AddScoped<IViewModelCreator, ParameterViewModelCreator>()
            .AddScoped<IViewModelCreator, SpaceAndCommaViewModelCreator>()
            .AddScoped<IViewModelCreator, TypeBaseViewModelCreator>()
            .AddScoped<IViewModelCreator, UsingsViewModelCreator>()
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
