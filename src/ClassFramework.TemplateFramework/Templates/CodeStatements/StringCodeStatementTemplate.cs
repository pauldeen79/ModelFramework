namespace ClassFramework.TemplateFramework.Templates.CodeStatements;

public class StringCodeStatementTemplate : CsharpClassGeneratorBase<StringCodeStatement>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);

        builder.AppendLine($"            {Model.Statement}");
    }
}
