namespace ClassFramework.Domain.Builders.Types;

public partial class ClassBuilder
{
    public ClassBuilder WithBaseClass(System.Type baseClassType)
        => WithBaseClass(baseClassType.IsNotNull(nameof(baseClassType)).FullName);
}
