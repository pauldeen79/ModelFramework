namespace ClassFramework.Pipelines.Entity;

public record PipelineConstructorSettings
{
    public ArgumentValidationType ValidateArguments { get; }
    public ArgumentValidationType OriginalValidateArguments { get; }
    public string CollectionTypeName { get; }

    public PipelineConstructorSettings(
        ArgumentValidationType validateArguments = ArgumentValidationType.None,
        ArgumentValidationType? originalValidateArguments = null,
        string collectionTypeName = "")
    {
        ValidateArguments = validateArguments;
        OriginalValidateArguments = originalValidateArguments ?? validateArguments;
        CollectionTypeName = collectionTypeName.IsNotNull(nameof(collectionTypeName));
    }
}
