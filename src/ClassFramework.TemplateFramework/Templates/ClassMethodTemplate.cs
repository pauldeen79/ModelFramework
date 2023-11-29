namespace ClassFramework.TemplateFramework.Templates;

public class ClassMethodTemplate : CsharpClassGeneratorBase<ClassMethodViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        Context.Engine.RenderChildTemplatesByModel(Model.GetAttributeModels(), new StringBuilderEnvironment(builder), Context);

        builder.Append(Model.CreateIndentation(1));
        builder.Append(Model.Data.GetModifiers());
        builder.Append(Model.ReturnTypeName);
        builder.Append(" ");
        builder.Append(Model.ExplicitInterfaceName);
        builder.Append(Model.Name);
        builder.Append(Model.Data.GetGenericTypeArgumentsString());
        builder.Append("(");

        /*        <# if (ViewModel.ShouldRenderModifiers) { #><#= Model.GetModifiers() #><# } #><#= ViewModel.ReturnTypeName #> <# if (ViewModel.ShouldRenderExplicitInterfaceName) { #><#= Model.ExplicitInterfaceName #>.<# } #><#= ViewModel.Name #><#= Model.GetGenericTypeArgumentsString() #>(<# if (Model.ExtensionMethod) { #>this <# } #><#@ renderChildTemplate name="CSharpClassGenerator.DefaultParameterTemplate" model="Model.Parameters" separatorTemplateName="CommaAndSpace" customResolverDelegate="ResolveFromMetadata" #>)<#= Model.GetGenericTypeArgumentConstraintsString() #><# if (ViewModel.OmitCode) { #>;<# } else { #>

        {<# WriteLine("");if (ViewModel.ShouldRenderCodeStatements) { #><# RootTemplate.PushIndent("            "); #><#@ renderChildTemplate model="Model.CodeStatements" customResolverDelegate="ResolveFromMetadata" customRenderChildTemplateDelegate="RenderModel" #><# RootTemplate.PopIndent(); #>
<# } #>        }<# } #>

    */
    }
}
