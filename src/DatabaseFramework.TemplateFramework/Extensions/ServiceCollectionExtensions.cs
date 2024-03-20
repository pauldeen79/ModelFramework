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
            .AddTransient<IViewModel, NonViewFieldViewModel>()
            .AddTransient<IViewModel, PrimaryKeyConstraintFieldViewModel>()
            .AddTransient<IViewModel, PrimaryKeyConstraintViewModel>()
            .AddTransient<IViewModel, StoredProcedureParameterViewModel>()
            .AddTransient<IViewModel, StoredProcedureViewModel>()
            .AddTransient<IViewModel, StringSqlStatementViewModel>()
            .AddTransient<IViewModel, TableViewModel>()
            .AddTransient<IViewModel, TableFieldViewModel>()
            .AddTransient<IViewModel, UniqueConstraintFieldViewModel>()
            .AddTransient<IViewModel, UniqueConstraintViewModel>()
            .AddTransient<IViewModel, ViewViewModel>()
            .AddChildTemplate<CheckConstraintTemplate>(typeof(CheckConstraint))
            .AddChildTemplate<CodeGenerationHeaderTemplate>(typeof(CodeGenerationHeaderModel))
            .AddChildTemplate<DefaultValueConstraintTemplate>(typeof(DefaultValueConstraint))
            .AddChildTemplate<ForeignKeyConstraintFieldTemplate>(typeof(ForeignKeyConstraintField))
            .AddChildTemplate<ForeignKeyConstraintTemplate>(typeof(ForeignKeyConstraintModel))
            .AddChildTemplate<IndexFieldTemplate>(typeof(IndexField))
            .AddChildTemplate<IndexTemplate>(typeof(Domain.Index))
            .AddChildTemplate<NonViewFieldTemplate>(typeof(NonViewFieldModel))
            .AddChildTemplate<PrimaryKeyConstraintFieldTemplate>(typeof(PrimaryKeyConstraintField))
            .AddChildTemplate<PrimaryKeyConstraintTemplate>(typeof(PrimaryKeyConstraint))
            .AddChildTemplate<StoredProcedureParameterTemplate>(typeof(StoredProcedureParameter))
            .AddChildTemplate<StoredProcedureTemplate>(typeof(StoredProcedure))
            .AddChildTemplate<StringSqlStatementTemplate>(typeof(StringSqlStatement))
            .AddChildTemplate<TableTemplate>(typeof(Table))
            .AddChildTemplate<TableFieldTemplate>(typeof(TableField))
            .AddChildTemplate<UniqueConstraintFieldTemplate>(typeof(UniqueConstraintField))
            .AddChildTemplate<UniqueConstraintTemplate>(typeof(UniqueConstraint))
            .AddChildTemplate<ViewTemplate>(typeof(View))
            ;
}
