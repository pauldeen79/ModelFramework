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

        builder.Append(Model.CreateIndentation(Model.AdditionalIndents));

        builder.AppendLine(Model.Statement);
    }
}
