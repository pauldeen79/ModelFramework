namespace ClassFramework.TemplateFramework.Templates.CodeStatements;

public class StringCodeStatementTemplate : CsharpClassGeneratorBase<StringCodeStatement>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        var vm = Context.GetModelFromContextByType<CsharpClassGeneratorViewModel>();
        if (vm is not null)
        {
            builder.Append(vm.CreateIndentation(2));
        }

        builder.AppendLine(Model.Statement);
    }
}
