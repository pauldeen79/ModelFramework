namespace DatabaseFramework.TemplateFramework.Templates;

public class ViewTemplate : DatabaseObjectTemplateBase<ViewViewModel>
{
    protected override void RenderDatabaseObject(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        RenderChildTemplateByModel(Model.CodeGenerationHeaders, builder);

        builder.AppendLine(@$"SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [{Model.Schema}].[{Model.Name}]
AS");

        if (!string.IsNullOrEmpty(Model.Definition))
        {
            builder.AppendLine(Model.Definition);
        }
        else
        {
            builder.AppendLine($"SELECT{Model.Distinct}{Model.Top} ");
            RenderChildTemplatesByModel(Model.SelectFields, builder);
            RenderChildTemplatesByModel(Model.Conditions, builder);
            RenderChildTemplatesByModel(Model.GroupByFields, builder);
            RenderChildTemplatesByModel(Model.OrderByFields, builder);
            RenderChildTemplatesByModel(Model.GroupByConditions, builder);
        }
    }
}

/*SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [<#= schemaEntity.Name.FormatAsDatabaseIdentifier() #>].[<#= Model.Name.FormatAsDatabaseIdentifier() #>]
AS
<# if (!string.IsNullOrEmpty(Model.Definition))
   { #>
<#= Model.Definition #>
<# }
   else
   { #>
SELECT<# if (Model.Distinct) { #> DISTINCT<# } #><# if (Model.Top.HasValue) { #> TOP <#= Model.Top #><# } #><# if (Model.Top.HasValue && Model.TopPercent) { #> PERCENT<# } #>
<#@ renderChildTemplate name="SqlServerDatabaseSchemaGenerator.DefaultViewSelectFieldsTemplate" model="Model" customResolverDelegate="ResolveFromMetadata" #>
FROM
<#@ renderChildTemplate name="SqlServerDatabaseSchemaGenerator.DefaultViewSourcesTemplate" model="Model" customResolverDelegate="ResolveFromMetadata" #>
<#@ renderChildTemplate name="SqlServerDatabaseSchemaGenerator.DefaultViewConditionsTemplate" model="Model" customResolverDelegate="ResolveFromMetadata" #>
<#@ renderChildTemplate name="SqlServerDatabaseSchemaGenerator.DefaultViewGroupByFieldsTemplate" model="Model" customResolverDelegate="ResolveFromMetadata" #>
<#@ renderChildTemplate name="SqlServerDatabaseSchemaGenerator.DefaultViewOrderByFieldsTemplate" model="Model" customResolverDelegate="ResolveFromMetadata" #>
<# } #>
GO
*/
