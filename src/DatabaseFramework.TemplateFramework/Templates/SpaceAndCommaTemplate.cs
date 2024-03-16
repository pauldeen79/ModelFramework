namespace DatabaseFramework.TemplateFramework.Templates;

public class SpaceAndCommaTemplate : DatabaseSchemaGeneratorBase<SpaceAndCommaViewModel>, IStringBuilderTemplate
{
    public void Render(StringBuilder builder)
    {
        Guard.IsNotNull(builder);

        builder.Append(", ");
    }
}
