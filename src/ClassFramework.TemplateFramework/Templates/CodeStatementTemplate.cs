namespace ClassFramework.TemplateFramework.Templates;

public class CodeStatementTemplate : CsharpClassGeneratorBase<CodeStatementViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        // Note that we render the model in the Data property of the ViewModel.
        // This means you have to register each type of CodeStatementBase type in the DI container.
        Context.Engine.RenderChildTemplateByModel(Model.GetModel(), new StringBuilderEnvironment(builder), Context);
    }
}
