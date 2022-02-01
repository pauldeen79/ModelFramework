namespace ModelFramework.Objects.Builders;

public partial class ClassConstructorBuilder
{
    public ClassConstructorBuilder ChainCallToBaseUsingParameters()
        => WithChainCall($"base({string.Join(", ", Parameters.Select(x => x.Name))})");
}
