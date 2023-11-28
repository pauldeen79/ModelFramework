namespace ClassFramework.TemplateFramework.Templates;

public class CodeStatementTemplate : CsharpClassGeneratorBase<CodeStatementViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        Context.Engine.RenderCsharpChildTemplate(Model.Data, new StringBuilderEnvironment(builder), Context);
    }
}
