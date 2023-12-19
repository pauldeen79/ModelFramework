namespace ClassFramework.TemplateFramework.Models;

public class UsingsModel
{
    public IEnumerable<TypeBase> Types { get; }

    public UsingsModel(IEnumerable<TypeBase> types)
    {
        Guard.IsNotNull(types);

        Types = types;
    }
}
