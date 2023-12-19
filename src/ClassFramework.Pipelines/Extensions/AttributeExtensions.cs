namespace ClassFramework.Pipelines.Extensions;

public static class AttributeExtensions
{
    public static Domain.Attribute ConvertToDomainAttribute(this System.Attribute instance, Func<System.Attribute, AttributeBuilder> initializeDelegate)
    {
        initializeDelegate = initializeDelegate.IsNotNull(nameof(initializeDelegate));

        var prefilled = initializeDelegate(instance);

        return new AttributeBuilder()
            .WithName(prefilled.Name)
            .AddParameters(prefilled.Parameters)
            .AddMetadata(prefilled.Metadata)
            .Build();
    }
}
