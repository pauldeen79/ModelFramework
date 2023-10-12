namespace ClassFramework.Domain.Types;

public partial record Class
{
    public bool HasPublicParameterlessConstructor()
        => Constructors.Count == 0 || Constructors.Any(x => x.Parameters.Count == 0 && x.Visibility == Domains.Visibility.Public);
}
