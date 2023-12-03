namespace ClassFramework.TemplateFramework.Models;

public class UsingsModel : ICustomModelProvider
{
    private readonly IEnumerable<TypeBase> _types;

    public UsingsModel(IEnumerable<TypeBase> types)
    {
        Guard.IsNotNull(types);

        _types = types;
    }

    public object Create() => _types;
}
