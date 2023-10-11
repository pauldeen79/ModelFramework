namespace ClassFramework.Domain;

public record Literal
{
    public string? Value { get; }
    public object? OriginalValue { get; }

    public Literal(string? value, object? originalValue = null)
    {
        Value = value;
        OriginalValue = originalValue;
    }
}
