namespace ClassFramework.Domain;

public class Literal
{
    public string? Value { get; }

    public object? OriginalValue { get; }

    public Literal(string? value, object? originalValue = null)
    {
        Value = value;
        OriginalValue = originalValue;
    }

    public override string ToString() => Value ?? "null";
}
