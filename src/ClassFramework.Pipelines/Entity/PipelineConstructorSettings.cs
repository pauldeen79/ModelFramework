namespace ClassFramework.Pipelines.Entity;

public class PipelineConstructorSettings
{
    public ArgumentValidationType ValidateArguments { get; }
    public ArgumentValidationType OriginalValidateArguments { get; }
    public string CollectionTypeName { get; }
    public bool AddFullConstructor { get; }
    public bool AddPublicParameterlessConstructor { get; }

    public PipelineConstructorSettings(
        ArgumentValidationType validateArguments = ArgumentValidationType.None,
        ArgumentValidationType? originalValidateArguments = null,
        string collectionTypeName = "",
        bool addFullConstructor = true,
        bool addPublicParameterlessConstructor = false)
    {
        ValidateArguments = validateArguments;
        OriginalValidateArguments = originalValidateArguments ?? validateArguments;
        CollectionTypeName = collectionTypeName.IsNotNull(nameof(collectionTypeName));
        AddFullConstructor = addFullConstructor;
        AddPublicParameterlessConstructor = addPublicParameterlessConstructor;
    }
}
