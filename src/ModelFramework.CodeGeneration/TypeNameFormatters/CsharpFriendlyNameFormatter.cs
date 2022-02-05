namespace ModelFramework.CodeGeneration.TypeNameFormatters;

public class CsharpFriendlyNameFormatter : ITypeNameFormatter
{
    public string? Format(string currentValue)
        => currentValue.GetCsharpFriendlyTypeName();
}
