namespace DatabaseFramework.TemplateFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClassFrameworkTemplates(this IServiceCollection services)
        => services
            // Add support for viewmodels in TemplateFramework
            .AddSingleton<ITemplateParameterConverter>(x => new ViewModelTemplateParameterConverter(() => x.GetServices<IViewModel>()))

            //.AddTransient<CsharpClassGenerator>()
            //.AddTransient<IViewModel, AttributeViewModel>()
            //...
            //.AddChildTemplate<AttributeTemplate>(typeof(Domain.Attribute))
            //...
            ;
}
