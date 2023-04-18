namespace ModelFramework.Objects.Builders;

public partial class ParameterBuilder
{
    public override string ToString() => $"{GetPrefix()}{TypeName}{GetNullableSuffix()} {Name}";

    private string GetPrefix()
    {
        if (IsRef)
        {
            return "ref ";
        }

        if (IsOut)
        {
            return "out ";
        }

        return string.Empty;
    }

    private string GetNullableSuffix()
        => IsNullable
            ? "?"
            : string.Empty;
}
