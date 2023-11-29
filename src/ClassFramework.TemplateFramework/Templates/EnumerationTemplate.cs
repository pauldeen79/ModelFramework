namespace ClassFramework.TemplateFramework.Templates;

public class EnumerationTemplate : CsharpClassGeneratorBase<EnumerationViewModel>, IStringBuilderTemplate
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public EnumerationTemplate(ICsharpExpressionCreator csharpExpressionCreator)
    {
        Guard.IsNotNull(csharpExpressionCreator);
        _csharpExpressionCreator = csharpExpressionCreator;
    }

    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);
        Guard.IsNotNull(Model);
        Guard.IsNotNull(Context);

        var attributes = Model.Data.Attributes.Select(attribute => new AttributeViewModel(attribute, Model.Settings, _csharpExpressionCreator, Model.Data));
        Context.Engine.RenderChildTemplatesByModel(attributes, new StringBuilderEnvironment(builder), Context);

        builder.Append(Model.CreateIndentation(1));
        builder.Append(Model.Data.GetModifiers());
        builder.Append("enum ");
        builder.AppendLine(Model.Name);
        builder.Append(Model.CreateIndentation(1));
        builder.AppendLine("{");

        foreach (var member in Model.Data.Members)
        {
            var valueExpression = member.Value is null
                ? string.Empty
                : $" = {_csharpExpressionCreator.Create(member.Value)}";

            builder.Append(Model.CreateIndentation(2));
            builder.Append(member.Name.Sanitize().GetCsharpFriendlyName());
            builder.Append(valueExpression);
            builder.AppendLine(",");
        }

        builder.Append(Model.CreateIndentation(1));
        builder.AppendLine("}");
    }
}
