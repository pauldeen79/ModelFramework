namespace ClassFramework.TemplateFramework.Templates;

public class CodeStatementTemplate : CsharpClassGeneratorBase<CodeStatementViewModel>, IStringBuilderTemplate
{
    public CodeStatementTemplate(IViewModelFactory viewModelFactory) : base(viewModelFactory)
    {
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        // Note that we render the model in the Data property of the ViewModel.
        // This means you have to register each type of CodeStatementBase type in the DI container.
        //TODO: Check if we can do this using the method in the baseclass, without getting a viewmodel
        var model = Model.GetModel();
        Context.Engine.RenderChildTemplate(model, new StringBuilderEnvironment(builder), Context, new TemplateByModelIdentifier(model));
    }
}
