namespace DatabaseFramework.TemplateFramework.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabaseFrameworkTemplates(this IServiceCollection services)
        => services
            // Add support for viewmodels in TemplateFramework
            .AddSingleton<ITemplateParameterConverter>(x => new ViewModelTemplateParameterConverter(() => x.GetServices<IViewModel>()))

            .AddTransient<DatabaseSchemaGenerator>()
            .AddTransient<IViewModel, CheckConstraintViewModel>()
            .AddTransient<IViewModel, CodeGenerationHeaderViewModel>()
            .AddTransient<IViewModel, DefaultValueConstraintViewModel>()
            .AddTransient<IViewModel, ForeignKeyConstraintFieldViewModel>()
            .AddTransient<IViewModel, ForeignKeyConstraintViewModel>()
            .AddTransient<IViewModel, IndexFieldViewModel>()
            .AddTransient<IViewModel, IndexViewModel>()
            .AddTransient<IViewModel, NewLineViewModel>()
            .AddTransient<IViewModel, SpaceAndCommaViewModel>()
            .AddTransient<IViewModel, TableViewModel>()
            .AddTransient<IViewModel, TableFieldViewModel>()
            .AddChildTemplate<CheckConstraintTemplate>(typeof(CheckConstraint))
            .AddChildTemplate<CodeGenerationHeaderTemplate>(typeof(CodeGenerationHeaderModel))
            .AddChildTemplate<DefaultValueConstraintTemplate>(typeof(DefaultValueConstraint))
            .AddChildTemplate<ForeignKeyConstraintFieldTemplate>(typeof(ForeignKeyConstraintField))
            .AddChildTemplate<ForeignKeyConstraintTemplate>(typeof(ForeignKeyConstraintModel))
            .AddChildTemplate<IndexFieldTemplate>(typeof(IndexField))
            .AddChildTemplate<IndexTemplate>(typeof(Domain.Index))
            .AddChildTemplate<NewLineTemplate>(typeof(NewLineModel))
            .AddChildTemplate<SpaceAndCommaTemplate>(typeof(SpaceAndCommaModel))
            .AddChildTemplate<TableTemplate>(typeof(Table))
            .AddChildTemplate<TableFieldTemplate>(typeof(TableField))
            ;
}
