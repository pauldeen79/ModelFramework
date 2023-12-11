namespace ClassFramework.TemplateFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClassFrameworkTemplates(this IServiceCollection services)
        => services
            // Add support for viewmodels in TemplateFramework
            .AddTransient<ITemplateParameterConverter>(x => new ViewModelTemplateParameterConverter(() => x.GetServices<IViewModel>()))

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
            .AddChildTemplate<AttributeTemplate>(typeof(Domain.Attribute))
            .AddChildTemplate<ClassConstructorTemplate>(typeof(ClassConstructor))
            .AddChildTemplate<ClassFieldTemplate>(typeof(ClassField))
            .AddChildTemplate<ClassMethodTemplate>(typeof(ClassMethod))
            .AddChildTemplate<ClassPropertyTemplate>(typeof(ClassProperty))
            .AddChildTemplate<CodeGenerationHeaderTemplate>(typeof(CodeGenerationHeaderModel))
            .AddChildTemplate<EnumerationMemberTemplate>(typeof(EnumerationMember))
            .AddChildTemplate<EnumerationTemplate>(typeof(Enumeration))
            .AddChildTemplate<NewLineTemplate>(typeof(NewLineModel))
            .AddChildTemplate<ParameterTemplate>(typeof(Parameter))
            .AddChildTemplate<PropertyCodeBodyTemplate>(typeof(PropertyCodeBodyModel))
            .AddChildTemplate<SpaceAndCommaTemplate>(typeof(SpaceAndCommaModel))
            .AddChildTemplate<TypeBaseTemplate>(typeof(TypeBase))
            .AddChildTemplate<UsingsTemplate>(typeof(UsingsModel))
            .AddChildTemplate<StringCodeStatementTemplate>(typeof(StringCodeStatement));
}
