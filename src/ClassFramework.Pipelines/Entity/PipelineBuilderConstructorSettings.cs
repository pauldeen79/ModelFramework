namespace ClassFramework.Pipelines.Entity;

public record PipelineBuilderConstructorSettings
{
    public ArgumentValidationType ValidateArguments { get; }
    public ArgumentValidationType OriginalValidateArguments { get; }
    public string CollectionTypeName { get; }

    public PipelineBuilderConstructorSettings(
        ArgumentValidationType validateArguments = ArgumentValidationType.None,
        ArgumentValidationType? originalValidateArguments = null,
        string collectionTypeName = "")
    {
        ValidateArguments = validateArguments;
        OriginalValidateArguments = originalValidateArguments ?? validateArguments;
        CollectionTypeName = collectionTypeName.IsNotNull(nameof(collectionTypeName));
    }
}
