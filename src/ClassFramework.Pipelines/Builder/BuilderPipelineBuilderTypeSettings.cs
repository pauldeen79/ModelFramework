namespace ClassFramework.Pipelines.Builder;

public record BuilderPipelineBuilderTypeSettings
{
    public string NewCollectionTypeName { get; }
    public bool UseTargetTypeNewExpressions { get; }
    public bool EnableNullableReferenceTypes { get; }
    public Func<TypeBase, bool, string>? FormatInstanceTypeNameDelegate { get; }

    public BuilderPipelineBuilderTypeSettings(
        string newCollectionTypeName = "System.Collections.Generic.List",
        bool useTargetTypeNewExpressions = true,
        bool enableNullableReferenceTypes = false,
        Func<TypeBase, bool, string>? formatInstanceTypeNameDelegate = null)
    {
        NewCollectionTypeName = newCollectionTypeName;
        UseTargetTypeNewExpressions = useTargetTypeNewExpressions;
        EnableNullableReferenceTypes = enableNullableReferenceTypes;
        FormatInstanceTypeNameDelegate = formatInstanceTypeNameDelegate;
    }
}
