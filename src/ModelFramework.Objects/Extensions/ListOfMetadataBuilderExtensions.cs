namespace ModelFramework.Objects.Extensions;

public static class ListOfMetadataBuilderExtensions
{
    public static List<MetadataBuilder> Replace(this List<MetadataBuilder> instance, string name, object? newValue)
        => instance.Chain(() =>
        {
            instance.RemoveAll(x => x.Name == name);
            if (newValue != null)
            {
                instance.Add(new MetadataBuilder().WithName(name).WithValue(newValue));
            }
        });
}
