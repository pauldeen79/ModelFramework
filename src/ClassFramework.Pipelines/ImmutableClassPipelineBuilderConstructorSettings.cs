namespace ClassFramework.Pipelines;

public record ImmutableClassPipelineBuilderConstructorSettings
{
    public ArgumentValidationType ValidateArguments { get; }
    public ArgumentValidationType OriginalValidateArguments { get; }
    public bool AddNullChecks { get; }
    public string CollectionTypeName { get; }

    public ImmutableClassPipelineBuilderConstructorSettings(
        ArgumentValidationType validateArguments = ArgumentValidationType.None,
        ArgumentValidationType? originalValidateArguments = null,
        bool addNullChecks = false,
        string collectionTypeName = "")
    {
        ValidateArguments = validateArguments;
        OriginalValidateArguments = originalValidateArguments ?? validateArguments;
        AddNullChecks = addNullChecks;
        CollectionTypeName = collectionTypeName.IsNotNull(nameof(collectionTypeName));
    }
}
