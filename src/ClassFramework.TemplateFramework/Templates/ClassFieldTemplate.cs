namespace ClassFramework.TemplateFramework.Templates;

public class ClassFieldTemplate : CsharpClassGeneratorBase<ClassFieldViewModel>, IStringBuilderTemplate
{
    private readonly ICsharpExpressionCreator _csharpExpressionCreator;

    public ClassFieldTemplate(ICsharpExpressionCreator csharpExpressionCreator)
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
        Context.Engine.RenderCsharpChildTemplates(attributes, new StringBuilderEnvironment(builder), Context);

        builder.Append($"        {Model.Data.GetModifiers()}");
        
        if (Model.Data.Event)
        {
            builder.Append("event ");
        }
        
        builder.Append($"{Model.TypeName} {Model.Name}");
        
        if (Model.ShouldRenderDefaultValue)
        {
            builder.Append($" = {Model.GetDefaultValueExpression()}");
        }
        
        builder.AppendLine(";");
    }
}
