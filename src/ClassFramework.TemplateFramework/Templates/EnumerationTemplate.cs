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

        var attributes = Model.Data.Attributes.Select(attribute => new AttributeViewModel(attribute, Model.Settings, _csharpExpressionCreator));
        Context.Engine.RenderCsharpChildTemplates(attributes, new StringBuilderEnvironment(builder), Context);

        builder.AppendLine($"        {Model.Data.GetModifiers()}enum {Model.Name}");
        builder.AppendLine("        {");

        foreach (var member in Model.Data.Members)
        {
            var valueExpression = member.Value is null
                ? string.Empty
                : $" = {_csharpExpressionCreator.Create(member.Value)}";

            builder.AppendLine($"            {member.Name.Sanitize().GetCsharpFriendlyName()}{valueExpression},");
        }

        builder.AppendLine("        }");
    }
}
