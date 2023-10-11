namespace ClassFramework.Domain.Types;

public partial record Class
{
    public override bool IsPoco()
        => (!Properties.Any() || Properties.All(p => p.HasSetter || p.HasInitializer))
        && HasPublicParameterlessConstructor();

    public bool HasPublicParameterlessConstructor()
        => Constructors.Count == 0 || Constructors.Any(x => x.Parameters.Count == 0);
}
