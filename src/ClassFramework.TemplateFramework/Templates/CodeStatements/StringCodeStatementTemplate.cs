namespace ClassFramework.TemplateFramework.Templates.CodeStatements;

public class StringCodeStatementTemplate : CsharpClassGeneratorBase<StringCodeStatementViewModel>, IStringBuilderTemplate
{
    public StringCodeStatementTemplate(IViewModelFactory viewModelFactory) : base(viewModelFactory)
    {
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        var vm = Context.GetModelFromContextByType<CsharpClassGeneratorViewModelBase>();
        if (vm is not null)
        {
            builder.Append(vm.CreateIndentation(2));
        }

        builder.AppendLine(Model.Statement);
    }
}
