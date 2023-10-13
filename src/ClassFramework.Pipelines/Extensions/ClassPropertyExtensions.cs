namespace ClassFramework.Pipelines.Extensions;

public static class ClassPropertyExtensions
{
    public static string GetDefaultValue(this ClassProperty property, bool enableNullableReferenceTypes, CultureInfo cultureInfo)
    {
        var md = property.Metadata.FirstOrDefault(x => x.Name == MetadataNames.CustomBuilderDefaultValue);
        if (md is not null && md.Value is not null)
        {
            if (md.Value is Literal literal && literal.Value is not null)
            {
                return literal.Value;
            }

            return md.Value.CsharpFormat(cultureInfo);
        }

        return property.TypeName.GetDefaultValue(property.IsNullable, property.IsValueType, enableNullableReferenceTypes);
    }
}
