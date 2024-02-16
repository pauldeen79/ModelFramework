namespace ClassFramework.Pipelines.BuilderInterface;

public class PipelineTypeSettings : PipelineBuilderTypeSettingsBase
{
    public PipelineTypeSettings(
        string newCollectionTypeName = "System.Collections.Generic.IReadOnlyCollection",
        string collectionInitializationStatementFormatString = "{NullCheck.Source.Argument}foreach (var item in source.[SourceExpression]) {BuilderMemberName}.Add(item)",
        string collectionCopyStatementFormatString = "foreach (var item in {NamePascalCsharpFriendlyName}) {Name}.Add(item);",
        string nonCollectionInitializationStatementFormatString = "{BuilderMemberName} = source.[SourceExpression]", // note that we are not prefixing {NullCheck.Source.Argument}, because we can simply always copy the value, regardless if it's null :)
        bool enableNullableReferenceTypes = false,
        IEnumerable<NamespaceMapping>? namespaceMappings = null,
        IEnumerable<TypenameMapping>? typenameMappings = null)
        : base(newCollectionTypeName, enableNullableReferenceTypes, namespaceMappings, typenameMappings)
    {
        CollectionInitializationStatementFormatString = collectionInitializationStatementFormatString;
        CollectionCopyStatementFormatString = collectionCopyStatementFormatString;
        NonCollectionInitializationStatementFormatString = nonCollectionInitializationStatementFormatString;
    }

    public string CollectionInitializationStatementFormatString { get; }
    public string CollectionCopyStatementFormatString { get; }
    public string NonCollectionInitializationStatementFormatString { get; }
}
